using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SSKBooks.Models;
using SSKBooks.Services;
using SSKBooks1.Controllers;
using Xunit;

namespace SSKBooks.Tests.Controllers
{
    public class AuthorControllerTests
    {
        private readonly Mock<IAuthorService> _mockService;
        private readonly AuthorsController _controller;

        public AuthorControllerTests()
        {
            _mockService = new Mock<IAuthorService>();
            _controller = new AuthorsController(_mockService.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfAuthors()
        {
            // Arrange
            var authors = new List<Author>
            {
                new Author { Id = 1, Name = "Author One" },
                new Author { Id = 2, Name = "Author Two" }
            };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(authors);

            // Act
            var result = await _controller.Index(null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Author>>(viewResult.Model);
            Assert.Equal(2, ((List<Author>)model).Count);
        }

        [Fact]
        public async Task Index_WithSearch_ReturnsFilteredAuthors()
        {
            // Arrange
            var searchTerm = "One";
            var authors = new List<Author>
            {
                new Author { Id = 1, Name = "Author One" }
            };
            _mockService.Setup(s => s.SearchByNameAsync(searchTerm)).ReturnsAsync(authors);

            // Act
            var result = await _controller.Index(searchTerm);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Author>>(viewResult.Model);
            Assert.Single(model);
        }

        [Fact]
        public async Task Details_WithValidId_ReturnsAuthor()
        {
            // Arrange
            var author = new Author { Id = 1, Name = "Author One" };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(author);

            // Act
            var result = await _controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Author>(viewResult.Model);
            Assert.Equal("Author One", model.Name);
        }

        [Fact]
        public async Task Details_WithNullId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((Author)null);

            // Act
            var result = await _controller.Details(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_Post_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var author = new Author { Id = 1, Name = "New Author" };

            // Act
            var result = await _controller.Create(author);

            // Assert
            _mockService.Verify(s => s.CreateAsync(author), Times.Once);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async Task Edit_Post_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var author = new Author { Id = 1, Name = "Updated Author" };
            _mockService.Setup(s => s.UpdateAsync(author)).ReturnsAsync(true);

            // Act
            var result = await _controller.Edit(1, author);

            // Assert
            _mockService.Verify(s => s.UpdateAsync(author), Times.Once);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async Task Edit_Post_InvalidId_ReturnsNotFound()
        {
            var author = new Author { Id = 2, Name = "Mismatch" };

            var result = await _controller.Edit(1, author);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteConfirmed_ValidId_RedirectsToIndex()
        {
            _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _controller.DeleteConfirmed(1);

            _mockService.Verify(s => s.DeleteAsync(1), Times.Once);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async Task DeleteConfirmed_InvalidId_ReturnsNotFound()
        {
            _mockService.Setup(s => s.DeleteAsync(99)).ReturnsAsync(false);

            var result = await _controller.DeleteConfirmed(99);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
