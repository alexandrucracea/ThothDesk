namespace ThothDeskCore.Api.DTOs;

public sealed record CreateAssignmentRequest(Guid CourseId, string Title, string? Description, DateTimeOffset DueAt, int MaxPoints);
public sealed record UpdateAssignmentRequest(string Title, string? Description, DateTimeOffset DueAt, int MaxPoints);
public sealed record AssignmentResponse(Guid Id, Guid CourseId, string Title, string? Description, 
                                        DateTimeOffset DueAt, int MaxPoints, DateTimeOffset CreatedAt);


