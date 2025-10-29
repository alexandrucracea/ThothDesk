using ThothDeskCore.Api.DTOs;

namespace ThothDeskCore.Api.Services.Interfaces;

public interface ICourseService
{
    Task<List<CourseResponse>> GetAllAsync(bool includeChildren = true, CancellationToken ct = default);
    Task<CourseResponse?> GetByIdAsync(Guid courseId, bool includeChildren = true, CancellationToken ct = default);
    Task<Guid> CreateAsync(CourseRequest request, CancellationToken ct = default);
    Task<bool> UpdateAsync(Guid id, UpdateCourseRequest request, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);



}

