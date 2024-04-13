using System;
namespace SIMS_Demo.Models
{
    public class Student
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public DateTime DoB { get; set; }  
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string StudentID { get; set; }
        public string Class { get; set; }
        public string Major{ get; set; }
        

        public Student()
        {
        }

    }
}

