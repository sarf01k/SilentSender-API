using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class Message
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Message content is required")]
        public string? Content { get; set; }

        [Required(ErrorMessage = "Invalid sender ID")]
        public string? SenderId { get; set; }

        [Required(ErrorMessage = "Invalid recipient ID")]
        public string? RecipientId { get; set; }

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