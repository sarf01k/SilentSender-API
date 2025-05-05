using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace server.Models
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "Email is required")]
        public override string? Email { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        public ICollection<Message>? Messages { get; set; }

        public string? Note { get; set; }

        [Required(ErrorMessage = "Invalid Google user ID")]
        public string? GoogleUserId { get; set; }
    }
}