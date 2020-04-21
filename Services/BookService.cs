using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace Services
{
    public class BookService : IBookService
    {
        private readonly ModelContext dbContext;

        public BookService(ModelContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<BookTable> GetBooks() => this.dbContext.Books.ToArray();

        public BookTable GetBookById(int id) => this.dbContext.Books.FirstOrDefault(b => b.Bookid == id);

        public int Add(BookVM book)
        {
            dbContext.Books.FromSql("");
            this.dbContext.Books.Add(book);
            dbContext.SaveChanges();
            return book.Bookid;
        }

        public void Delete(int id)
        {            
            var currentBook = this.dbContext.Books.FirstOrDefault(b => b.Bookid == id);

            if (currentBook == null)
            {
                throw new ArgumentNullException($"Book with ID {id} doesn't exist!");
            }

            this.dbContext.Books.Remove(currentBook);
            dbContext.SaveChanges();
        }

        public void Update(int id, BookTable book)
        {
            var currentBook = this.dbContext.Books.FirstOrDefault(b => b.Bookid == id);
            if (currentBook == null)
            {
                throw new ArgumentNullException($"Book with ID {id} doesn't exist!");
            }

            currentBook.BookTitle = book.BookTitle;
            currentBook.Author = book.Author;
            currentBook.PublishDate = book.PublishDate;
            currentBook.Pages = book.Pages;
            dbContext.SaveChanges();
        }
    }
}
