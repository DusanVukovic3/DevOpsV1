using System.ComponentModel.DataAnnotations;

namespace Movie_Collection.Movies.DTO
{
    public class MovieDTO
    {
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
        public string Genre { get; set; } = string.Empty;

        [Required]
        public DateOnly ReleaseDate { get; set; }


        // Ucim github al preko konzole
    }
}
