using ThothDeskCore.Api.DTOs;
using ThothDeskCore.Domain;

namespace ThothDeskCore.Api.Extensions;

public static class AssignmentMappingExtensions
{
    public static AssignmentResponse ToResponse(this Assignment assignment)
    {
        return new AssignmentResponse(
            assignment.Id,
            assignment.CourseId,
            assignment.Title,
            assignment.Description,
            assignment.DueAt,
            assignment.MaxPoints,
            assignment.CreatedAt
            );
    }
}

