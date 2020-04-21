using Data.Models;
using LibraryAPI.ViewModels;
using System.Collections.Generic;

namespace LibraryAPI
{
    public interface IAuthorService
    {
        IEnumerable<AuthorVM> GetAuthors();
        AuthorVM GetAuthorById(int id);
        AuthorVM Add(AuthorVM book);
        void Delete(int id);
        void Update(int id, AuthorVM book);
    }
}
