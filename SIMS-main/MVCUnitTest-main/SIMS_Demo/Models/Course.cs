namespace SIMS_Demo.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public DateTime Time { get; set; }
        public string Slot { get; set; }
        public string RoomNumber { get; set; }
        public string Teacher { get; set; }
        public Course() { }
    }
}
