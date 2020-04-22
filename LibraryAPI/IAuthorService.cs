using Data.Models;
using LibraryAPI.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryAPI
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorVM>> GetAuthors();
        Task<AuthorVM> GetAuthorById(int id);
        Task<AuthorVM> Add(AuthorVM book);
        Task Delete(int id);
        Task Update(int id, AuthorVM book);
    }
}
