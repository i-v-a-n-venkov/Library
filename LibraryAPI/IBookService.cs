using LibraryAPI.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface IBookService
    {
        Task<IEnumerable<BookVM>> GetBooks();
        Task<BookVM> GetBookById(int id);
        Task<BookVM> Add(BookVM book);
        Task Delete(int id);
        Task Update(int id, BookVM book);
    }
}