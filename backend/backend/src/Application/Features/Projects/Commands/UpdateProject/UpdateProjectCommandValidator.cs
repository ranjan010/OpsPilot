using FluentValidation;
using System;

namespace Application.Features.Projects.Commands.UpdateProject;

public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Project ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name is required.")
            .MaximumLength(120);

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Project description cannot exceed 500 characters.");
    }
}