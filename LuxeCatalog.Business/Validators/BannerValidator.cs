using FluentValidation;
using LuxeCatalog.Business.DTOs.Banners;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.Validators
{
    public class BannerRequestValidator : AbstractValidator<BannerRequest>
    {
        public BannerRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("El titulo del banner es requerido.")
                .MaximumLength(200).WithMessage("El titulo no puede superar los 200 caracteres");

            RuleFor(x => x.SeasonId)
                .GreaterThan(0).WithMessage("Debes selecconar una temporada");

            RuleFor(x => x.Subtitle)
                .MaximumLength(300).WithMessage("El subtitulo no puede superar los 300 caracteres")
                .When(x => x.Subtitle is not null);
        }

    }
}
