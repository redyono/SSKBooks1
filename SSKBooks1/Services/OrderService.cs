using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SSKBooks.Data;
using SSKBooks.Models;
using SSKBooks.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSKBooks.Services
{
    public class OrderService : IOrderService
    {
        private readonly SSKBooksDbContext _context;

        public OrderService(SSKBooksDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(IdentityUser user, bool isAdmin)
        {
            var query = _context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .ThenInclude(i => i.Book);

            return isAdmin
                ? await query.ToListAsync()
                : await query.Where(o => o.UserId == user.Id).ToListAsync();
        }

        public async Task<Order?> GetOrderDetailsAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .ThenInclude(i => i.Book)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<List<Book>> GetAvailableBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<List<OrderCreateViewModel.BookSelection>> GetBookSelectionsAsync()
        {
            var books = await GetAvailableBooksAsync();
            return books.Select(b => new OrderCreateViewModel.BookSelection
            {
                BookId = b.Id,
                Title = b.Title,
                Quantity = 1
            }).ToList();
        }

        public async Task<(bool Success, string? ErrorMessage)> CreateOrderAsync(OrderCreateViewModel model, string userId)
        {
            var selectedItems = model.Books
                .Where(b => b.IsSelected && b.Quantity > 0)
                .Select(b => new OrderItem
                {
                    BookId = b.BookId,
                    Quantity = b.Quantity
                }).ToList();

            if (!selectedItems.Any())
                return (false, "Please select at least one book.");

            var order = new Order
            {
                UserId = userId,
                OrderDate = System.DateTime.Now,
                Items = selectedItems
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return (true, null);
        }

        public async Task<Order?> GetOrderForDeleteAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}
