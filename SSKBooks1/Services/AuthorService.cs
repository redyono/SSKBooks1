using Microsoft.EntityFrameworkCore;
using SSKBooks.Data;
using SSKBooks.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSKBooks.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly SSKBooksDbContext _context;

        public AuthorService(SSKBooksDbContext context)
        {
            _context = context;
        }

        public async Task<List<Author>> GetAllAsync()
        {
            return await _context.Authors
                .OrderBy(a => a.Name)
                .ToListAsync();
        }

        public async Task<List<Author>> SearchByNameAsync(string name)
        {
            return await _context.Authors
                .Where(a => a.Name.ToLower().Contains(name.ToLower()))
                .OrderBy(a => a.Name)
                .ToListAsync();
        }

        public async Task<Author> GetByIdAsync(int id)
        {
            return await _context.Authors.FindAsync(id);
        }

        public async Task CreateAsync(Author author)
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Author author)
        {
            var exists = await _context.Authors.AnyAsync(a => a.Id == author.Id);
            if (!exists) return false;

            _context.Authors.Update(author);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return false;

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
