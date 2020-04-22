using AutoMapper;
using Data.Models;
using LibraryAPI.ViewModels;

namespace LibraryAPI.AutoMapperProfile
{
    public class BookProfie : Profile
    {
        public BookProfie()
        {
            CreateMap<BookTable, BookVM>().ReverseMap();
        }
    }
}
