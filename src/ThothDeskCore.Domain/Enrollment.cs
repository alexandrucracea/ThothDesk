
using System.ComponentModel.DataAnnotations;

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

        [Required]
        [StringLength(64)]
        public string RoleInCourse { get; private set; }
        public DateTimeOffset EnrolledAt { get; private set; }

        private Enrollment(Guid courseId, Guid userId, string roleInCourse)
        {
            CourseId = courseId;
            UserId = userId;
            RoleInCourse = roleInCourse;
            EnrolledAt = DateTimeOffset.Now;
        }

        public static Enrollment Create(Guid courseId, Guid userId, string roleInCourse)
        {
            if (string.IsNullOrWhiteSpace(roleInCourse))
            {
                throw new ArgumentException("You must specify the role that the student has in the specified course", nameof(roleInCourse));
            }

            if (roleInCourse.Length > 64)
            {
                throw new ArgumentException("Role in course should not have a length grater than 64 characters", nameof(roleInCourse));
            }

            return new Enrollment(courseId, userId, roleInCourse);
        }

        public static void Validate(string? roleInCourse)
        {
            if (roleInCourse is null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(roleInCourse))
            {
                throw new ArgumentException("You must specify the role that the student has in the specified course", nameof(roleInCourse));
            }

            if (roleInCourse.Length > 64)
            {
                throw new ArgumentException("Role in course should not have a length grater than 64 characters", nameof(roleInCourse));
            }
        }

        public void Update(Guid? courseId, Guid? userId, string? roleInCourse)
        {
            Validate(roleInCourse);


            if (courseId is not null)
            {
                CourseId = courseId.Value;
            }

            if (userId is not null)
            {
                UserId = userId.Value;
            }

            if (!string.IsNullOrWhiteSpace(roleInCourse))
            {
                RoleInCourse = roleInCourse;
            }

        }
        // RoleInCourse (Student/Teacher), EnrolledAt
        //Unicitate data de CourseId, UserId
    }
}
