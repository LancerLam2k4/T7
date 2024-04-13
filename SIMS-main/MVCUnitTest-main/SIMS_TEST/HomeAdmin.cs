using Microsoft.AspNetCore.Mvc;
using SIMS_Demo.Controllers;
using SIMS_Demo.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SIMS_Demo.Tests.Controllers
{
    public class HomeAdminControllerTests
    {
        [Fact]
        public void Index_Get_ReturnsViewResult()
        {
            // Arrange
            var controller = new HomeAdminController();

            // Act
            var result = controller.Index("username", "userrole") as ViewResult;

            // Assert
            Assert.NotNull(result);
        }


        [Fact]
        public void Edit_Get_WithInvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            var controller = new HomeAdminController();
            var userId = -1; // Đặt id của người dùng không tồn tại

            // Act
            var result = controller.Edit(userId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Delete_Get_RedirectsToIndexAfterDelete()
        {
            // Arrange
            var controller = new HomeAdminController();
            var userId = 1; // Đặt id của người dùng tồn tại

            // Act
            var result = controller.Delete(userId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void NewStudent_Get_ReturnsViewResult()
        {
            // Arrange
            var controller = new HomeAdminController();

            // Act
            var result = controller.NewStudent() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void NewStudent_Post_RedirectsToIndexAfterSuccessfulRegistration()
        {
            // Arrange
            var controller = new HomeAdminController();
            var newUser = new User
            {
                // Cung cấp dữ liệu cho người dùng mới
                Name = "New Student",
                Email = "newstudent@example.com",
                Password = "password",
                Role = "Student"
                // Các thông tin khác của người dùng mới có thể thêm ở đây
            };

            // Act
            var result = controller.NewStudent(newUser) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }
        [Fact]
        public void Delete_ReturnsViewResult_WithUserList()
        {
            // Arrange
            var controller = new HomeAdminController();

            // Act
            var result = controller.Index("TestUser", "Admin");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<User>>(viewResult.ViewData.Model);
            Assert.NotNull(model);
        }

        [Fact]
        public void Edit_ReturnsNotFound_WhenUserNotFound()
        {
            // Arrange
            var controller = new HomeAdminController();
            var invalidId = 999; // Assuming ID 999 does not exist

            // Act
            var result = controller.Edit(invalidId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void NewStudent_ReturnsRedirectToActionResult_WhenModelStateIsValid()
        {
            // Arrange
            var controller = new HomeAdminController();
            var newUser = new User { Name = "Test User", Role = "Student" };

            // Act
            var result = controller.NewStudent(newUser);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
        
    }
}

