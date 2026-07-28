using FluentValidation;

namespace Application.Features.Tickets.Commands.CreateTicket;

public class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommand>
{
    public CreateTicketCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Ticket title is required.")
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(4000);
    }
}
