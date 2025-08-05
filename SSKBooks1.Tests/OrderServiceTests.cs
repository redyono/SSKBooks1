using Microsoft.EntityFrameworkCore;
using Moq;
using SSKBooks.Data;
using SSKBooks.Models;
using SSKBooks.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class OrderServiceTests
{
    private SSKBooksDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<SSKBooksDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDB")
            .Options;

        return new SSKBooksDbContext(options);
    }

    [Fact]
    public async Task GetAvailableBooksAsync_ReturnsBooks()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        context.Books.AddRange(
            new Book { Id = 1, Title = "Book A" },
            new Book { Id = 2, Title = "Book B" }
        );
        await context.SaveChangesAsync();

        var service = new OrderService(context);

        // Act
        var books = await service.GetAvailableBooksAsync();

        // Assert
        Assert.NotNull(books);
        Assert.Equal(2, books.Count);
    }
}
