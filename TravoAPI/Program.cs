using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;  
using System.Text;
using TravoAPI.Data;
using TravoAPI.Models;

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
        // 1) Log the raw header the server sees:
        OnMessageReceived = ctx =>
        {
            var hdr = ctx.Request.Headers["Authorization"].ToString();
            var log = ctx.HttpContext.RequestServices
                            .GetRequiredService<ILogger<Program>>();
            log.LogInformation("JWT → OnMessageReceived: {hdr}", hdr);
            return Task.CompletedTask;
        },

        // 2) Log any exception thrown while validating:
        OnAuthenticationFailed = ctx =>
        {
            var log = ctx.HttpContext.RequestServices
                            .GetRequiredService<ILogger<Program>>();
            log.LogError(ctx.Exception, "JWT → OnAuthenticationFailed");
            return Task.CompletedTask;
        },

        // 3) Log the challenge details just before sending 401:
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

// — Controllers, Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// 2. Configure the HTTP request pipeline:

// — Swagger (in Development)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// — Enable CORS *before* authentication so preflights succeed
app.UseCors("AllowReactClient");

app.UseHttpsRedirection();

app.UseStaticFiles();

// — Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();


// — Map controllers
app.MapControllers();

app.Run();
