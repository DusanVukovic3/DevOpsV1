using Movie_Collection.Generic;
using Movie_Collection.Movies.DTO;
using Movie_Collection.Movies.Model;

namespace Movie_Collection.Movies.Service
{
    public interface IMovieService : ICRUDRepository<Movie>
    {
        IEnumerable<Movie> SearchMovies(MovieSearchDTO dto);
        IEnumerable<Movie> SearchMoviesByUser(MovieSearchDTO dto, string username);
        IEnumerable<Movie> GetAllMoviesByUser(string userId);
        IEnumerable<Movie> GetAllMoviesNotInUser(string userId);
    }
}
