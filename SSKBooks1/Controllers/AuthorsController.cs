using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSKBooks.Models;
using SSKBooks.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSKBooks1.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        // 🔓 Public
        public async Task<IActionResult> Index(string search)
        {
            var authors = string.IsNullOrWhiteSpace(search)
                ? await _authorService.GetAllAsync()
                : await _authorService.SearchByNameAsync(search);

            ViewBag.Search = search;
            return View(authors);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var author = await _authorService.GetByIdAsync(id.Value);
            if (author == null) return NotFound();

            return View(author);
        }

        // 🔒 Admin Only
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name")] Author author)
        {
            if (ModelState.IsValid)
            {
                await _authorService.CreateAsync(author);
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var author = await _authorService.GetByIdAsync(id.Value);
            if (author == null) return NotFound();

            return View(author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Author author)
        {
            if (id != author.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var updated = await _authorService.UpdateAsync(author);
                if (!updated) return NotFound();

                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var author = await _authorService.GetByIdAsync(id.Value);
            if (author == null) return NotFound();

            return View(author);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _authorService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}
