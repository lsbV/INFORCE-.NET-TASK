using FluentValidation;

namespace Server.Validators;

public class PaginationInfoValidator : AbstractValidator<PaginationInfo>
{
    public PaginationInfoValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1).WithMessage("Page must be greater than or equal to 1");
        RuleFor(x => x.Count).GreaterThanOrEqualTo(1).LessThanOrEqualTo(100).WithMessage("Count must be between 1 and 100");
    }

}