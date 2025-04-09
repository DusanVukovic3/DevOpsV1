namespace Movie_Collection.Authentication.DTO
{
    public class AddMovieToUserDTO
    {
        public string Username { get; set; } = string.Empty;
        public Guid MovieId { get; set; }
    }
}
