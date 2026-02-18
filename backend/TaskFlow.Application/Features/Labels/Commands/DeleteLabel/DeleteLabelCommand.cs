using MediatR;
using TaskFlow.Application.Common.Models;

namespace TaskFlow.Application.Features.Labels.Commands.DeleteLabel;

public class DeleteLabelCommand : IRequest<Result>
{
    public int Id { get; set; }

    public DeleteLabelCommand(int id)
    {
        Id = id;
    }
}
