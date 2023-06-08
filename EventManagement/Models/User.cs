using System.ComponentModel.DataAnnotations;

namespace EventManagement.Models
{
    public partial class User
    {
        [Required(ErrorMessage = "This field is required")]
        public Guid UserId { get; set; } 
        [Required(ErrorMessage = "This field is required")]
        public string FirstName { get; set; } = null!;

    }
}
