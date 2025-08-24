
using System.Security.Cryptography;

namespace ThothDeskCore.Domain;

public class Assignment
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid CourseId { get; private set; }

    public string Title { get; private set; } = default!;

    public string? Description { get; private set; }

    public DateTimeOffset DueAt { get; private set; }

    public int MaxPoints { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.Now;

    public Assignment(Guid courseId, string title, string? description, DateTimeOffset dueAt, int maxPoints)
    {
        CourseId = courseId;
        Title = title;
        Description = description;
        DueAt = dueAt;
        MaxPoints = maxPoints;
    }

    //Update is here to provide a general rule when it comes to updating the data from this entity
    public void Update(string title, string? description, DateTimeOffset dueAt, int maxPoints)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title cannot be empty");
        }

        if (maxPoints <= 0)
        {
            throw new ArgumentException("MaxPoints must be a positive integer");
        }

        if (dueAt < DateTimeOffset.Now)
        {
            throw new ArgumentException("Date due must be in the future");
        }

        Title = title;
        Description = description;
        DueAt = dueAt;
        MaxPoints = maxPoints;
    }

}

