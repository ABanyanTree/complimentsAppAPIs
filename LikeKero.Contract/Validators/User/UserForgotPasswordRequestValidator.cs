using LikeKero.Contract.Requests.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Contract.Validators
{
    public class UserForgotPasswordRequestValidator : AbstractValidator<UserForgotPasswordRequest>
    {
        public UserForgotPasswordRequestValidator()
        {
            RuleFor(expression: x => x.EmailAddress).NotEmpty().WithMessage("Please enter Email Address");
        }
    }
}
