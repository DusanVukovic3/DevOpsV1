using Movie_Collection.Generic;
using Movie_Collection.Movies.Model;

namespace Movie_Collection.Movies.Repository
{
    public interface IMovieRepository : ICRUDRepository<Movie>
    {
        IEnumerable<Movie> GetAllMoviesByUser(string userId);
        IEnumerable<Movie> GetAllMoviesNotInUser(string userId);
    }
}
