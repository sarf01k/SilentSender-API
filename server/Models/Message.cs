using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class Message
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Message content is required")]
        public string? Content { get; set; }

        [Required(ErrorMessage = "Invalid recipient ID")]
        public string? RecipientId { get; set; }

        public string? Sender { get; set; }

        public Tag? Tag { get; set; }

        public bool IsAnonymous { get; set; } = true;
        public bool Flagged { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum Tag
    {
        Praise,
        Suggestion,
        Criticism
    }
}