using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SSKBooks.Data;
using SSKBooks.Models;
using SSKBooks.Data;
using SSKBooks1.Models;
using SSKBooks1.Services;

namespace SSKBooks1.Tests
{
    public class BookServiceTests
    {
        private SSKBooksDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<SSKBooksDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new SSKBooksDbContext(options);

            context.Categories.Add(new Category { Id = 1, Name = "Fiction" });
            context.Authors.Add(new Author { Id = 1, Name = "Test Author" });
            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task CreateAsync_ShouldAddBook()
        {
            // Arrange
            var db = GetDbContext();
            var service = new BookService(db);

            var book = new Book
            {
                Title = "Test Book",
                Price = 12.99m,
                AuthorId = 1,
                CategoryId = 1
            };

            // Act
            var result = await service.CreateAsync(book);

            // Assert
            result.Id.Should().BeGreaterThan(0);
            db.Books.Count().Should().Be(1);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllBooks()
        {
            // Arrange
            var db = GetDbContext();
            var service = new BookService(db);

            db.Books.Add(new Book { Title = "Book 1", Price = 10, AuthorId = 1, CategoryId = 1 });
            db.Books.Add(new Book { Title = "Book 2", Price = 15, AuthorId = 1, CategoryId = 1 });
            db.SaveChanges();

            // Act
            var books = await service.GetAllAsync();

            // Assert
            books.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectBook()
        {
            // Arrange
            var db = GetDbContext();
            var service = new BookService(db);

            var book = new Book { Title = "Book 1", Price = 10, AuthorId = 1, CategoryId = 1 };
            db.Books.Add(book);
            db.SaveChanges();

            // Act
            var result = await service.GetByIdAsync(book.Id);

            // Assert
            result.Should().NotBeNull();
            result!.Title.Should().Be("Book 1");
        }
    }
}
