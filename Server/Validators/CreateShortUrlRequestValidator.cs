using FluentValidation;

namespace Server.Validators
{
    public class CreateShortUrlRequestValidator : AbstractValidator<CreateShortUrlRequest>
    {
        public CreateShortUrlRequestValidator()
        {
            RuleFor(x => x.Url).NotEmpty().MinimumLength(5).IsUrl().WithMessage("Url is required");
        }

    }
}

internal static class UrlValidatorExtensions
{
    public static IRuleBuilderOptions<T, string> IsUrl<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("Url is not valid");
    }
}