using FluentValidation;

namespace Application.Features.Projects.Commands.CreateProject;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name is required.")
            .MaximumLength(120);

        RuleFor(x => x.Key)
            .NotEmpty().WithMessage("Project key is required.")
            .Matches("^[A-Z0-9-]+$").WithMessage("Project key must contain uppercase letters, numbers, or hyphens.");
    }
}
