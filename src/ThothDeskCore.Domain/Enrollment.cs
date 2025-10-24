
namespace ThothDeskCore.Domain
{
    public class Enrollment
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        //Foreign keys
        public Guid CourseId { get; private set; }
        public Guid UserId { get; private set; }

        //Navigation relationships
        public Course Course { get; private set; } = default!;
        //todo add relation for User entity
        public string RoleInCourse { get; private set; }
        public DateTimeOffset EnrolledAt { get; private set; }

        public Enrollment(Guid courseId, Guid userId, string roleInCourse)
        {
            CourseId = courseId;
            UserId = userId;
            RoleInCourse = roleInCourse;
            EnrolledAt = DateTimeOffset.Now;
        }

        // RoleInCourse (Student/Teacher), EnrolledAt
        //Unicitate data de CourseId, UserId
    }
}
