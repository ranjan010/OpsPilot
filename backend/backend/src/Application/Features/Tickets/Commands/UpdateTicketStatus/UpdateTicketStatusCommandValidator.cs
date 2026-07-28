using Domain.Enums;
using FluentValidation;

namespace Application.Features.Tickets.Commands.UpdateTicketStatus;

public class UpdateTicketStatusCommandValidator : AbstractValidator<UpdateTicketStatusCommand>
{
    public UpdateTicketStatusCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty().WithMessage("Ticket ID is required.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid ticket status value.");
    }
}
