using MediatR;
using TaskFlow.Application.Common.Models;

namespace TaskFlow.Application.Features.Labels.Commands.CreateLabel;

public class CreateLabelCommand : IRequest<Result<int>>
{
    public int BoardId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ColorHex { get; set; } = string.Empty;
}
