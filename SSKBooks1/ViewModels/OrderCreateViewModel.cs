using System.ComponentModel.DataAnnotations;

namespace SSKBooks.ViewModels
{
    public class OrderCreateViewModel
    {
        public List<BookSelection> Books { get; set; } = new();

        public class BookSelection
        {
            public int BookId { get; set; }
            public string Title { get; set; }
            public bool IsSelected { get; set; }
            [Range(1, 100)]
            public int Quantity { get; set; } = 1;
        }
    }
}
