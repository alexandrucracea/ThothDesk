using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThothDeskCore.Domain;
using ThothDeskCore.Infrastructure;

namespace ThothDeskCore.Api.Controllers;

[ApiController]
[Route("api/thothdesk/v1/[controller]")]
public class CoursesController(AppDbContext db) : ControllerBase
{
    public record CreateCourseRequest(string Code, string Name, string Semester, int Credits);

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] string? semester, int page = 1, int pageSize = 200)
    {
        var query = db.Courses.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(semester))
        {
            query = query.Where(c => c.Semester == semester);
        }

        var items = await query.OrderBy(c => c.CreatedAt).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return Ok(items);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
        => await db.Courses.FindAsync(id) is { } c ? Ok(c) : NotFound();

    [HttpPost]
    public async Task<IActionResult> Create(CreateCourseRequest request)
    {
        var course = new Course(request.Code, request.Name, request.Semester, request.Credits);

        db.Courses.Add(course);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = course.Id }, course);
    }

    [HttpPatch]
    public async Task<IActionResult> Update(Guid id, CreateCourseRequest request)
    {
        var course = await db.Courses.FindAsync(id);
        if (course is null)
        {
            return NotFound();
        }

        db.Entry(course).CurrentValues.SetValues(new
        {
            course.Id, request.Code, request.Semester, request.Credits
        });

        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var course = await db.Courses.FindAsync(id);
        
        if (course is null)
        {
            return NotFound();
        }

        db.Courses.Remove(course);
        await db.SaveChangesAsync();

        return NoContent();
    } 
}

