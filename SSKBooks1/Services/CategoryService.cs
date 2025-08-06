using Microsoft.EntityFrameworkCore;
using SSKBooks.Data;
using SSKBooks.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSKBooks.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly SSKBooksDbContext _context;

        public CategoryService(SSKBooksDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task CreateAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Category category)
        {
            var exists = await _context.Categories.AnyAsync(c => c.Id == category.Id);
            if (!exists) return false;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
