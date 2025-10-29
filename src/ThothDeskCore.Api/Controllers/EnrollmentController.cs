using Microsoft.AspNetCore.Mvc;
using ThothDeskCore.Api.DTOs;
using ThothDeskCore.Api.Services.Interfaces;
using ThothDeskCore.Infrastructure;

namespace ThothDeskCore.Api.Controllers;

[ApiController]
[Route("api/thothdesk/v1/[controller]")]
[Produces("application/json")]
public class EnrollmentController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentController(AppDbContext dbContext, IEnrollmentService enrollmentService)
    {
        _dbContext = dbContext;
        _enrollmentService = enrollmentService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EnrollmentResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken, int page = 1, int pageSize = 200)
    {
        var allEnrollments = await _enrollmentService.GetAllAsync(cancellationToken);

        var allEnrollmentsToShow = allEnrollments.OrderBy(e => e.CourseId)
                                                                      .Skip((page - 1) * pageSize)
                                                                      .Take(pageSize).ToList();

        return Ok(new { page, pageSize, allEnrollmentsToShow });
    }


    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(EnrollmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var enrollment = await _enrollmentService.GetByIdAsync(id, cancellationToken);

        if (enrollment is null)
        {
            return NotFound(new { message = $"Enrollment with id {id} was not found." });
        }

        return Ok(enrollment);

    }


    [HttpPost]
    [ProducesResponseType(typeof(EnrollmentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(EnrollmentRequest enrollment, CancellationToken cancellationToken)
    {
        try
        {
            var id = await _enrollmentService.CreateAsync(enrollment, cancellationToken);
            return CreatedAtAction(nameof(GetById), enrollment);

        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }


    [HttpPatch("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEnrollmentRequest enrollment, CancellationToken cancellationToken)
    {
        try
        {
            var isUpdated = await _enrollmentService.UpdateAsync(id, enrollment, cancellationToken);

            if (!isUpdated)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }


    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var isDeleted = await _enrollmentService.DeleteAsync(id, cancellationToken);

        return isDeleted ? NoContent() : NotFound();
        ;
    }
}

