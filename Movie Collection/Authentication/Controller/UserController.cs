using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie_Collection.Authentication.DTO;
using Movie_Collection.Authentication.Model;
using Movie_Collection.Authentication.Service;
using Movie_Collection.Movies.Exceptions;

namespace Movie_Collection.Authentication.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAll()
        {
            return Ok(_userService.GetAll());
        }

        [HttpGet("getAllDto")]
        [Authorize]
        public IActionResult GetAllDto()
        {
            return Ok(_userService.GetAllDto());
        }

        [HttpGet("getAllExceptLogged/{username}")]
        [Authorize]
        public IActionResult GetAllExceptLogged([FromRoute] string username)
        {
            return Ok(_userService.GetAllExceptLogged(username));
        }

        [HttpGet("getByUsername")]
        [Authorize]
        public IActionResult GetByUsername(string username)
        {
            return Ok(_userService.GetByUsername(username));
        }


        [HttpPut("addMovieToUser/")]
        [Authorize(Roles = "Regular")]
        public IActionResult AddMovieToUser([FromBody] AddMovieToUserDTO request)
        {
            try
            {
                User userWithMovies = _userService.AddMovieToList(request.Username, request.MovieId);
                return Ok(userWithMovies.Username + " sucesfully added movie!");
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch (DuplicateMovieException ex)
            {
                return Conflict(new { ex.Message });
            }
        }

        [HttpPut("removeMovieFromUser/")]
        [Authorize(Roles = "Regular")]
        public IActionResult RemoveMovieFromUser([FromBody] AddMovieToUserDTO request)
        {
            try
            {
                User userWithMovies = _userService.RemoveMovieFromUser(request.Username, request.MovieId);
                return Ok(userWithMovies.Username + " sucesfully removed movie!");
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch (DuplicateMovieException ex)
            {
                return Conflict(new { ex.Message });
            }
        }




    }
}
