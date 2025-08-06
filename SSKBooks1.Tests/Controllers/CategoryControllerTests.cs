using Microsoft.AspNetCore.Mvc;
using Moq;
using SSKBooks.Models;
using SSKBooks.Services;
using SSKBooks1.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SSKBooks1.Tests.Controllers
{
    public class CategoryControllerTests
    {
        private readonly Mock<ICategoryService> _mockService;
        private readonly CategoriesController _controller;

        public CategoryControllerTests()
        {
            _mockService = new Mock<ICategoryService>();
            _controller = new CategoriesController(_mockService.Object);
        }

        [Fact]
        public async Task Index_ReturnsAllCategories()
        {
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Fiction" },
                new Category { Id = 2, Name = "Science" }
            };

            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(categories);

            var result = await _controller.Index();

            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Category>>(view.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task Details_NullId_ReturnsNotFound()
        {
            var result = await _controller.Details(null);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_InvalidId_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((Category)null);

            var result = await _controller.Details(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ValidId_ReturnsCategory()
        {
            var category = new Category { Id = 1, Name = "Fiction" };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(category);

            var result = await _controller.Details(1);

            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Category>(view.Model);
            Assert.Equal("Fiction", model.Name);
        }

        [Fact]
        public async Task Create_ValidModel_RedirectsToIndex()
        {
            var category = new Category { Name = "History" };

            var result = await _controller.Create(category);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async Task Create_InvalidModel_ReturnsView()
        {
            _controller.ModelState.AddModelError("Name", "Required");

            var result = await _controller.Create(new Category());

            var view = Assert.IsType<ViewResult>(result);
            Assert.IsType<Category>(view.Model);
        }

        [Fact]
        public async Task Edit_Get_InvalidId_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetByIdAsync(5)).ReturnsAsync((Category)null);

            var result = await _controller.Edit(5);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Post_ValidModel_RedirectsToIndex()
        {
            var category = new Category { Id = 1, Name = "Updated" };
            _mockService.Setup(s => s.UpdateAsync(category)).ReturnsAsync(true);

            var result = await _controller.Edit(1, category);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async Task Edit_Post_ModelInvalid_ReturnsView()
        {
            _controller.ModelState.AddModelError("Name", "Required");
            var category = new Category { Id = 1 };

            var result = await _controller.Edit(1, category);

            var view = Assert.IsType<ViewResult>(result);
            Assert.Same(category, view.Model);
        }

        [Fact]
        public async Task Edit_Post_IdMismatch_ReturnsNotFound()
        {
            var category = new Category { Id = 2 };

            var result = await _controller.Edit(1, category);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_Get_InvalidId_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetByIdAsync(404)).ReturnsAsync((Category)null);

            var result = await _controller.Delete(404);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteConfirmed_ValidId_Redirects()
        {
            _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _controller.DeleteConfirmed(1);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async Task DeleteConfirmed_InvalidId_ReturnsNotFound()
        {
            _mockService.Setup(s => s.DeleteAsync(2)).ReturnsAsync(false);

            var result = await _controller.DeleteConfirmed(2);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
