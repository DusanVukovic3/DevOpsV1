using Movie_Collection.Authentication.DTO;
using Movie_Collection.Authentication.Model;
using Movie_Collection.Generic;

namespace Movie_Collection.Authentication.Service
{
    public interface IUserService : ICRUDRepository<User>
    {
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        string CreateToken(User user);
        User GetByUsername(string user);
        bool IsUsernameUnique(string username);
        bool IsEmailUnique(string email);
        User AddMovieToList(string username, Guid movieId);
        User RemoveMovieFromUser(string username, Guid movieId);
        IEnumerable<UserDTO> GetAllDto();
        IEnumerable<UserDTO> GetAllExceptLogged(string username);

    }
}
