namespace TaskFlow.Application.Features.Comments.DTOs;

public class CommentDto
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public int TaskItemId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty; // Yorumu yapan kullanıcı
    public DateTime CreatedDate { get; set; }
}
