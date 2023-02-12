using LikeKero.Contract.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Contract.Validators
{
   public class CreateTestCustomerRequestValidator : AbstractValidator<TestCustomerRequest>
    {
        public CreateTestCustomerRequestValidator()
        {            
            RuleFor(expression: x => x.FirstName).NotEmpty().Matches(expression: "^[a-zA-Z ']*$");
            RuleFor(expression: x => x.LastName).NotEmpty().Matches(expression: "^[a-zA-Z ']*$");
            RuleFor(expression: x => x.Gender).NotEmpty();
         

        }
    }
}
