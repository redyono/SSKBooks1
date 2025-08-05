using Microsoft.EntityFrameworkCore;
using SSKBooks.Data;
using SSKBooks.Models;
using SSKBooks.Services;
using SSKBooks.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SSKBooks1.Tests.Services
{
    public class OrderServiceTests_V2
    {
        private SSKBooksDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<SSKBooksDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new SSKBooksDbContext(options);
        }

        [Fact]
        public async Task CreateOrderAsync_WithValidBooks_ReturnsSuccess()
        {
            var context = GetDbContext();
            context.Books.Add(new Book { Id = 1, Title = "Test Book" });
            await context.SaveChangesAsync();

            var service = new OrderService(context);
            var model = new OrderCreateViewModel
            {
                Books = new List<OrderCreateViewModel.BookSelection>
                {
                    new OrderCreateViewModel.BookSelection
                    {
                        BookId = 1,
                        IsSelected = true,
                        Quantity = 2
                    }
                }
            };

            var result = await service.CreateOrderAsync(model, "user1");

            Assert.True(result.Success);
            Assert.Null(result.ErrorMessage);
        }

        [Fact]
        public async Task CreateOrderAsync_WithNoSelection_ReturnsError()
        {
            var context = GetDbContext();
            var service = new OrderService(context);
            var model = new OrderCreateViewModel
            {
                Books = new List<OrderCreateViewModel.BookSelection>()
            };

            var result = await service.CreateOrderAsync(model, "user1");

            Assert.False(result.Success);
            Assert.Equal("Please select at least one book.", result.ErrorMessage);
        }

        [Fact]
        public async Task GetOrderDetailsAsync_ReturnsCorrectOrder()
        {
            var context = GetDbContext();
            var order = new Order { Id = 10, UserId = "u1", Items = new List<OrderItem>() };
            context.Orders.Add(order);
            await context.SaveChangesAsync();

            var service = new OrderService(context);
            var result = await service.GetOrderDetailsAsync(10);

            Assert.NotNull(result);
            Assert.Equal(10, result.Id);
        }

        [Fact]
        public async Task DeleteOrderAsync_RemovesOrder()
        {
            var context = GetDbContext();
            var order = new Order { Id = 7, UserId = "u2" };
            context.Orders.Add(order);
            await context.SaveChangesAsync();

            var service = new OrderService(context);

            await service.DeleteOrderAsync(7);
            var result = await context.Orders.FindAsync(7);

            Assert.Null(result);
        }
    }
}
