using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using ThothDeskCore.Api.DTOs;
using ThothDeskCore.Api.Extensions;
using ThothDeskCore.Api.Services.Interfaces;
using ThothDeskCore.Domain;
using ThothDeskCore.Infrastructure;

namespace ThothDeskCore.Api.Services;

public class AssignmentService : IAssignmentService
{
    private readonly AppDbContext _dbContext;
    private readonly IValidator<CreateAssignmentRequest> _validator;
    private readonly ICourseService _courseService;

    public AssignmentService(AppDbContext dbContext, IValidator<CreateAssignmentRequest> validator, ICourseService courseService)
    {
        _dbContext = dbContext;
        _validator = validator;
        _courseService = courseService;
    }

    public async Task<AssignmentResponse> CreateAsync(CreateAssignmentRequest request, CancellationToken cancellationToken)
    {
        ValidationResult? assignmentValidationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!assignmentValidationResult.IsValid)
        {
            throw new ValidationException(assignmentValidationResult.Errors);
        }

        var course = await _courseService.GetByIdAsync(request.CourseId);

        if (course is null)
        {
            throw new NotFoundException($"Course {request.CourseId}");
        }

        var assignment = new Assignment(request.CourseId, request.Title, request.Description, request.DueAt, request.MaxPoints);

        _dbContext.Assignments.Add(assignment);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return assignment.ToResponse();
    }

    public async Task<AssignmentResponse> GetByIdAsync(Guid assignmentId, CancellationToken cancellationToken)
    {
        var assignment = await _dbContext.Assignments.AsNoTracking().FirstOrDefaultAsync(a => a.Id == assignmentId, cancellationToken);

        if (assignment is null)
        {
            throw new NotFoundException($"Assignment {assignmentId}");
        }

        return assignment.ToResponse();

    }

    public Task<IEnumerable<AssignmentResponse>> GetAllAsync()
    {
        throw new NotImplementedException(); //TODO
    }

    public async Task<AssignmentResponse> UpdateAsync(Guid id, CreateAssignmentRequest request, CancellationToken cancellationToken)
    {
        ValidationResult? assignmentValidationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!assignmentValidationResult.IsValid)
        {
            throw new ValidationException(assignmentValidationResult.Errors);
        }

        var assignment = await _dbContext.Assignments.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        if (assignment is null)
        {
            throw new NotFoundException($"Assignment {id}");
        }

        //we will double check if the course still exists just to make sure we don't create anomalies in the database

        var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == request.CourseId, cancellationToken);

        if (course is null)
        {
            throw new NotFoundException($"Course {request.CourseId}");
        }

        assignment.Update(request.Title, request.Description, request.DueAt, request.MaxPoints);

        await _dbContext.SaveChangesAsync(cancellationToken); //todo edit to handle concurrency

        return assignment.ToResponse();

    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var assignment = await _dbContext.Assignments.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        if (assignment is null)
        {
            throw new NotFoundException($"Assignment {id} was not found");
        }

        _dbContext.Remove(assignment);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

