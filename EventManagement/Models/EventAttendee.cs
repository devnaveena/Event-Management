
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace EventManagement.Models
{
    public partial class EventAttendee
    {

        public Guid Id { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public Guid? UserId { get; set; }

        [ForeignKey("EventId")]
        public Event? Event { get; set; }

        public Guid? EventId { get; set; }


    }
}
