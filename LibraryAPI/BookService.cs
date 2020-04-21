using Data.Models;
using LibraryAPI;
using LibraryAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Services
{
    public class BookService : IBookService
    {
        private readonly ModelContext dbContext;

        public BookService(ModelContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<BookVM> GetBooks()
        {
            return dbContext.Books.Select(b => new BookVM
            {
                BookId = b.Bookid,
                BookTitle = b.BookTitle,
                Pages = b.Pages,
                PublishDate = b.PublishDate,
                AuthorTables = b.AuthorTables
            }).ToList();


            //var list = from p in dbContext.Books
            //           select new BookVM
            //           {
            //               Author = p.Author,
            //               BookId = p.Bookid,
            //               BookTitle = p.BookTitle,
            //               Pages = p.Pages,
            //               PublishDate = p.PublishDate
            //           };

            //return list.ToList();
        }

        public BookVM GetBookById(int id)
        {
            var currentBook = this.dbContext.Books.Include(x => x.AuthorTables).FirstOrDefault(b => b.Bookid == id);
            var resultVM = new BookVM
            {
                BookId = currentBook.Bookid,
                BookTitle = currentBook.BookTitle,
                PublishDate = currentBook.PublishDate,
                AuthorTables = currentBook.AuthorTables,
                Pages = currentBook.Pages
            };

            return resultVM;
        }

        public BookVM Add(BookVM book)
        {
            var bookExists = dbContext.Books.Select(b => b.BookTitle).Contains(book.BookTitle);

            if (bookExists)
            {
                throw new ArgumentException($"Book with name {book.BookTitle} already exists in db!");
            }

            var nextVal = dbContext.BookNextVals.FromSql<BookNextValQuery>("select books_next_id.NEXTVAL from dual").ToList();

            var bookId = Convert.ToInt32(nextVal[0].NextVal);

            var newBook = new BookTable
            {
                Bookid = bookId,
                BookTitle = book.BookTitle,
                Pages = book.Pages,
                PublishDate = book.PublishDate,
            };

            this.dbContext.Books.Add(newBook);
            dbContext.SaveChanges();

            var resultVM = new BookVM
            {
                BookId = newBook.Bookid,
                BookTitle = newBook.BookTitle,
                PublishDate = newBook.PublishDate,
                Pages = newBook.Pages
            };

            return resultVM;
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

        public void Update(int id, BookVM book)
        {
            var currentBook = this.dbContext.Books.FirstOrDefault(b => b.Bookid == id);
            if (currentBook == null)
            {
                throw new ArgumentNullException($"Book with ID {id} doesn't exist!");
            }

            var currentAuthor = dbContext.Authors.FirstOrDefault(x => x.AuthorId == book.AuthorTables.Select(x => x.AuthorId).FirstOrDefault());

            if (currentAuthor != null)
            {
                var test = book.AuthorTables.FirstOrDefault(id => id.AuthorId == currentAuthor.AuthorId);
                UpdateAuthor(currentAuthor.AuthorId, test);
            }

            currentBook.BookTitle = book.BookTitle;
            currentBook.PublishDate = book.PublishDate;
            currentBook.Pages = book.Pages;
            dbContext.SaveChanges();
        }
        private void UpdateAuthor(int authorId, AuthorTable author)
        {
            var currentAuthor = this.dbContext.Authors.FirstOrDefault(a => a.AuthorId == authorId);
            currentAuthor.AuthorName = author.AuthorName;
            currentAuthor.BookTableId = author.BookTableId;
            dbContext.SaveChanges();
        }
    }
}