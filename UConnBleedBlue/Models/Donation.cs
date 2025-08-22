namespace UConnBleedBlue.Models
{
    public partial class Donation
    {
        public int PlayerId { get; set; }
        public string? PlayerName { get; set; }
        public string? FinalYear { get; set; }
        public double Amount { get; set; }
    }
}
