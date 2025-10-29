namespace ThothDeskCore.Api.DTOs;


public sealed record EnrollmentRequest(Guid CourseId, Guid UserId, string RoleInCourse);

public sealed record EnrollmentResponse(Guid Id, Guid CourseId, Guid UserId, string RoleInCourse);

public sealed record UpdateEnrollmentRequest(Guid? CourseId, Guid? UserId, string? RoleInCourse);

