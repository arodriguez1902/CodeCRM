using FluentValidation;
using CRM.Application.DTOs;

namespace CRM.Application.Validators
{
    public class CreateContactDTOValidator : AbstractValidator<CreateContactDTO>
    {
        public CreateContactDTOValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("El nombre es requerido")
                .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("El apellido es requerido")
                .MaximumLength(100).WithMessage("El apellido no puede exceder 100 caracteres");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("El formato del email no es válido")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Position)
                .MaximumLength(100).WithMessage("El cargo no puede exceder 100 caracteres");

            RuleFor(x => x.Department)
                .MaximumLength(100).WithMessage("El departamento no puede exceder 100 caracteres");
        }
    }
}