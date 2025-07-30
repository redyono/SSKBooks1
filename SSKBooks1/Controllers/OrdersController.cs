using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSKBooks.Data;
using SSKBooks.Models;
using SSKBooks.ViewModels;

namespace SSKBooks1.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly SSKBooksDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OrdersController(SSKBooksDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (User.IsInRole("Admin"))
            {
                var allOrders = _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Book);

                return View(await allOrders.ToListAsync());
            }
            else
            {
                var userOrders = _context.Orders
                    .Where(o => o.UserId == user.Id)
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Book);

                return View(await userOrders.ToListAsync());
            }
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .ThenInclude(i => i.Book)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // ✅ REPLACED: GET: Orders/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var books = await _context.Books.ToListAsync();

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

        // ✅ REPLACED: POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(OrderCreateViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            var selectedItems = model.Books
                .Where(b => b.IsSelected && b.Quantity > 0)
                .Select(b => new OrderItem
                {
                    BookId = b.BookId,
                    Quantity = b.Quantity
                }).ToList();

            if (!selectedItems.Any())
            {
                ModelState.AddModelError("", "Please select at least one book.");
                return View(model);
            }

            var order = new Order
            {
                UserId = user.Id,
                OrderDate = DateTime.Now,
                Items = selectedItems
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var order = await _context.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
