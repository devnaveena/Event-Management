namespace EventManagement.Models
{
    public partial class EventAttendeeDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string EventName { get; set; } = string.Empty;


    }
}
