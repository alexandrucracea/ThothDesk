namespace ThothDeskCore.Domain;

public class Submission
{
    public Guid Id { get; private set; }

    public Guid AssignmentId { get; private set; }

    public Guid StudentId { get; private set; }

    public int AttemptNo { get; private set; }

    public EStatus Status { get; private set; }

    public string PayloadUrlOrText { get; private set; }


    public Submission(Guid id, Guid assignmentId, Guid studentId, int attemptNo, EStatus status,
        string payloadUrlOrText)
    {
        Id = id;
        AssignmentId = assignmentId;
        StudentId = studentId;
        AttemptNo = attemptNo;
        Status = status;
        PayloadUrlOrText = payloadUrlOrText;
    }

    public void Update(int attemptNo, EStatus status, string payloadUrlOrText)
    {
        if (string.IsNullOrEmpty(PayloadUrlOrText))
        {
            throw new ArgumentException("PayloadUrlOrText cannot be null or empty");
        }

        if (attemptNo < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(attemptNo), "Attempt Number cannot be negative");
        }

        if (attemptNo < AttemptNo)
        {
            throw new ArgumentException("The updated attempt number cannot be less than the actual attempt number",
                nameof(attemptNo));
        }

        AttemptNo = attemptNo;
        Status = status;
        PayloadUrlOrText = payloadUrlOrText;
    }
}