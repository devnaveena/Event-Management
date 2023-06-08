namespace EventManagement.Helpers
{
      public class ErrorDto
    {
        public int StatusCode { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}