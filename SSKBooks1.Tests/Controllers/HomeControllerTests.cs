using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SSKBooks.Models;
using SSKBooks1.Controllers;
using SSKBooks1.Models;
using System.Diagnostics;
using Xunit;

namespace SSKBooks1.Tests.Controllers
{
    public class HomeControllerTests
    {
        private readonly Mock<ILogger<HomeController>> _mockLogger;
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            _mockLogger = new Mock<ILogger<HomeController>>();
            _controller = new HomeController(_mockLogger.Object);
        }

        [Fact]
        public void Index_ReturnsView()
        {
            var result = _controller.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Privacy_ReturnsView()
        {
            var result = _controller.Privacy();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void AccessDenied_ReturnsView()
        {
            var result = _controller.AccessDenied();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Error_ReturnsErrorViewModel()
        {
            // Arrange
            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            _controller.ControllerContext = controllerContext;

            // Act
            var result = _controller.Error();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ErrorViewModel>(viewResult.Model);
            Assert.False(string.IsNullOrEmpty(model.RequestId));
        }
    }
}
