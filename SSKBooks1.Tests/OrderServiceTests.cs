using Microsoft.EntityFrameworkCore;
using SSKBooks.Data;
using SSKBooks.Models;
using SSKBooks.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SSKBooks1.Tests.Services
{
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
            var context = GetInMemoryDbContext();
            context.Books.AddRange(
                new Book { Id = 1, Title = "Book A" },
                new Book { Id = 2, Title = "Book B" }
            );
            await context.SaveChangesAsync();

            var service = new OrderService(context);

            var books = await service.GetAvailableBooksAsync();

            Assert.NotNull(books);
            Assert.Equal(2, books.Count);
        }
    }
}
