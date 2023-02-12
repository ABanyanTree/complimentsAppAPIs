using LikeKero.Contract.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Contract.Validators
{
   public class UserLoginRequestValidator : AbstractValidator<UserLoginRequest>
    {
        public UserLoginRequestValidator()
        {
            //RuleFor(expression: x => x.Name).NotEmpty().Matches(expression: "^[a-zA-Z0-9 ]*$");
            RuleFor(expression: x => x.EmailAddress).NotEmpty();
            RuleFor(expression: x => x.Password).NotEmpty();
             
        }
    }
}
