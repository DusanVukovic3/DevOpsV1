using Movie_Collection.Movies.Model;

namespace Movie_Collection.Movies.DTO
{
    public class MovieSearchDTO
    {
        public string? NameSearch { get; set; }
        public string? AuthorSearch { get; set; }
        public int YearSearch { get; set; }
        public Genre GenreSearch { get; set; }

    }
}
