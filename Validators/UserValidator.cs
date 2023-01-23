using realworldProject.Models;
using FluentValidation;

namespace realworldProject.Validators
{
    public class UserLoginValidator: AbstractValidator<UserLoginRequest>
    {
        public UserLoginValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Password).Length(8, 24);
        }
    }

    public class UserCreateValidator : AbstractValidator<UserCreateRequest>
    {
        public UserCreateValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Password).Length(8, 24);
            RuleFor(x => x.username).NotEmpty();
        }
    }

    



}
