
using FluentValidation;
using LuxeCatalog.Business.DTOs.Auth;

namespace LuxeCatalog.Business.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo es requerido")
                .EmailAddress().WithMessage("El formato del correo no es valido");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es requerida")
                .MinimumLength(6).WithMessage("La contraseña debe tener almenos 6 caracteres");
        }

    }
}
