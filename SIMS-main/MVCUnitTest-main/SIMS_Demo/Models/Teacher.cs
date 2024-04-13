using System;
namespace SIMS_Demo.Models
{
	public class Teacher
	{
		public int Id { get; set; }
		public String Name { get; set; }
		public DateTime DoB { get; set; }
		public string Email { get; set; }
		public string Subjet { get; set; }
		public string Department { get; set; }
		
        public Teacher()
		{
		}
	}
}

