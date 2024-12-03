﻿using FluentValidation;

namespace UserComponent;

public class LoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }

}

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}