using Movie_Collection.Movies.Exceptions;
using Movie_Collection.Movies.Model;
using Movie_Collection.Settings;

namespace Movie_Collection.Movies.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly AppDbContext _context;

        public MovieRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Movie> GetAll()
        {
            return _context.Movies.ToList();
        }

        public Movie GetById(Guid id)
        {
            var result = _context.Movies.SingleOrDefault(p => p.MovieId == id);
            return result ?? throw new NotFoundException("Movie with selected id not found!");
        }

        public Movie Create(Movie entity)
        {
            _context.Movies.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public Movie Update(Movie entity)
        {
            var updatingMovie = _context.Movies.FirstOrDefault(p => p.MovieId == entity.MovieId) ?? throw new NotFoundException();

            updatingMovie.Update(entity);
            _context.SaveChanges();
            return updatingMovie;
        }

        public void Delete(Guid id)
        {
            var movie = GetById(id);
            _context.Movies.Remove(movie);
            _context.SaveChanges();
        }

        public IEnumerable<Movie> GetAllMoviesByUser(string userId)
        {
            return _context.Movies
                .Where(movie => movie.Users.Any(user => user.Username == userId))
                .ToList();
        }

        public IEnumerable<Movie> GetAllMoviesNotInUser(string userId)
        {
            return _context.Movies
                .Where(movie => !movie.Users.Any(user => user.Username == userId))
                .ToList();
        }


    }
}

