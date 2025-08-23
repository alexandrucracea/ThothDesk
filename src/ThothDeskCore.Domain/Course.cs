
namespace ThothDeskCore.Domain
{
    public class Course
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Semester { get; private set; }
        public int Credits { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }

        public Course(string code, string name, string semester, int credits)
        {
            Code = code;
            Name = name;
            Semester = semester;
            Credits = credits;
            CreatedAt = DateTimeOffset.Now;
        }

    }
}
