using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SIMS_Demo.Controllers;
using SIMS_Demo.Models;
using Xunit;

namespace SIMS_Demo.Tests.Controllers
{
    public class StudentControllerTests
    {
        [Fact]
        public void Delete_ReturnsRedirectToActionResult_WhenStudentExists()
        {
            // Arrange
            var controller = new StudentController();
            var id = 1; // ID của sinh viên cần xóa

            // Act
            var result = controller.Delete(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void Delete_ReturnsRedirectToActionResult_WhenStudentDoesNotExist()
        {
            // Arrange
            var controller = new StudentController();
            var id = 999; // ID của sinh viên không tồn tại

            // Act
            var result = controller.Delete(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }
    }
}