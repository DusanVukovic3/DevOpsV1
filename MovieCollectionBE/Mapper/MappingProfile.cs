using AutoMapper;
using Movie_Collection.Authentication.DTO;
using Movie_Collection.Authentication.Model;
using Movie_Collection.Movies.DTO;
using Movie_Collection.Movies.Model;

namespace Movie_Collection.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MovieDTO, Movie>().ReverseMap(); // Added ReverseMap for MovieDTO
            CreateMap<RegisterUserDTO, User>();
            CreateMap<LoginUserDTO, User>();
            CreateMap<UserDTO, User>().ReverseMap();   // Added ReverseMap for UserDTO
        }
    }
}
