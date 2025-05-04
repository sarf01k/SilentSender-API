using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class User
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        public ICollection<Message>? Messages { get; set; }
    }
}