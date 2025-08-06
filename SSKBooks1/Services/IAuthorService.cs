using SSKBooks.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSKBooks.Services
{
    public interface IAuthorService
    {
        Task<List<Author>> GetAllAsync();
        Task<List<Author>> SearchByNameAsync(string name);
        Task<Author> GetByIdAsync(int id);
        Task CreateAsync(Author author);
        Task<bool> UpdateAsync(Author author);
        Task<bool> DeleteAsync(int id);
    }
}
