namespace ThothDeskCore.Api.DTOs;

public record CourseRequest(string Code, string Name, string Semester, int Credits);

public sealed record CourseResponse(Guid Id, string Code, string Name, string Semester, int Credits, DateTimeOffset CreatedAt);

