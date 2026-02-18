using FluentValidation;

namespace TaskFlow.Application.Features.TaskItems.Commands.ChangeTaskStatus;

public class ChangeTaskStatusCommandValidator : AbstractValidator<ChangeTaskStatusCommand>
{
    public ChangeTaskStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0.");

        RuleFor(x => x.NewStatus)
            .IsInEnum().WithMessage("NewStatus must be a valid TaskStatus (0=Todo, 1=InProgress, 2=Done).");
    }
}
