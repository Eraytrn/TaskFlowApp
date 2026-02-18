using FluentValidation;

namespace TaskFlow.Application.Features.TaskItems.Commands.CreateTaskItem;

public class CreateTaskItemCommandValidator : AbstractValidator<CreateTaskItemCommand>
{
    public CreateTaskItemCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters.");

        RuleFor(x => x.BoardId)
            .GreaterThan(0).WithMessage("BoardId must be greater than 0.");

        RuleFor(x => x.AssignedUserId)
            .GreaterThan(0).When(x => x.AssignedUserId.HasValue)
            .WithMessage("AssignedUserId must be greater than 0 if provided.");
    }
}