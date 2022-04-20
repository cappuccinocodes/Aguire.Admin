namespace WebApplication1.Models
{
    public class SleepToShow
    {
        public int Id { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
