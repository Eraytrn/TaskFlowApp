using TaskFlow.Domain.Enums;

namespace TaskFlow.Application.DTOs;

public class BoardJoinRequestDto
{
    public int Id { get; set; }
    public int BoardId { get; set; }
    public required string BoardTitle { get; set; }
    public int RequesterId { get; set; }
    public required string RequesterName { get; set; }
    public required string RequesterEmail { get; set; }
    public RequestStatus Status { get; set; }
    public DateTime RequestDate { get; set; }
    public DateTime? ResponseDate { get; set; }
    public int? ResponderId { get; set; }
    public string? ResponderName { get; set; }
}
