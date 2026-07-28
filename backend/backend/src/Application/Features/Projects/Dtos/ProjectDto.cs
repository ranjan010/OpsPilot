namespace Application.Features.Projects.Dtos;

public record ProjectDto(
    Guid Id,
    string Name,
    string Key,
    string Description,
    bool IsArchived,
    DateTime CreatedAt
);
