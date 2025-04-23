using Microsoft.IdentityModel.Tokens;
using Movie_Collection.Authentication.DTO;
using Movie_Collection.Authentication.Model;
using Movie_Collection.Authentication.Repository;
using Movie_Collection.Movies.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Movie_Collection.Authentication.Service
{
    public class UserService : IUserService
    {
        private readonly IConfiguration? configuration;
        private readonly IUserRepository _userRepository;

        public UserService() { }    //I have made these 2 constructors because of tests

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public UserService(IConfiguration _configuration, IUserRepository userRepository)
        {
            configuration = _configuration;
            _userRepository = userRepository;
        }
    
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        public string CreateToken(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
            };

            if (user.UserRole == UserRole.Admin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "Regular"));
            }

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computeHash.SequenceEqual(passwordHash);
        }

        public User Create(User entity)
        {
            return _userRepository.Create(entity);
        }

        public void Delete(Guid id)
        {
             _userRepository.Delete(id);
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public IEnumerable<UserDTO> GetAllDto()
        {
            return _userRepository.GetAllDto();
        }

        public User GetById(Guid id)
        {
            return _userRepository.GetById(id);
        }

        public User GetByUsername(string user)
        {
            return _userRepository.GetByUsername(user);
        }

        public User Update(User entity)
        {
            return _userRepository.Update(entity);
        }

        public bool IsUsernameUnique(string username)
        {
            try
            {
                return _userRepository.GetByUsername(username) == null;
            }
            catch (NotFoundException)
            {
                return true;
            }
        }

        public bool IsEmailUnique(string email)
        {
            try
            {
                return _userRepository.GetByEmail(email) == null;
            }
            catch (NotFoundException)
            {
                return true;
            }
        }


        public User AddMovieToList(string username, Guid movieId)
        {
            return _userRepository.AddMovieToUser(username, movieId);
        }

        public User RemoveMovieFromUser(string username, Guid movieId)
        {
            return _userRepository.RemoveMovieFromUser(username, movieId);
        }

        public IEnumerable<UserDTO> GetAllExceptLogged(string username)
        {
            return _userRepository.GetAllExceptLogged(username);
        }
    }
}
