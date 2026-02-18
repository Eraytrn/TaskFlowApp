using TaskFlow.Application.Common.Mappings;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Features.Labels.DTOs;

public class LabelDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ColorHex { get; set; } = string.Empty;
}
