namespace ThothDeskCore.Api.DTOs;

public record CourseRequest(string Code, string Name, string Semester, int Credits);

public sealed record CourseResponse(Guid Id,
                                    string Code,
                                    string Name,
                                    string Semester,
                                    int Credits,
                                    DateTimeOffset CreatedAt,
                                    List<AssignmentResponse>? Assignments = null,
                                    List<EnrollmentResponse>? Enrollments = null);

public sealed record UpdateCourseRequest(string? Code, string? Name, string? Semester, int? Credits);

