using Movie_Collection.Movies.Model;

namespace Movie_Collection.Authentication.DTO
{
    public class UserDTO
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public List<Movie> Movies { get; set; } = new List<Movie>();

        public void AddMovie(Movie movieToAdd)
        {
            Movies.Add(movieToAdd);
        }
    }
}
