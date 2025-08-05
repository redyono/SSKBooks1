using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SSKBooks.Models;
using SSKBooks.Services;
using SSKBooks.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SSKBooks1.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<IdentityUser> _userManager;

        public OrdersController(IOrderService orderService, UserManager<IdentityUser> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var orders = await _orderService.GetOrdersAsync(user, User.IsInRole("Admin"));
            return View(orders);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var order = await _orderService.GetOrderDetailsAsync(id.Value);
            if (order == null) return NotFound();

            return View(order);
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            var books = await _orderService.GetAvailableBooksAsync();
            var viewModel = new OrderCreateViewModel
            {
                Books = books.Select(b => new OrderCreateViewModel.BookSelection
                {
                    BookId = b.Id,
                    Title = b.Title,
                    Quantity = 1
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(OrderCreateViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _orderService.CreateOrderAsync(model, user.Id);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.ErrorMessage ?? "Could not create order.");
                model.Books = await _orderService.GetBookSelectionsAsync();
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var order = await _orderService.GetOrderForDeleteAsync(id.Value);
            if (order == null) return NotFound();

            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
