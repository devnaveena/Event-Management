namespace EventManagement.Helpers
{
    public class Exceptions : Exception
    {
        public Exceptions(string message) : base(message)
        {

        }
    }

    public class ConflictException : Exceptions
    {
        public ConflictException(string message) : base(message)
        {

        }
    }

    public class NotFoundException : Exceptions
    {
        public NotFoundException(string message) : base(message)
        {

        }
    }
    public class BadRequestException : Exceptions
    {
        public BadRequestException(string message) : base(message)
        {

        }
    }

}