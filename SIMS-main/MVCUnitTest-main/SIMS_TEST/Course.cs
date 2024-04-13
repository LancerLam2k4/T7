using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIMS_Demo.Controllers;
using SIMS_Demo.Models;
using Xunit;

namespace SIMS_Demo.Tests.Controllers
{
    public class CourseControllerTests
    {
        [Fact]
        public void TimeTable_ReturnsViewResult()
        {
            // Arrange
            var controller = new CourseController();

            // Act
            var result = controller.TimeTable() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

    }
}
