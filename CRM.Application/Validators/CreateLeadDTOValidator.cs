using FluentValidation;
using CRM.Application.DTOs;

namespace CRM.Application.Validators
{
    public class CreateLeadDTOValidator : AbstractValidator<CreateLeadDTO>
    {
        public CreateLeadDTOValidator()
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

            RuleFor(x => x.Company)
                .MaximumLength(255).WithMessage("La empresa no puede exceder 255 caracteres");
        }
    }
}