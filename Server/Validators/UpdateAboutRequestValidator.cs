using FluentValidation;
using Server.Controllers;

namespace Server.Validators;

public class UpdateAboutRequestValidator : AbstractValidator<AboutController.UpdateAboutRequest>
{
    public UpdateAboutRequestValidator()
    {
        RuleFor(x => x.Text).NotEmpty().WithMessage("Text is required");
    }

}