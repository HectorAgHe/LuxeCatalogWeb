using FluentValidation;
using LuxeCatalog.Business.DTOs.Seasons;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.Validators
{
    public class SeasonRequestValidator : AbstractValidator<SeasonRequest>
    {
        public SeasonRequestValidator()
        {
            RuleFor(x => x.Label)
                .NotEmpty().WithMessage("El nombre de la temporada es requerida")
                .MinimumLength(3).WithMessage("El nombre debe tener almenos 3 caracteres")
                .MaximumLength(150).WithMessage("El nombre no puede superar los 150 caracteres");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("La descripción no puede tener mas de 150 caracteres")
                .When(x => x.Description is not null);
        }
    }
}
