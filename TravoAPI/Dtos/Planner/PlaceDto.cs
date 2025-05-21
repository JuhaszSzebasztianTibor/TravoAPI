namespace TravoAPI.Dtos.Planner
{
    public class PlaceDto
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public string Time { get; set; }
        public List<NoteDto> Notes { get; set; }
    }
}
