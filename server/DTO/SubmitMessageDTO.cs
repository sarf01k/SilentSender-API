namespace server.DTO
{
    public class SubmitMessageDTO
    {
        public required string Content { get; set; }
        public string? RecipientId { get; set; }
        public string? Sender { get; set; }
        public Tag? Tag { get; set; }
        public bool IsAnonymous { get; set; } = true;
        public bool Flagged { get; set; } = false;
    }

    public enum Tag
    {
        Praise,
        Suggestion,
        Criticism
    }
}