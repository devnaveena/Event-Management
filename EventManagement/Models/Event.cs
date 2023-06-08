using System.ComponentModel.DataAnnotations;
using EventManagement.Helpers;
namespace EventManagement.Models
{
    public partial class Event
    {
        public Guid EventId { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string? EventName { get; set; } = string.Empty;
        // [ValidateDate("yyyy-MM-dd HH:mm:ss")]
        public DateTime? EventStartDateTime { get; set; }
        // [ValidateDate("yyyy-MM-dd HH:mm:ss")]
        public DateTime? EventEndDateTime { get; set; }


    }
}
