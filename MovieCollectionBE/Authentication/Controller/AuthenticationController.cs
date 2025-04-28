using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie_Collection.Authentication.DTO;
using Movie_Collection.Authentication.Model;
using Movie_Collection.Authentication.Service;

namespace Movie_Collection.Authentication.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IUserService userService, IMapper mapper, ILogger<AuthenticationController> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("register")]
        public ActionResult<User> Register([FromBody] RegisterUserDTO request)
        {
            _userService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            if (!ModelState.IsValid)
            {
                return Conflict(ModelState);
            }

            if (!_userService.IsEmailUnique(request.Email))
            {
                return Conflict("User with that email already exists!");
            }

            if (!_userService.IsUsernameUnique(request.Username))
            {
                return Conflict("User with that username already exists!"); 
            }

            var user = _mapper.Map<User>(request);

            user.UserId = Guid.NewGuid();
            user.Username = request.Username;
            user.Email = request.Email;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.UserRole = UserRole.Regular;   //Default role is regular one

            var newUser = _userService.Create(user);

            return Ok(newUser);

        }

        [HttpPost("login")]
        public ActionResult<string> Login(LoginUserDTO request)
        {
            _logger.LogInformation("LOGGING WHILE LOGGING IN");

            var user = _userService.GetByUsername(request.Username);

            if (user.Username != request.Username)
            {
                return Conflict("User not found");
            }

            if (!_userService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return Conflict("Wrong password!");     // in production, it is safer to put -> username or password incorrect
            }

            string token = _userService.CreateToken(user);

            return Ok(token);
        }


        [HttpGet, Authorize]
        public ActionResult<string> GetLoggedUser() //Gets the username of logged in user, or rather from token that is currently in localStorage
        {
            _logger.LogInformation("This is logging while checking for logged user");

            var userName = User?.Identity?.AuthenticationType;  
            
            return Ok(userName);
        }


    }
}
