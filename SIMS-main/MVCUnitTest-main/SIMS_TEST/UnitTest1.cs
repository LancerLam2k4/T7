using Microsoft.AspNetCore.Mvc;
using SIMS_Demo.Controllers;
using SIMS_Demo.Models;
namespace SIMS_TEST;

public class UnitTest1
{
    [Fact]
    public void Login_Get_ReturnsViewResult()
    {
        // Arrange
        var controller = new AuthenticationController();

        // Act
        var result = controller.Login() as ViewResult;

        // Assert
        Assert.NotNull(result);
    }
}
