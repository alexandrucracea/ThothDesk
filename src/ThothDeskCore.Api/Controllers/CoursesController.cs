using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThothDeskCore.Api.DTOs;
using ThothDeskCore.Api.Services.Interfaces;
using ThothDeskCore.Domain;
using ThothDeskCore.Infrastructure;

namespace ThothDeskCore.Api.Controllers;

[ApiController]
[Route("api/thothdesk/v1/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ICourseService _courseService;

    public CoursesController(AppDbContext db, ICourseService courseService)
    {
        _db = db;
        _courseService = courseService;
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] string? semester, int page = 1, int pageSize = 200)
    {
        var query = _db.Courses.AsNoTracking(); //TODO to search and document what this does

        if (!string.IsNullOrWhiteSpace(semester))
        {
            query = query.Where(c => c.Semester == semester);
        }

        var items = await query.OrderBy(c => c.CreatedAt).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return Ok(items);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        CourseResponse? course = await _courseService.GetByIdAsync(id);

        if (course == null)
        {
            return NotFound(new { message = $"Course with id {id} was not found." }); 
        }

        return Ok(course);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CourseRequest request)
    {
        var course = new Course(request.Code, request.Name, request.Semester, request.Credits);

        _db.Courses.Add(course);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = course.Id }, course);
    }

    [HttpPatch]
    public async Task<IActionResult> Update(Guid id, CourseRequest request)
    {
        var course = await _db.Courses.FindAsync(id);
        if (course is null)
        {
            return NotFound();
        }

        _db.Entry(course).CurrentValues.SetValues(new
        {
            course.Id,
            request.Code,
            request.Semester,
            request.Credits
        });

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var course = await _db.Courses.FindAsync(id);

        if (course is null)
        {
            return NotFound();
        }

        _db.Courses.Remove(course);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}

