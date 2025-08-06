using SSKBooks.Models;

public interface IBookService
{
    Task<List<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(int id);
    Task<Book> CreateAsync(Book book);
    Task<Book?> EditAsync(Book book);
    Task DeleteAsync(int id);
    bool BookExists(int id);
}
