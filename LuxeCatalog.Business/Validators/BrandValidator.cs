using FluentValidation;
using LuxeCatalog.Business.DTOs.Brands;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.Validators
{
    public class BrandRequestValidator : AbstractValidator<BrandRequest>
    {
        public BrandRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre de la marca es requerida")
                .MaximumLength(200).WithMessage("El nombre de la marca no puede superar los 200 caracteres");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("La descrpcion no puede superar los 500 caracteres")
                .When(x => x.Description is not null);
        }
    }
}
