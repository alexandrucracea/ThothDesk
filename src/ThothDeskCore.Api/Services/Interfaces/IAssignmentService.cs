using ThothDeskCore.Api.DTOs;

namespace ThothDeskCore.Api.Services.Interfaces;

public interface IAssignmentService
{
    Task<AssignmentResponse> CreateAsync(CreateAssignmentRequest request, CancellationToken cancellationToken);
    Task<AssignmentResponse> GetByIdAsync(Guid assignmentId, CancellationToken cancellationToken);
    Task<IEnumerable<AssignmentResponse>> GetAllAsync();
    Task<AssignmentResponse> UpdateAsync(Guid id, CreateAssignmentRequest request, CancellationToken cancellationToken);    
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);

}

