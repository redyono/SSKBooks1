using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SSKBooks.Models;

namespace SSKBooks.Data
{
    public class SSKBooksDbContext : IdentityDbContext
    {
        public SSKBooksDbContext(DbContextOptions<SSKBooksDbContext> options)
            : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed Categories
            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Fiction" },
                new Category { Id = 2, Name = "Non-Fiction" }
            );

            // Seed Authors
            builder.Entity<Author>().HasData(
                new Author { Id = 1, Name = "Stephen King" },
                new Author { Id = 2, Name = "J.K. Rowling" }
            );

            // Seed Books
             builder.Entity<Book>().HasData(
                new Book
                {
                    Id = 1,
                    Title = "The Shining",
                    Price = 15.99m,
                    AuthorId = 1,
                    CategoryId = 1
                },
                new Book
                {
                    Id = 2,
                    Title = "Harry Potter and the Philosopher's Stone",
                    Price = 12.50m,
                    AuthorId = 2,
                    CategoryId = 1
                },
                new Book
                {
                    Id = 3,
                    Title = "Carrie",
                    Price = 13.49m,
                    AuthorId = 1,
                    CategoryId = 1
                },
                new Book
                {
                    Id = 4,
                    Title = "Harry Potter and the Chamber of Secrets",
                    Price = 14.20m,
                    AuthorId = 2,
                    CategoryId = 1
                },
                new Book
                {
                    Id = 5,
                    Title = "On Writing",
                    Price = 11.00m,
                    AuthorId = 1,
                    CategoryId = 2 // Non-Fiction
                },
                new Book
                {
                    Id = 6,
                    Title = "Fantastic Beasts and Where to Find Them",
                    Price = 10.75m,
                    AuthorId = 2,
                    CategoryId = 2
                },
                new Book
                {
                    Id = 7,
                    Title = "The Institute",
                    Price = 17.25m,
                    AuthorId = 1,
                    CategoryId = 1
                }
            );

        }

            
        }
    }

