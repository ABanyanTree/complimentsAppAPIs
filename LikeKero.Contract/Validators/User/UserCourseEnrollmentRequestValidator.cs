using LikeKero.Contract.Requests;
using LikeKero.Contract.Requests.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Contract.Validators
{
   public class UserCourseEnrollmentRequestValidator : AbstractValidator<UserCourseEnrollmentRequest>
    {
        public UserCourseEnrollmentRequestValidator()
        {
            RuleFor(expression: x => x.CourseId).NotEmpty();
            RuleFor(expression: x => x.RequesterUserId).NotEmpty();
        }
    }
}
