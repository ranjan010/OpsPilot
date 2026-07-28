using MediatR;
using System;

namespace Application.Features.Projects.Commands.DeleteProject;

public record DeleteProjectCommand(Guid Id) : IRequest<Unit>;
