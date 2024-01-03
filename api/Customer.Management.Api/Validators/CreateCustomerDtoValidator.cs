using Customers.Management.Api.Dtos;
using FluentValidation;

namespace Customers.Management.Api.Validators
{
    public class CreateCustomerDtoValidator : AbstractValidator<CustomerDto>
    {
        public CreateCustomerDtoValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
