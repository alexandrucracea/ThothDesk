using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ThothDeskCore.Api.DTOs;
using ThothDeskCore.Api.Services.Interfaces;
using ThothDeskCore.Infrastructure;

namespace ThothDeskCore.Api.Services;

public class CourseService : ICourseService
{
    private readonly AppDbContext _dbContext;

    public CourseService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    //private readonly ICourseService _courseService;
    public async Task<CourseResponse?> GetByIdAsync(Guid courseId)
    {
        var retrievedCourse = await _dbContext.Courses.FindAsync(courseId);

        return retrievedCourse is null ? null : new CourseResponse(retrievedCourse.Id,
                                                                   retrievedCourse.Code,
                                                                   retrievedCourse.Name,
                                                                   retrievedCourse.Semester, 
                                                                   retrievedCourse.Credits, 
                                                                   retrievedCourse.CreatedAt);

    }
}

