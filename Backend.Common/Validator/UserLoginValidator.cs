using Backend.Common.Models.Users;
using FluentValidation;

namespace Backend.Common.Validator;

public class UserLoginValidator: AbstractValidator<UserLogin>
{
    public UserLoginValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Please enter a Username");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password cannot be empty")
            .MinimumLength(6).WithMessage("Enter at least 6 Characters")
            .Matches(@"[0-9]+").WithMessage("The Password must contain a number")
            .Matches(@"[a-z]+").WithMessage("The password must contain a lowercase letter")
            .Matches(@"[A-Z]+").WithMessage("The password must contain an uppercase letter")
            .Matches(@"[\W]+").WithMessage("The password must contain a spezial character")
            .Matches(@"[^\s]*").WithMessage("The password cannot contain whitespaces");
    }
}