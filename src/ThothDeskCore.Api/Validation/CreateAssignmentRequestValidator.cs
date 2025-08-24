using FluentValidation;
using ThothDeskCore.Api.DTOs;

namespace ThothDeskCore.Api.Validation;

public class CreateAssignmentRequestValidator : AbstractValidator<CreateAssignmentRequest>
{
    public CreateAssignmentRequestValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Description).MaximumLength(2048);
        RuleFor(x => x.MaxPoints).InclusiveBetween(1, 100);
        RuleFor(x => x.DueAt).GreaterThan(DateTimeOffset.Now.AddMinutes(-1));
    }

    //todo create validator for update as well
}

