using Microsoft.EntityFrameworkCore;
using SSKBooks.Data;
using SSKBooks.Models;
using SSKBooks.Data;
using SSKBooks1.Models;

namespace SSKBooks1.Services
{
    public class BookService : IBookService
    {
        private readonly SSKBooksDbContext _context;

        public BookService(SSKBooksDbContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetAllAsync()
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book> CreateAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }
    }
}
