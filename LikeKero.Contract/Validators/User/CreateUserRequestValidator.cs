using LikeKero.Contract.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Contract.Validators
{
   public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            //RuleFor(expression: x => x.Name).NotEmpty().Matches(expression: "^[a-zA-Z0-9 ]*$");
            RuleFor(expression: x => x.FirstName).NotEmpty().Matches(expression: "^[a-zA-Z ']*$");
            RuleFor(expression: x => x.LastName).NotEmpty().Matches(expression: "^[a-zA-Z ']*$");
            RuleFor(expression: x => x.EmailAddress).NotEmpty();//.EmailAddress();
            RuleFor(expression: x => x.LoginId).NotEmpty();
            RuleFor(expression: x => x.RequesterUserId).NotEmpty();
            RuleFor(expression: x => x.JobCodeId).NotEmpty();
            RuleFor(expression: x => x.GroupId).NotEmpty();
            RuleFor(m => m.HiringDate)
             .NotEmpty()
             .LessThanOrEqualTo(DateTime.Now.Date)
             .When(m => m.HiringDate.HasValue);
            RuleFor(m => m.RoleChangeDate)
            .NotEmpty()
            .LessThanOrEqualTo(DateTime.Now.Date)
            .When(m => m.RoleChangeDate.HasValue);

        }
    }
}
