using Data.Models;
using System.Collections.Generic;

namespace Services
{
    public interface IBookService
    {
        IEnumerable<BookTable> GetBooks();
        BookTable GetBookById(int id);
        int Add(BookTable book);
        void Delete(int id);
        void Update(int id, BookTable book);
    }
}