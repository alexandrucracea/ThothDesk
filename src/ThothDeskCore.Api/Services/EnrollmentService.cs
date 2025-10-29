using Microsoft.EntityFrameworkCore;
using ThothDeskCore.Api.DTOs;
using ThothDeskCore.Api.Services.Interfaces;
using ThothDeskCore.Domain;
using ThothDeskCore.Infrastructure;

namespace ThothDeskCore.Api.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly AppDbContext _dbContext;

        public EnrollmentService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<EnrollmentResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Enrollments.AsNoTracking()
                .Select(e => new EnrollmentResponse(e.Id,
                                                             e.CourseId,
                                                             e.UserId,
                                                             e.RoleInCourse))
                .ToListAsync(cancellationToken);
        }

        public async Task<EnrollmentResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Enrollments.AsNoTracking()
                                               .Where(e => e.Id == id)
                                               .Select(e => new EnrollmentResponse(e.Id,
                                                   e.CourseId,
                                                   e.UserId,
                                                   e.RoleInCourse))
                                               .FirstOrDefaultAsync(cancellationToken);

        }

        public async Task<Guid> CreateAsync(EnrollmentRequest request, CancellationToken cancellationToken = default)
        {
            var exists = await _dbContext.Enrollments.AnyAsync(e => e.UserId == request.UserId && e.CourseId == request.CourseId, cancellationToken);

            if (exists)
            {
                throw new InvalidOperationException("An enrollment for the specified user in the spciefied course already exists");
            }

            var enrollmentToCreate = Enrollment.Create(request.CourseId, request.UserId, request.RoleInCourse);

            _dbContext.Enrollments.Add(enrollmentToCreate);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return enrollmentToCreate.Id;
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateEnrollmentRequest request, CancellationToken cancellationToken = default)
        {
            var enrollmentToUpdate =
                await _dbContext.Enrollments.FirstOrDefaultAsync(e =>
                    e.CourseId == request.CourseId && e.UserId == request.UserId, cancellationToken: cancellationToken);

            if (enrollmentToUpdate == null)
            {
                return false;
            }

            enrollmentToUpdate.Update(request.CourseId, request.UserId, request.RoleInCourse);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var affectedLines = await _dbContext.Enrollments.Where(e => e.Id == id)
                                                                .ExecuteDeleteAsync(cancellationToken);

            return affectedLines > 0;

        }
    }
}
