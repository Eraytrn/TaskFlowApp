using FluentValidation;

namespace TaskFlow.Application.Features.TaskItems.Commands.AssignTaskToUser;

public class AssignTaskToUserCommandValidator : AbstractValidator<AssignTaskToUserCommand>
{
    public AssignTaskToUserCommandValidator()
    {
        RuleFor(x => x.TaskItemId)
            .GreaterThan(0).WithMessage("TaskItemId must be greater than 0.");

        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("UserId must be greater than 0.");
    }
}
