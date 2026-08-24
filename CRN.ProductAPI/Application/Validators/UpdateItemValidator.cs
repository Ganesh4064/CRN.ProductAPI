using CRN.ProductAPI.Application.DTOs;
using FluentValidation;

namespace CRN.ProductAPI.Application.Validators;

public class UpdateItemValidator : AbstractValidator<UpdateItemDto>
{
    public UpdateItemValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0.");
    }
}