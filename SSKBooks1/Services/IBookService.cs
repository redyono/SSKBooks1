using SSKBooks.Models;
using SSKBooks1.Models;

namespace SSKBooks1.Services
{
    public interface IBookService
    {
        Task<List<Book>> GetAllAsync();
        Task<Book?> GetByIdAsync(int id);
        Task<Book> CreateAsync(Book book);
    }
}
