using FluentValidation;

namespace TaskFlow.Application.Features.Comments.Commands.CreateComment;

public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentCommandValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required.")
            .MaximumLength(500).WithMessage("Content cannot exceed 500 characters.");

        RuleFor(x => x.TaskItemId)
            .GreaterThan(0).WithMessage("TaskItemId must be greater than 0.");

        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("UserId must be greater than 0.");
    }
}
