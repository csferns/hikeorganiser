using FluentValidation;

namespace HikeOrganiser.Core.Behaviours.Events.Schedule;

public class RequestValidator : AbstractValidator<Request>
{
    public RequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty();
        
        RuleFor(x => x.Description)
            .NotEmpty();

        RuleFor(x => x.StartDate)
            .GreaterThan(DateTime.Today.AddDays(-1))
            .LessThan(DateTime.MaxValue);

        When(x => x.EndDate.HasValue, () =>
        {
            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .LessThan(DateTime.MaxValue);
        });
        
        RuleFor(x => x.Location)
            .NotEmpty()
                .When(x => !x.BucketListId.HasValue);

        RuleFor(x => x.BucketListId)
            .GreaterThan(0)
                .When(x => string.IsNullOrEmpty(x.Location));
    }
}