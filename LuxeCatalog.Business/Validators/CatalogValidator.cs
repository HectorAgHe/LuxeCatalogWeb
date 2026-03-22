using FluentValidation;
using LuxeCatalog.Business.DTOs.Catalogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.Validators
{
    public class CatalogRequestValidator : AbstractValidator<CatalogRequest>
    {
        public CatalogRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del catalogo es requerido")
                .MaximumLength(200).WithMessage("El nombre no puede superar 200 caracteres");

            RuleFor(x => x.Pages)
                .GreaterThan(0).WithMessage("El número de paginas debe ser mayor a 0");

            RuleFor(x => x.SeasonId)
                .GreaterThan(0).WithMessage("Debes seleccionar una temporada");

            RuleFor(x => x.Category)
                .MaximumLength(100).WithMessage("La categoria no puede superar los 100 caracteres")
                .When(x => x.Category is not null);
        }
    }
}
