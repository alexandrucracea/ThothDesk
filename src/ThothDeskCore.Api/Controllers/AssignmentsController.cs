using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThothDeskCore.Api.DTOs;
using ThothDeskCore.Api.Extensions;
using ThothDeskCore.Domain;
using ThothDeskCore.Infrastructure;

namespace ThothDeskCore.Api.Controllers;

[ApiController]
[Route("api/thothdesk/v1/[controller]")]
public class AssignmentsController(AppDbContext db) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Instructor, Admin")]
    public async Task<IActionResult> Create(CreateAssignmentRequest request)
    {
        if (!await db.Courses.AnyAsync(course => course.Id == request.CourseId))
        {
            return BadRequest($"Course {request.CourseId} not found");
        }

        var assignment = new Assignment(
            request.CourseId,
            request.Title,
            request.Description,
            request.DueAt,
            request.MaxPoints
        );

        db.Assignments.Add(assignment);

        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = assignment.Id }, assignment.ToResponse());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var assignment = await db.Assignments
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (assignment == null)
        {
            return NotFound();
        }

        return Ok(assignment.ToResponse());
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? courseId, int page = 1, int pageSize = 20)
    {
        var query = db.Assignments.AsNoTracking();
        if (courseId is not null)
        {
            query = query.Where(assignment => assignment.CourseId == courseId);
        }

        var items = await query.OrderByDescending(assignment => assignment.CreatedAt)
                                                    .Skip((page - 1) * pageSize)
                                                    .Take(pageSize)
                                                    .Select(assignment => assignment.ToResponse())
                                                    .ToListAsync();

        return Ok(items);
    }

    [Authorize(Roles = "Instructor,Admin")]
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateAssignmentRequest request)
    {
        var assignment = await db.Assignments.FindAsync(id);
        
        if (assignment is null)
        {
            return NotFound();
        }

        assignment.Update(request.Title, request.Description, request.DueAt, request.MaxPoints);

        await db.SaveChangesAsync();

        return NoContent();

    }

    [Authorize(Roles = "Instructor,Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var assignment = await db.Assignments.FindAsync(id);

        if (assignment is null)
        {
            return NotFound();
        }

        db.Assignments.Remove(assignment);
        await db.SaveChangesAsync();

        return NoContent();
    }
}

