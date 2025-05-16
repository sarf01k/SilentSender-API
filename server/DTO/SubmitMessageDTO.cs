namespace server.DTO
{
    public class SubmitMessageDTO
    {
        public required string Content { get; set; }
        public string? Sender { get; set; }
        public Tag? Tag { get; set; }
    }

    public enum Tag
    {
        Praise,
        Suggestion,
        Criticism
    }
}