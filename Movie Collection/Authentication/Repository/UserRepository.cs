using Microsoft.EntityFrameworkCore;
using Movie_Collection.Authentication.DTO;
using Movie_Collection.Authentication.Model;
using Movie_Collection.Movies.Exceptions;
using Movie_Collection.Movies.Repository;
using Movie_Collection.Settings;

namespace Movie_Collection.Authentication.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IMovieRepository _movieRepository;

        public UserRepository(AppDbContext context, IMovieRepository movieRepository)
        {
            _context = context;
            _movieRepository = movieRepository;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.Include(u => u.Movies).ToList(); 
        }

        public IEnumerable<UserDTO> GetAllDto()
        {
            return _context.Users.Select(u => new UserDTO   //To miss error 500 A possible object cycle was detected
            {
                UserId = u.UserId,
                Email = u.Email,
                Username = u.Username,
                Movies = u.Movies.ToList()  
            }).ToList();
        }

        public IEnumerable<UserDTO> GetAllExceptLogged(string username)
        {
            return _context.Users
                .Where(u => u.Username != username)     // Exclude the user with the given username
                .Select(u => new UserDTO
                {
                    UserId = u.UserId,
                    Email = u.Email,
                    Username = u.Username,
                    Movies = u.Movies.ToList()
                })
                .ToList();
        }


        public User GetById(Guid id)
        {
            var result = _context.Users.SingleOrDefault(p => p.UserId == id);
            return result ?? throw new NotFoundException("User with selected id not found!");
        }

        public User GetByUsername(string username)
        {
            var result = _context.Users
                .Include(u => u.Movies)  // Eager loading of the Movies collection, done like this because without it, user wasn't loading movies
                .SingleOrDefault(p => p.Username == username);

            return result ?? throw new NotFoundException($"User with username '{username}' not found");
        }

        public User GetByEmail(string email)
        {
            var result = _context.Users.SingleOrDefault(p => p.Email == email);
            return result ?? throw new NotFoundException("User with selected email not found!");
        }

        public User Create(User entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _context.Users.Add(entity);
            _context.SaveChanges();

            return entity;
        }

        public User Update(User entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var updatingUser = _context.Users.FirstOrDefault(p => p.UserId == entity.UserId) ?? throw new NotFoundException();

            updatingUser.Update(entity);
            _context.SaveChanges();
            return updatingUser;
        }

        public void Delete(Guid id)
        {
            var user = GetById(id);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public User AddMovieToUser(string username, Guid movieId)
        {
            var updatingUser = GetByUsername(username) ?? throw new NotFoundException();
            var movieToAdd = _movieRepository.GetById(movieId);

            var existingMovie = updatingUser.Movies.FirstOrDefault(m => m.MovieId == movieId);
            if (existingMovie != null)
            {
                throw new DuplicateMovieException("Movie already in user's collection");    //In frontend i already made a getAll that gets only the movies that are not in user's collection, so this is not that useful
            }

            if (movieToAdd != null)
            {
                updatingUser.AddMovie(movieToAdd);
                _context.SaveChanges();

                return updatingUser;
            }
            return updatingUser;
        }

        public User RemoveMovieFromUser(string username, Guid movieId)
        {
            var updatingUser = GetByUsername(username) ?? throw new NotFoundException();
            var movieToRemove = _movieRepository.GetById(movieId);

            var existingMovie = updatingUser.Movies.FirstOrDefault(m => m.MovieId == movieId) ?? throw new NotFoundException("Movie not found in user's collection");

            if (movieToRemove != null)
            {
                updatingUser.RemoveMovie(movieToRemove);
                _context.SaveChanges();

                return updatingUser;
            }
            return updatingUser;
        }




    }
}
