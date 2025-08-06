using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SSKBooks1.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SSKBooks1.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
        private readonly Mock<RoleManager<IdentityRole>> _mockRoleManager;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            var userStore = new Mock<IUserStore<IdentityUser>>();
            _mockUserManager = new Mock<UserManager<IdentityUser>>(userStore.Object, null, null, null, null, null, null, null, null);

            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            _mockRoleManager = new Mock<RoleManager<IdentityRole>>(roleStore.Object, null, null, null, null);

            _controller = new UsersController(_mockUserManager.Object, _mockRoleManager.Object);
        }

        [Fact]
        public async Task Index_ReturnsAllUsers()
        {
            var users = new List<IdentityUser>
            {
                new IdentityUser { UserName = "user1" },
                new IdentityUser { UserName = "user2" }
            };

            _mockUserManager.Setup(u => u.Users).Returns(users.AsQueryable());

            var result = _controller.Index();

            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<IdentityUser>>(view.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task Details_NullId_ReturnsNotFound()
        {
            var result = await _controller.Details(null);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_InvalidUser_ReturnsNotFound()
        {
            _mockUserManager.Setup(u => u.FindByIdAsync("bad-id")).ReturnsAsync((IdentityUser)null);

            var result = await _controller.Details("bad-id");

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ValidUser_ReturnsView()
        {
            var user = new IdentityUser { Id = "1", UserName = "user1" };
            _mockUserManager.Setup(u => u.FindByIdAsync("1")).ReturnsAsync(user);

            var result = await _controller.Details("1");

            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<IdentityUser>(view.Model);
            Assert.Equal("user1", model.UserName);
        }

        [Fact]
        public async Task DeleteConfirmed_ValidUser_DeletesAndRedirects()
        {
            var user = new IdentityUser { Id = "1", UserName = "user1" };
            _mockUserManager.Setup(u => u.FindByIdAsync("1")).ReturnsAsync(user);
            _mockUserManager.Setup(u => u.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

            var result = await _controller.DeleteConfirmed("1");

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async Task DeleteConfirmed_UserNotFound_ReturnsNotFound()
        {
            _mockUserManager.Setup(u => u.FindByIdAsync("x")).ReturnsAsync((IdentityUser)null);

            var result = await _controller.DeleteConfirmed("x");

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
