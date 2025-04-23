using Movie_Collection.Movies.Model;
using System.ComponentModel.DataAnnotations;

namespace Movie_Collection.Authentication.Model
{
    public enum UserRole
    {
        Admin,
        Regular,
    }

    public class User
    {
        public Guid UserId {  get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Username { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = new byte[32];
        public byte[] PasswordSalt { get; set; } = new byte[32];
        public UserRole UserRole { get; set; }
        public List<Movie> Movies { get; set; } = new List<Movie>();

        public User() {}
        public User(string email, string username, byte[] passwordSalt, byte[] passwordHash, UserRole userRole, List<Movie> movies)
        {
            Email = email;
            Username = username;
            PasswordSalt = passwordSalt;
            PasswordHash = passwordHash;
            UserRole = userRole;
            Movies = movies;
        }

        public void Update(User entity)
        {
            Username = entity.Username;
            UserRole = entity.UserRole;
        }

        public void AddMovie(Movie movie)
        {
            Movies.Add(movie);
        }

        public void RemoveMovie(Movie movie)
        {
            Movies.Remove(movie);
        }
    }

}
