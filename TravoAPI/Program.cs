using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TravoAPI.Data;
using TravoAPI.Models;
using TravoAPI.Services;
using TravoAPI.Services.Interfaces;
using System.Text.Json.Serialization;
using TravoAPI.Mapping;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;
using TravoAPI.Repositories;
using TravoAPI.Repositories.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// 1. Add services to the container:

// — Entity Framework Core (SQL Server)
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// — Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDBContext>()
    .AddDefaultTokenProviders();

// — JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SecurityTokenValidators.Clear();
    options.SecurityTokenValidators.Add(new JwtSecurityTokenHandler());

    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
                                      Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])
                                  )
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = ctx =>
        {
            var hdr = ctx.Request.Headers["Authorization"].ToString();
            var log = ctx.HttpContext.RequestServices
                            .GetRequiredService<ILogger<Program>>();
            log.LogInformation("JWT → OnMessageReceived: {hdr}", hdr);
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = ctx =>
        {
            var log = ctx.HttpContext.RequestServices
                            .GetRequiredService<ILogger<Program>>();
            log.LogError(ctx.Exception, "JWT → OnAuthenticationFailed");
            return Task.CompletedTask;
        },
        OnChallenge = ctx =>
        {
            var log = ctx.HttpContext.RequestServices
                            .GetRequiredService<ILogger<Program>>();
            log.LogError(
                "JWT → OnChallenge: error = {err}, description = {desc}",
                ctx.Error,
                ctx.ErrorDescription
            );
            return Task.CompletedTask;
        }
    };
});

// — CORS (allow React app to send Authorization header)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactClient", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader()    // allows “Authorization”
            .AllowCredentials();
    });
});

// — Register Generic + Specific Repositories
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ITripRepository, TripRepository>();
builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();
builder.Services.AddScoped<IPackingRepository, PackingRepository>();
builder.Services.AddScoped<IDestinationRepository, DestinationRepository>();



// — Register Service Layer
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITripService, TripService>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<IPackingService, PackingService>();
builder.Services.AddScoped<IDestinationService, DestinationService>();
builder.Services.AddScoped<IDayPlanService, DayPlanService>();
builder.Services.AddScoped<IPlaceService, PlaceService>();
builder.Services.AddScoped<INoteService, NoteService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

//Mapping
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// — Controllers, Swagger
builder.Services.AddControllers().AddJsonOptions(opts =>
{
    // Serialize all enums as strings, e.g. BudgetStatus.Paid → "Paid"
    opts.JsonSerializerOptions.Converters
        .Add(new JsonStringEnumConverter());

    // Corrected line - removed extra closing parenthesis
    opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

    //EF navigation loops (DayPlan→Place→DayPlan, Place→Note→Place) don’t crash serialization:
    opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 2. Configure the HTTP request pipeline:

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// — Enable CORS *before* authentication so preflights succeed
app.UseCors("AllowReactClient");

app.UseHttpsRedirection();

app.UseStaticFiles(); // so that wwwroot/uploads is publicly readable
app.UseRouting();

// — Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// — Map controllers
app.MapControllers();

app.Run();
