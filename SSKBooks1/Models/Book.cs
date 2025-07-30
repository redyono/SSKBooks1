using SSKBooks1.Models;
using System.ComponentModel.DataAnnotations;

namespace SSKBooks.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public decimal Price { get; set; }

        public int AuthorId { get; set; }
        public Author? Author { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
