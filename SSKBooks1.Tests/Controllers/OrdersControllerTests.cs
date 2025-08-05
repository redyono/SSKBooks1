using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SSKBooks.Models;
using SSKBooks.Services;
using SSKBooks1.Controllers;
using SSKBooks.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace SSKBooks1.Tests.Controllers
{
    public class OrdersControllerTests
    {
        private Mock<IOrderService> _orderServiceMock;
        private Mock<UserManager<IdentityUser>> _userManagerMock;

        public OrdersControllerTests()
        {
            _orderServiceMock = new Mock<IOrderService>();
            _userManagerMock = MockUserManager();
        }

        private static Mock<UserManager<IdentityUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            return new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        }

        [Fact]
        public async Task Index_AdminUser_ReturnsAllOrders()
        {
            // Arrange
            var user = new IdentityUser { Id = "admin-id", UserName = "admin@test.com" };
            var fakeOrders = new List<Order> { new Order { Id = 1 }, new Order { Id = 2 } };

            _userManagerMock.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(user);

            _orderServiceMock.Setup(s => s.GetOrdersAsync(user, true))
                .ReturnsAsync(fakeOrders);

            var controller = new OrdersController(_orderServiceMock.Object, _userManagerMock.Object);
            controller.ControllerContext = GetMockControllerContext(isAdmin: true);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Order>>(viewResult.Model);
            Assert.Equal(2, ((List<Order>)model).Count);
        }

        private ControllerContext GetMockControllerContext(bool isAdmin)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "admin@test.com"),
                new Claim(ClaimTypes.Role, isAdmin ? "Admin" : "User")
            };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            return new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };
        }
    }
}
