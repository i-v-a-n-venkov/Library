using AutoMapper;
using Data.Models;
using LibraryAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI
{
    public class AuthorService : IAuthorService
    {
        private readonly ModelContext dbContext;
        private readonly IBookService bookService;
        private readonly IMapper autoMapper;

        public AuthorService(ModelContext dbContext, IBookService bookService, IMapper autoMapper)
        {
            this.dbContext = dbContext;
            this.bookService = bookService;
            this.autoMapper = autoMapper;
        }

        public async Task<IEnumerable<AuthorVM>> GetAuthors()
        {
            //var authors = this.dbContext.Authors
            //    .Include(b => b.Book)
            //    .Select(a => new AuthorVM
            //    {
            //        Id = a.AuthorId,
            //        AuthorName = a.AuthorName,
            //        BookTableId = a.BookTableId
            //    });

            var allAuthors = await dbContext.Authors.Include(x => x.Book).ToListAsync();
            var result = this.autoMapper.Map<List<AuthorVM>>(allAuthors);
            return result;

            //return authors;
        }

        public async Task<AuthorVM> GetAuthorById(int id)
        {
            var currentAuthor = await this.dbContext.Authors.FirstOrDefaultAsync(a => a.AuthorId == id);
            var currentBook = await this.dbContext.Books.FindAsync(currentAuthor.BookTableId);

            if (currentBook == null)
            {
                throw new ArgumentException("Author doesn't have any book!");
            }

            //var resultAuthor = new AuthorVM
            //{
            //    Id = currentAuthor.AuthorId,
            //    AuthorName = currentAuthor.AuthorName,
            //    BookTableId = currentAuthor.BookTableId
            //};

            var resultAuthor = this.autoMapper.Map<AuthorVM>(currentAuthor);
            return resultAuthor;
        }

        public async Task<AuthorVM> Add(AuthorVM author)
        {
            var authorExists = await dbContext.Authors.Select(x => x.AuthorName).ContainsAsync(author.AuthorName);

            if (authorExists)
            {
                throw new ArgumentException($"Author with name {author.AuthorName} already exists in db!");
            }

            //var nextVal = dbContext.AuthorNextVals.FromSql<AuthorNextValQuery>("select authors_next_id.NEXTVAL from dual").ToList();
            //var authorId = Convert.ToInt32(nextVal[0].NextVal);

            var authorId = await GetNextValue();

            var currentBook = await this.dbContext.Books.FindAsync(author.BookTableId);

            if (currentBook == null)
            {
                throw new ArgumentException($"The book with id {author.BookTableId} doesn't exist!");
            }

            //var newAuthor = new AuthorTable
            //{
            //    AuthorId = authorId,
            //    AuthorName = author.AuthorName,
            //    BookTableId = author.BookTableId
            //};

            var newAuthor = this.autoMapper.Map<AuthorTable>(author);
            newAuthor.AuthorId = authorId;
            currentBook.AuthorTables = new List<AuthorTable> { newAuthor };
            await this.dbContext.Authors.AddAsync(newAuthor);

            await dbContext.SaveChangesAsync();
            //var resultAsVM = new AuthorVM
            //{
            //    Id = result.AuthorId,
            //    AuthorName = result.AuthorName,
            //    BookTableId = result.BookTableId
            //};

            var resultAsVM = this.autoMapper.Map<AuthorVM>(newAuthor);

            return resultAsVM;
        }

        public async Task Delete(int id)
        {
            var currentAuthor = await this.dbContext.Authors.FirstOrDefaultAsync(a => a.AuthorId == id);

            if (currentAuthor == null)
            {
                throw new ArgumentNullException($"Author with ID {id} doesn't exist!");
            }

            this.dbContext.Authors.Remove(currentAuthor);
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(int id, AuthorVM author)
        {
            var currentAuthor = await this.dbContext.Authors.FirstOrDefaultAsync(a => a.AuthorId == id);
            if (currentAuthor == null)
            {
                throw new ArgumentNullException($"Author with ID {id} doesn't exist!");
            }

            currentAuthor.AuthorName = author.AuthorName;
            currentAuthor.BookTableId = author.BookTableId;

            await dbContext.SaveChangesAsync();
        }

        public async Task<int> GetNextValue()
        {
            using (var command = dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = $"select authors_next_id.NEXTVAL from dual";
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

