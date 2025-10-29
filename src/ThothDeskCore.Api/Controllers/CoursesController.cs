using Microsoft.AspNetCore.Mvc;
using ThothDeskCore.Api.DTOs;
using ThothDeskCore.Api.Services.Interfaces;
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

    //TODO getAllCourses by Semester => develop a new feature in the service and implement in the controller
    //TODO search for steps in configuring the setup for this project to document it
    [HttpGet]
    public async Task<IActionResult> List([FromQuery] string? semester, int page = 1, int pageSize = 200)
    {
        var allCourses = await _courseService.GetAllAsync();

        var coursesToShow = allCourses.OrderBy(c => c.CreatedAt).
                                                        Skip((page - 1) * pageSize).
                                                        Take(pageSize).ToList();

        return Ok(new { page, pageSize, coursesToShow });

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
        var courseToAdd = new CourseRequest(request.Code, request.Name, request.Semester, request.Credits);

        await _courseService.CreateAsync(courseToAdd);

        return CreatedAtAction(nameof(Get), courseToAdd);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCourseRequest request, CancellationToken ct)
    {
        var isUpdated = await _courseService.UpdateAsync(id, request, ct);

        if (!isUpdated)
        {
            return NotFound();
        }

        return NoContent();
    }


    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var isDeleted = await _courseService.DeleteAsync(id);

        if (!isDeleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}

