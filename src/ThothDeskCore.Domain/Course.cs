
namespace ThothDeskCore.Domain
{
    public class Course
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Code { get; set; }
        public string Name { get; set; }
        public string Semester { get; set; }
        public int Credits { get; set; }
        public DateTimeOffset CreatedAt { get; private set; }

        //Navigations for relationships
        public ICollection<Enrollment> Enrollments { get; private set; } = new List<Enrollment>();
        public ICollection<Assignment> Assignments { get; private set; } = new List<Assignment>();

        private Course(string code, string name, string semester, int credits)
        {
            Code = code;
            Name = name;
            Semester = semester;
            Credits = credits;
            CreatedAt = DateTimeOffset.UtcNow;
        }

        public static Course Create(string code, string name, string semester, int credits)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("Course code is required.", nameof(code));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Course name is required. ", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(semester))
            {
                throw new ArgumentException("Semester is required. ", nameof(semester));
            }

            if (credits < 1 || credits > 30)
            {
                throw new ArgumentOutOfRangeException(nameof(credits), "Credits must be between 1 and 30.");
            }

            return new Course(code.Trim().ToUpperInvariant(),
                name.Trim(),
                semester.Trim(),
                credits);

        }
    }
}
