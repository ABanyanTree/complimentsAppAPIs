using LikeKero.Contract.Requests.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Contract.Validators.User
{
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            //.Matches(expression: "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$")
            RuleFor(expression: x => x.Password).NotEmpty();
            RuleFor(expression: x => x.UserId).NotEmpty();
            RuleFor(expression: x => x.RequesterUserId).NotEmpty();
        }
    }
}
