namespace Movie_Collection.Movies.Exceptions
{
    public class DuplicateMovieException : Exception
    {
        public DuplicateMovieException(string message) : base(message)
        {
        }
    }

}
