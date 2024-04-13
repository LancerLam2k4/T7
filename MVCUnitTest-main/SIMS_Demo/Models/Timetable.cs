public class Timetable
{
    public DateTime Date { get; set; }
    public int Slot { get; set; }
    public string StudentGroup { get; set; }
    public string Instructor { get; set; }
    public string CourseName { get; set; }
    public int SessionNumber { get; set; }
    public string CampusProgramme { get; set; }
    public int Id { get; internal set; }

    public Timetable(){ }
}
