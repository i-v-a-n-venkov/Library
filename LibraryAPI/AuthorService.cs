using Data.Models;
using LibraryAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace LibraryAPI
{
    public class AuthorService : IAuthorService
    {
        private readonly ModelContext dbContext;
        private readonly IBookService bookService;

        public AuthorService(ModelContext dbContext, IBookService bookService)
        {
            this.dbContext = dbContext;
            this.bookService = bookService;
        }

        public IEnumerable<AuthorVM> GetAuthors()
        {
            var authors = this.dbContext.Authors
                .Include(b => b.Book)
                .Select(a => new AuthorVM
                {
                    Id = a.AuthorId,
                    AuthorName = a.AuthorName,
                    BookTableId = a.BookTableId
                });

            return authors;
        }

        public AuthorVM GetAuthorById(int id)
        {
            var currentAuthor = this.dbContext.Authors.FirstOrDefault(a => a.AuthorId == id);
            var currentBook = this.dbContext.Books.Find(currentAuthor.BookTableId);

            if (currentBook == null)
            {
                throw new ArgumentException("Author doesn't have any book!");
            }

            var resultAuthor = new AuthorVM
            {
                Id = currentAuthor.AuthorId,
                AuthorName = currentAuthor.AuthorName,
                BookTableId = currentAuthor.BookTableId
            };

            return resultAuthor;
        }

        public AuthorVM Add(AuthorVM author)
        {
            var authorExists = dbContext.Authors.Select(x => x.AuthorName).Contains(author.AuthorName);

            if (authorExists)
            {
                throw new ArgumentException($"Author with name {author.AuthorName} already exists in db!");
            }

            //var nextVal = dbContext.AuthorNextVals.FromSql<AuthorNextValQuery>("select authors_next_id.NEXTVAL from dual").ToList();
            //var authorId = Convert.ToInt32(nextVal[0].NextVal);

            var authorId = GetNextValue();

            var currentBook = this.dbContext.Books.Find(author.BookTableId);

            if (currentBook == null)
            {
                throw new ArgumentException($"The book with id {author.BookTableId} doesn't exist!");
            }

            var newAuthor = new AuthorTable
            {
                AuthorId = authorId,
                AuthorName = author.AuthorName,
                BookTableId = author.BookTableId
            };

            currentBook.AuthorTables = new List<AuthorTable> { newAuthor };

            var result = this.dbContext.Authors.Add(newAuthor).Entity;
            var resultAsVM = new AuthorVM
            {
                Id = result.AuthorId,
                AuthorName = result.AuthorName,
                BookTableId = result.BookTableId
            };

            dbContext.SaveChanges();
            return resultAsVM;
        }

        public void Delete(int id)
        {
            var currentAuthor = this.dbContext.Authors.FirstOrDefault(a => a.AuthorId == id);

            if (currentAuthor == null)
            {
                throw new ArgumentNullException($"Author with ID {id} doesn't exist!");
            }

            this.dbContext.Authors.Remove(currentAuthor);
            dbContext.SaveChanges();
        }

        public void Update(int id, AuthorVM author)
        {
            var currentAuthor = this.dbContext.Authors.FirstOrDefault(a => a.AuthorId == id);
            if (currentAuthor == null)
            {
                throw new ArgumentNullException($"Author with ID {id} doesn't exist!");
            }

            currentAuthor.AuthorName = author.AuthorName;
            currentAuthor.BookTableId = author.BookTableId;

            dbContext.SaveChanges();
        }

        public int GetNextValue()
        {
            using (var command = dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = $"select authors_next_id.NEXTVAL from dual";
                dbContext.Database.OpenConnection();

                using (var reader = command.ExecuteReader())
                {
                    reader.Read();
                    return reader.GetInt32(0);
                }
            }
        }
    }
}

