using Microsoft.AspNetCore.Identity;
using SSKBooks.Models;
using SSKBooks.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SSKBooks.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrdersAsync(IdentityUser user, bool isAdmin);
        Task<Order?> GetOrderDetailsAsync(int orderId);
        Task<List<Book>> GetAvailableBooksAsync();
        Task<List<OrderCreateViewModel.BookSelection>> GetBookSelectionsAsync();
        Task<(bool Success, string? ErrorMessage)> CreateOrderAsync(OrderCreateViewModel model, string userId);
        Task<Order?> GetOrderForDeleteAsync(int id);
        Task DeleteOrderAsync(int id);
    }
}
