namespace UConnBleedBlue.Models
{
    public partial class Player
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? HeadCoach { get; set; }
        public string? FinalYear { get; set; }
        public bool AttendingTailgate { get; set; } = false;
        public int NumberOfTickets { get; set; } = 0;
    }
}
