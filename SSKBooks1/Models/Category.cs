using System.ComponentModel.DataAnnotations;

namespace SSKBooks.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
