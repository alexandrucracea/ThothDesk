using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThothDeskCore.Api.DTOs;
using ThothDeskCore.Api.Extensions;
using ThothDeskCore.Api.Services.Interfaces;
using ThothDeskCore.Infrastructure;

namespace ThothDeskCore.Api.Controllers;

[ApiController]
[Route("api/thothdesk/v1/[controller]")]
public class AssignmentsController : ControllerBase
{
    private readonly IAssignmentService _assignmentService;
    private readonly AppDbContext _db;

    public AssignmentsController(AppDbContext db, IAssignmentService assignmentService)
    {
        _db = db;
        _assignmentService = assignmentService;
    }
    [HttpPost]
    //[Authorize(Roles = "Instructor, Admin")]
    public async Task<IActionResult> Create(CreateAssignmentRequest request, CancellationToken cancellationToken)
    {
        var created = await _assignmentService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AssignmentResponse),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AssignmentResponse),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(AssignmentResponse),StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var assignment = await _assignmentService.GetByIdAsync(id, cancellationToken);

        return Ok(assignment);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? courseId, int page = 1, int pageSize = 20)
    {
        var query = _db.Assignments.AsNoTracking();
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

    //[Authorize(Roles = "Instructor,Admin")]
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateAssignmentRequest request)
    {
        var assignment = await _db.Assignments.FindAsync(id);

        if (assignment is null)
        {
            return NotFound();
        }

        assignment.Update(request.Title, request.Description, request.DueAt, request.MaxPoints);

        await _db.SaveChangesAsync();

        return NoContent();

    }

    //[Authorize(Roles = "Instructor,Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var assignment = await _db.Assignments.FindAsync(id);

        if (assignment is null)
        {
            return NotFound();
        }

        _db.Assignments.Remove(assignment);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}

