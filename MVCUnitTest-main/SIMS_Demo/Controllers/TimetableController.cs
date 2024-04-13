using Microsoft.AspNetCore.Mvc;
using SIMS_Demo.Models;
using System;
using System.Collections.Generic;

namespace SIMS_Demo.Controllers
{
    public class TimetableController : Controller
    {
        // Action to display timetable entries
        public IActionResult Index()
        {
            // This is where you retrieve timetable entries from the database or another data source
            List<Timetable> timetableEntries = new List<Timetable>
            {
                new Timetable
                {
                    Id = 1,
                    Date = DateTime.Today,
                    Slot = 1,
                    StudentGroup = "IT0603",
                    Instructor = "TA8",
                    CourseName = "Business Process Support(7428)",
                    SessionNumber = 25,
                    CampusProgramme = "BTEC"
                },
                new Timetable
                {
                    Id = 2,
                    Date = DateTime.Today.AddDays(1),
                    Slot = 2,
                    StudentGroup = "IT0604",
                    Instructor = "TA9",
                    CourseName = "Software Engineering(7429)",
                    SessionNumber = 26,
                    CampusProgramme = "BTEC"
                }
            };

            return View(timetableEntries);
        }

        // Action to display details of a timetable entry
        public IActionResult Detail(int id)
        {
            // This is where you retrieve the details of the specified timetable entry from the database or another data source
            Timetable timetableEntry = new Timetable
            {
                Id = id,
                Date = DateTime.Today,
                Slot = 1,
                StudentGroup = "IT0603",
                Instructor = "TA8",
                CourseName = "Business Process Support(7428)",
                SessionNumber = 25,
                CampusProgramme = "BTEC"
            };

            if (timetableEntry == null)
            {
                return NotFound(); // Return 404 Not Found if the timetable entry is not found
            }

            return View(timetableEntry); // Return the timetable entry details to the Detail view
        }
    }
}
