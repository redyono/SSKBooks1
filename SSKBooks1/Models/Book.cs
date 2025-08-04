using SSKBooks1.Models;
using System.ComponentModel.DataAnnotations;

namespace SSKBooks.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        [Display(Name = "Book Title")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 1000.00, ErrorMessage = "Price must be between $0.01 and $1000.")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name = "Author")]
        [Required(ErrorMessage = "Please select an author.")]
        public int AuthorId { get; set; }

        public Author? Author { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Please select a category.")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}
