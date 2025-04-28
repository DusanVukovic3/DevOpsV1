using Movie_Collection.Movies.DTO;
using Movie_Collection.Movies.Model;
using Movie_Collection.Movies.Repository;

namespace Movie_Collection.Movies.Service
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _iMovieRepository;

        public MovieService(IMovieRepository iMovieRepository)
        {
            _iMovieRepository = iMovieRepository;
        }

        public Movie Create(Movie movie)
        {
            return _iMovieRepository.Create(movie);
        }

        public void Delete(Guid movieId)
        {
            _ = GetById(movieId);

            _iMovieRepository.Delete(movieId);
        }

        public IEnumerable<Movie> GetAll()
        {
            return _iMovieRepository.GetAll();
        }

        public Movie GetById(Guid id)
        {
            return _iMovieRepository.GetById(id);
        }

        public Movie Update(Movie movie)
        {
            return _iMovieRepository.Update(movie);
        }

        public IEnumerable<Movie> SearchMovies(MovieSearchDTO dto)  
        {
            var matchingMovies = GetAll()   //I changed method not to use _context, like bellow, so it can be done with unit test, instead of integration
                .Where(movie =>
                    (string.IsNullOrEmpty(dto.NameSearch) || movie.Name.ToLower().Contains(dto.NameSearch.ToLower())) &&
                    (string.IsNullOrEmpty(dto.AuthorSearch) || movie.Author.ToLower().Contains(dto.AuthorSearch.ToLower())) &&
                    (dto.GenreSearch == 0 || movie.Genre == dto.GenreSearch) &&
                    (dto.YearSearch == 0 || movie.ReleaseDate.Year == dto.YearSearch));

            return matchingMovies;
        }

        public IEnumerable<Movie> SearchMoviesByUser(MovieSearchDTO dto, string username) 
        {
            var matchingMovies = GetAllMoviesByUser(username)   //I changed method not to use _context, like bellow, so it can be done with unit test, instead of integration
                .Where(movie =>
                    (string.IsNullOrEmpty(dto.NameSearch) || movie.Name.ToLower().Contains(dto.NameSearch.ToLower())) &&
                    (string.IsNullOrEmpty(dto.AuthorSearch) || movie.Author.ToLower().Contains(dto.AuthorSearch.ToLower())) &&
                    (dto.GenreSearch == 0 || movie.Genre == dto.GenreSearch) &&
                    (dto.YearSearch == 0 || movie.ReleaseDate.Year == dto.YearSearch));

            return matchingMovies;
        }

        public IEnumerable<Movie> GetAllMoviesByUser(string userId)
        {
            return _iMovieRepository.GetAllMoviesByUser(userId);
        }

        public IEnumerable<Movie> GetAllMoviesNotInUser(string userId)
        {
            return _iMovieRepository.GetAllMoviesNotInUser(userId);
        }
    }
}
