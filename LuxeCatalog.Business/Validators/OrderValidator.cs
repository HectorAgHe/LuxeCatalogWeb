using FluentValidation;
using LuxeCatalog.Business.DTOs.Orders;

namespace LuxeCatalog.Business.Validators;

public class OrderRequestValidator : AbstractValidator<OrderRequest>
{
    public OrderRequestValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("Usuario inválido.");

        RuleFor(x => x.PaymentMethod)
            .NotEmpty().WithMessage("El método de pago es requerido.")
            .Must(x => new[] { "Transferencia", "Depósito", "Contra Entrega" }.Contains(x))
            .WithMessage("Método de pago inválido. Opciones: Transferencia, Depósito, Contra Entrega.");

        RuleFor(x => x.AddressSnapshot)
            .NotEmpty().WithMessage("La dirección de envío es requerida.")
            .Must(x => x != "{}").WithMessage("Debes seleccionar una dirección de envío.");

        RuleFor(x => x.Products)
            .NotEmpty().WithMessage("El pedido debe tener al menos un producto.");

        RuleForEach(x => x.Products).SetValidator(new OrderProductRequestValidator());
    }
}

public class OrderProductRequestValidator : AbstractValidator<OrderProductRequest>
{
    public OrderProductRequestValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0.");
    }
}

public class UpdateOrderStatusRequestValidator : AbstractValidator<UpdateOrderStatusRequest>
{
    public UpdateOrderStatusRequestValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("El estado es requerido.")
            .Must(x => new[] { "Pendiente", "Adquirido", "SinExistencia", "Entregado" }.Contains(x))
            .WithMessage("Estado inválido.");
    }
}