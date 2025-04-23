using Movie_Collection.Authentication.DTO;
using Movie_Collection.Authentication.Model;
using Movie_Collection.Generic;

namespace Movie_Collection.Authentication.Repository
{
    public interface IUserRepository : ICRUDRepository<User>
    {
        IEnumerable<UserDTO> GetAllDto();
        User GetByUsername(string username);
        User GetByEmail(string email);
        User AddMovieToUser(string username, Guid movieId);
        User RemoveMovieFromUser(string username, Guid movieId);
        IEnumerable<UserDTO> GetAllExceptLogged(string username);

    }
}
