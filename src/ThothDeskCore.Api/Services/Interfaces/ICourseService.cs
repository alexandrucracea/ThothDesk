using ThothDeskCore.Api.DTOs;

namespace ThothDeskCore.Api.Services.Interfaces;

public interface ICourseService
{
    Task<CourseResponse?> GetByIdAsync(Guid courseId);
}

