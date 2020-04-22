using AutoMapper;
using Data.Models;
using LibraryAPI.ViewModels;

namespace LibraryAPI.AutoMapperProfile
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<AuthorTable, AuthorVM>().ReverseMap();
        }
    }
}
