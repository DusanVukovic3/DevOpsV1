using Movie_Collection.Authentication.Model;
using System.ComponentModel.DataAnnotations;

namespace Movie_Collection.Movies.Model
{
    public enum Genre   //If this list won't be updated often, then it's better if it's enum
    {
        NoSelection, //0
        Action, 
        Comedy,
        Horror,
        Cartoon,
        Adventure,
        Romance,
        Science_Fiction
    }

    public class Movie
    {
        public Guid MovieId { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "Maximum length is 30 characters!")]
        [MinLength(1, ErrorMessage = "Minimum length is 1 character!")]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [Required]
        [MaxLength(25, ErrorMessage = "Maximum length is 25 characters!")]
        [MinLength(1, ErrorMessage = "Minimum length is 1 character!")]
        public string Author { get; set; } = string.Empty;

        [Required]
        public Genre Genre { get; set; }

        [Required]
        public DateOnly ReleaseDate { get; set; }

        public List<User> Users { get; set; } = new List<User>();


        public Movie() { }

        public Movie(Guid movieId, string name, string description, string author, Genre genre, DateOnly releaseDate)
        {
            MovieId = movieId;
            Name = name;
            Description = description;
            Author = author;
            Genre = genre;
            ReleaseDate = releaseDate;
        }

        public void Update(Movie selectedMovie)    
        {
            Description = selectedMovie.Description;
            Author = selectedMovie.Author;
            Genre = selectedMovie.Genre;
        }

        public static IEnumerable<string> GetAllGenres()
        {
            return Enum.GetNames(typeof(Genre));
        }

        public void AddUser(User updatingUser)
        {
            Users.Add(updatingUser);
        }
    }
}
