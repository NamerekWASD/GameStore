namespace Exceptions
{
    public class NonAuthorizedException : Exception
    {
        public NonAuthorizedException() : base() { }
        public NonAuthorizedException(string message) : base(message) { }
    }
}
