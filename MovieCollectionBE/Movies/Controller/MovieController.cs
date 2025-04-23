using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movie_Collection.Movies.DTO;
using Movie_Collection.Movies.Exceptions;
using Movie_Collection.Movies.Model;
using Movie_Collection.Movies.Service;

namespace Movie_Collection.Movies.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _iMovieService;
        private readonly IMapper _mapper;

        public MovieController(IMovieService movieService, IMapper mapper)
        {
            _iMovieService = movieService;
            _mapper = mapper;
        }

        [HttpGet]      
        public IActionResult GetAll()
        {
            return Ok(_iMovieService.GetAll()); //Works
        }


        [HttpGet("byUsername/{username}")]
        [Authorize(Roles = "Regular")]
        public IActionResult GetAllByUsername([FromRoute] string username)
        {
            return Ok(_iMovieService.GetAllMoviesByUser(username));
        }

        [HttpGet("inverseByUsername/{username}")]
        [Authorize(Roles = "Regular")]
        public IActionResult GetAllByUsernameInverse([FromRoute] string username)
        {
            return Ok(_iMovieService.GetAllMoviesNotInUser(username));
        }

        [HttpGet("allGenres")]
        public IActionResult GetAllGenres()
        {
            try
            {
                var allGenres = Movie.GetAllGenres();
                return Ok(allGenres);
            }
            catch (NotFoundException )
            {
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            return Ok(_iMovieService.GetById(id));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([FromBody] MovieDTO movieDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movie = _mapper.Map<Movie>(movieDto);
            movie.MovieId = Guid.NewGuid();

            if (Enum.TryParse(movieDto.Genre, true, out Genre genre) && genre != Genre.NoSelection)     //Movies without genre cannot be created
            {
                movie.Genre = genre;
            }
            else
            {
                return BadRequest("Invalid Genre");
            }

            var newMovie = _iMovieService.Create(movie);

            if (newMovie == null)
            {
                return NotFound();
            }

            return Ok(newMovie);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] MovieDTO movieDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movie = _mapper.Map<Movie>(movieDto);
            movie.MovieId = id;

            try
            {
                var result = _iMovieService.Update(movie);
                return Ok(result);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            try
            {
                _iMovieService.Delete(id);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("search")]
        [Authorize(Roles = "Admin")]
        public IActionResult SearchMovies([FromBody] MovieSearchDTO dto)
        {
            return Ok(_iMovieService.SearchMovies(dto));
        }

        [HttpPost("searchByUser/{username}")]
        [Authorize(Roles = "Regular")]
        public IActionResult SearchMoviesByUser([FromBody] MovieSearchDTO dto, [FromRoute] string username)
        {
            return Ok(_iMovieService.SearchMoviesByUser(dto, username));
        }



    }
}
