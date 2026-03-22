using FluentValidation;
using LuxeCatalog.Business.DTOs.Users;

namespace LuxeCatalog.Business.Validators;

public class UserRequestValidator : AbstractValidator<UserRequest>
{
    public UserRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("El nombre es requerido.")
            .MaximumLength(100).WithMessage("El nombre no puede superar 100 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo es requerido.")
            .EmailAddress().WithMessage("El formato del correo no es válido.")
            .MaximumLength(200).WithMessage("El correo no puede superar 200 caracteres.");

        RuleFor(x => x.Password)
            .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.Password));

        RuleFor(x => x.Phone1)
            .MaximumLength(20).WithMessage("El teléfono no puede superar 20 caracteres.")
            .When(x => x.Phone1 is not null);
    }
}

public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("El nombre es requerido.")
            .MaximumLength(100).WithMessage("El nombre no puede superar 100 caracteres.");

        RuleFor(x => x.Password)
            .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.Password));

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("La descripción no puede superar 500 caracteres.")
            .When(x => x.Description is not null);
    }
}

public class AddressRequestValidator : AbstractValidator<AddressRequest>
{
    public AddressRequestValidator()
    {
        RuleFor(x => x.Label)
            .NotEmpty().WithMessage("La etiqueta es requerida.")
            .MaximumLength(100).WithMessage("La etiqueta no puede superar 100 caracteres.");

        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("La calle es requerida.")
            .MaximumLength(200).WithMessage("La calle no puede superar 200 caracteres.");

        RuleFor(x => x.ExtNumber)
            .NotEmpty().WithMessage("El número exterior es requerido.");

        RuleFor(x => x.Neighborhood)
            .NotEmpty().WithMessage("La colonia es requerida.");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("La ciudad es requerida.");

        RuleFor(x => x.State)
            .NotEmpty().WithMessage("El estado es requerido.");

        RuleFor(x => x.ZipCode)
            .NotEmpty().WithMessage("El código postal es requerido.")
            .Matches(@"^\d{5}$").WithMessage("El código postal debe tener 5 dígitos.");
    }
}