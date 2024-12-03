using FluentValidation;

namespace UserComponent;

public class RegisterRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }

}

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}