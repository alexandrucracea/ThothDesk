using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ThothDeskCore.Api.DTOs;
using ThothDeskCore.Api.Services.Interfaces;
using ThothDeskCore.Domain;
using ThothDeskCore.Infrastructure;

namespace ThothDeskCore.Api.Services;

public class CourseService : ICourseService
{
    private readonly AppDbContext _dbContext;

    public CourseService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<CourseResponse>> GetAllAsync(bool includeChildren = true, CancellationToken ct = default)
    {
        //TODO convert this to use AutoMapper in the future
        if (!includeChildren)
        {
            return await _dbContext.Courses
                .AsNoTracking()
                .Select(c => new CourseResponse(
                    c.Id,
                    c.Code,
                    c.Name,
                    c.Semester,
                    c.Credits,
                    c.CreatedAt,
                    null,
                    null))
                .ToListAsync(ct);
        }

        return await _dbContext.Courses
            .AsNoTracking()
            .Select(c => new CourseResponse(
                c.Id,
                c.Code,
                c.Name,
                c.Semester,
                c.Credits,
                c.CreatedAt,
                c.Assignments
                    .OrderBy(a => a.DueAt)
                    .Select(a => new AssignmentResponse(a.Id,
                                                                 a.CourseId,
                                                                 a.Title,
                                                                 a.Description,
                                                                 a.DueAt,
                                                                 a.MaxPoints,
                                                                 a.CreatedAt))
                    .ToList(),
                c.Enrollments
                    .Select(e => new EnrollmentResponse(e.Id,
                                                                  e.CourseId,
                                                                  e.UserId,
                                                                  e.RoleInCourse))
                    .ToList()
                ))
            .ToListAsync(ct);
    }

    public async Task<CourseResponse?> GetByIdAsync(Guid courseId, bool includeChildren = true, CancellationToken ct = default)
    {

        if (!includeChildren)
        {
            return await _dbContext.Courses
                .AsNoTracking()
                .Where(c => c.Id == courseId)
                .Select(c => new CourseResponse(
                    c.Id,
                    c.Code,
                    c.Name,
                    c.Semester,
                    c.Credits,
                    c.CreatedAt,
                    null,
                    null))
                .FirstOrDefaultAsync(ct);
        }


        return await _dbContext.Courses
            .AsNoTracking()
            .Where(c => c.Id == courseId)
            .Select(c => new CourseResponse(
                c.Id,
                c.Code,
                c.Name,
                c.Semester,
                c.Credits,
                c.CreatedAt,
                c.Assignments
                    .OrderBy(a => a.DueAt)
                    .Select(a => new AssignmentResponse(a.Id,
                        a.CourseId,
                        a.Title,
                        a.Description,
                        a.DueAt,
                        a.MaxPoints,
                        a.CreatedAt))
                    .ToList(),
                c.Enrollments
                    .Select(e => new EnrollmentResponse(e.Id,
                        e.CourseId,
                        e.UserId,
                        e.RoleInCourse))
                    .ToList()))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<Guid> CreateAsync(CourseRequest request, CancellationToken ct = default)
    {
        bool exists = await _dbContext.Courses
            .AnyAsync(c => c.Code == request.Code && c.Semester == request.Semester, ct);

        if (exists)
        {
            throw new InvalidOperationException("A course with the same Code and Semester already exists.");
        }

        var courseToCreate = Course.Create(request.Code, request.Name, request.Semester, request.Credits); 

        _dbContext.Courses.Add(courseToCreate);
        await _dbContext.SaveChangesAsync(ct);

        return courseToCreate.Id;

    }


    public async Task<bool> UpdateAsync(Guid id, UpdateCourseRequest request, CancellationToken ct = default)
    {
        var courseToUpdate = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == id, ct);
        
        if (courseToUpdate is null)
        {
            return false;
        }

        if (request.Credits.HasValue && (request.Credits.Value < 1 || request.Credits.Value > 30))
            throw new ArgumentOutOfRangeException(nameof(request.Credits), "Credits must be between 1 and 30.");

        if (!string.IsNullOrWhiteSpace(request.Code))
            courseToUpdate.Code = request.Code.Trim().ToUpperInvariant();

        if (!string.IsNullOrWhiteSpace(request.Name))
            courseToUpdate.Name = request.Name.Trim();

        if (!string.IsNullOrWhiteSpace(request.Semester))
            courseToUpdate.Semester = request.Semester.Trim();

        if (request.Credits.HasValue)
            courseToUpdate.Credits = request.Credits.Value;

        await _dbContext.SaveChangesAsync(ct);

        return true;
    }
    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var courseToUpdate = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == id, ct);

        if (courseToUpdate is null)
        {
            return false;
        }

        _dbContext.Courses.Remove(courseToUpdate);
        return true;
    }
}

