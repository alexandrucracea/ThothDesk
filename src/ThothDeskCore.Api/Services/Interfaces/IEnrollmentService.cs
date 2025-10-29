using ThothDeskCore.Api.DTOs;

namespace ThothDeskCore.Api.Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task<List<EnrollmentResponse>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<EnrollmentResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Guid> CreateAsync(EnrollmentRequest request, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(Guid id, UpdateEnrollmentRequest request, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
