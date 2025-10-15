using FluentValidation;
using CRM.Application.DTOs;

namespace CRM.Application.Validators
{
    public class UpdateLeadDTOValidator : AbstractValidator<UpdateLeadDTO>
    {
        public UpdateLeadDTOValidator()
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

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("El estado no es válido");

            RuleFor(x => x.Rating)
                .IsInEnum().WithMessage("El rating no es válido");
        }
    }
}