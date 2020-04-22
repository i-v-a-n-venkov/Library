using AutoMapper;
using Data.Models;
using LibraryAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class BookService : IBookService
    {
        private readonly ModelContext dbContext;
        private readonly IMapper autoMapper;

        public BookService(ModelContext dbContext, IMapper autoMapper)
        {
            this.dbContext = dbContext;
            this.autoMapper = autoMapper;
        }

        public async Task<IEnumerable<BookVM>> GetBooks()
        {
            //return dbContext.Books.Select(b => new BookVM
            //{
            //    BookId = b.Bookid,
            //    BookTitle = b.BookTitle,
            //    Pages = b.Pages,
            //    PublishDate = b.PublishDate,
            //    AuthorTables = b.AuthorTables
            //}).ToList();

            var allBooks = await dbContext.Books.Include(x => x.AuthorTables).ToListAsync();
            var result = this.autoMapper.Map<List<BookVM>>(allBooks);
            return result;


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

        public async Task<BookVM> GetBookById(int id)
        {
            var currentBook = await this.dbContext.Books.Include(x => x.AuthorTables).FirstOrDefaultAsync(b => b.Bookid == id);
            //var resultVM = new BookVM
            //{
            //    BookId = currentBook.Bookid,
            //    BookTitle = currentBook.BookTitle,
            //    PublishDate = currentBook.PublishDate,
            //    AuthorTables = currentBook.AuthorTables,
            //    Pages = currentBook.Pages
            //};

            //var currentBook = this.dbContext.Books.FirstOrDefault(b => b.Bookid == id);
            var result = this.autoMapper.Map<BookVM>(currentBook);

            return result;
        }

        public async Task<BookVM> Add(BookVM book)
        {
            var bookExists = await dbContext.Books.Select(b => b.BookTitle).ContainsAsync(book.BookTitle);

            if (bookExists)
            {
                throw new ArgumentException($"Book with name {book.BookTitle} already exists in db!");
            }

            //var nextVal = dbContext.BookNextVals.FromSql<BookNextValQuery>("select books_next_id.NEXTVAL from dual").ToList();
            //var bookId = Convert.ToInt32(nextVal[0].NextVal);

            var bookId = await GetNextValue();

            //var newBook = new BookTable
            //{
            //    Bookid = bookId,
            //    BookTitle = book.BookTitle,
            //    Pages = book.Pages,
            //    PublishDate = book.PublishDate,
            //};

            var newBook = this.autoMapper.Map<BookTable>(book);
            newBook.Bookid = bookId;
            await this.dbContext.Books.AddAsync(newBook);
            await dbContext.SaveChangesAsync();

            //var resultVM = new BookVM
            //{
            //    BookId = newBook.Bookid,
            //    BookTitle = newBook.BookTitle,
            //    PublishDate = newBook.PublishDate,
            //    Pages = newBook.Pages
            //};

            var resultVM = this.autoMapper.Map<BookVM>(newBook);
            return resultVM;
        }

        public async Task Delete(int id)
        {
            var currentBook = await this.dbContext.Books.FirstOrDefaultAsync(b => b.Bookid == id);

            if (currentBook == null)
            {
                throw new ArgumentNullException($"Book with ID {id} doesn't exist!");
            }

            this.dbContext.Books.Remove(currentBook);
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(int id, BookVM book)
        {
            var currentBook = await this.dbContext.Books.FirstOrDefaultAsync(b => b.Bookid == id);
            if (currentBook == null)
            {
                throw new ArgumentNullException($"Book with ID {id} doesn't exist!");
            }

            var currentAuthor = await dbContext.Authors.FirstOrDefaultAsync(x => x.AuthorId == book.AuthorTables.Select(x => x.AuthorId).FirstOrDefault());

            if (currentAuthor != null)
            {
                var test = book.AuthorTables.FirstOrDefault(author => author.AuthorId == currentAuthor.AuthorId);
                await UpdateAuthor(currentAuthor.AuthorId, test);
            }

            currentBook.BookTitle = book.BookTitle;
            currentBook.PublishDate = book.PublishDate;
            currentBook.Pages = book.Pages;
            await dbContext.SaveChangesAsync();
        }
        private async Task UpdateAuthor(int authorId, AuthorTable author)
        {
            var currentAuthor = await this.dbContext.Authors.FirstOrDefaultAsync(a => a.AuthorId == authorId);
            currentAuthor.AuthorId = author.AuthorId;
            currentAuthor.AuthorName = author.AuthorName;
            currentAuthor.BookTableId = author.BookTableId;
            await dbContext.SaveChangesAsync();
        }

        public async Task<int> GetNextValue()
        {
            using (var command = dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = $"select books_next_id.NEXTVAL from dual";
               await dbContext.Database.OpenConnectionAsync();

                using (var reader = command.ExecuteReader())
                {
                    await reader.ReadAsync();
                    return reader.GetInt32(0);
                }
            }
        }
    }
}