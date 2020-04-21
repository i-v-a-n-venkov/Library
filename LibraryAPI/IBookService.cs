using Data.Models;
using LibraryAPI.ViewModels;
using System.Collections.Generic;

namespace Services
{
    public interface IBookService
    {
        IEnumerable<BookVM> GetBooks();
        BookVM GetBookById(int id);
        BookVM Add(BookVM book);
        void Delete(int id);
        void Update(int id, BookVM book);
    }
}