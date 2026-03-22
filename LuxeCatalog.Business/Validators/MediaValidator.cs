using FluentValidation;
using LuxeCatalog.Business.DTOs.Media;

namespace LuxeCatalog.Business.Validators;

public class HeroImageRequestValidator : AbstractValidator<HeroImageRequest>
{
    public HeroImageRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("El título es requerido.")
            .MaximumLength(200).WithMessage("El título no puede superar 200 caracteres.");

        RuleFor(x => x.ImageUrl)
            .NotEmpty().WithMessage("La URL de la imagen es requerida.")
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("La URL de la imagen no es válida.");
    }
}

public class BannerImageRequestValidator : AbstractValidator<BannerImageRequest>
{
    public BannerImageRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("El título es requerido.")
            .MaximumLength(200).WithMessage("El título no puede superar 200 caracteres.");

        RuleFor(x => x.ImageUrl)
            .NotEmpty().WithMessage("La URL de la imagen es requerida.")
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("La URL de la imagen no es válida.");
    }
}

public class VideoRequestValidator : AbstractValidator<VideoRequest>
{
    public VideoRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("El título es requerido.")
            .MaximumLength(200).WithMessage("El título no puede superar 200 caracteres.");

        RuleFor(x => x.YoutubeId)
            .NotEmpty().WithMessage("El ID de YouTube es requerido.")
            .MaximumLength(50).WithMessage("El ID de YouTube no puede superar 50 caracteres.")
            .Matches(@"^[a-zA-Z0-9_-]{11}$")
            .WithMessage("El ID de YouTube debe tener exactamente 11 caracteres.");
    }
}