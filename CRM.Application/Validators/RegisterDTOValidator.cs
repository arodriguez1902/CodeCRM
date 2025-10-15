using FluentValidation;
using CRM.Application.DTOs;

namespace CRM.Application.Validators
{
    public class RegisterDTOValidator : AbstractValidator<RegisterDTO>
    {
        public RegisterDTOValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("El nombre de usuario es requerido")
                .MinimumLength(3).WithMessage("El usuario debe tener al menos 3 caracteres")
                .MaximumLength(50).WithMessage("El usuario no puede tener más de 50 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es requerido")
                .EmailAddress().WithMessage("El formato del email no es válido");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es requerida")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres")
                .Matches("[A-Z]").WithMessage("La contraseña debe contener al menos una mayúscula")
                .Matches("[a-z]").WithMessage("La contraseña debe contener al menos una minúscula")
                .Matches("[0-9]").WithMessage("La contraseña debe contener al menos un número");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("El nombre es requerido")
                .MaximumLength(100).WithMessage("El nombre no puede tener más de 100 caracteres");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("El apellido es requerido")
                .MaximumLength(100).WithMessage("El apellido no puede tener más de 100 caracteres");
        }
    }
}