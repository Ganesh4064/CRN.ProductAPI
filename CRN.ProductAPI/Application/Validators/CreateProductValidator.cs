using CRN.ProductAPI.Application.DTOs;
using FluentValidation;

namespace CRN.ProductAPI.Application.Validators;

public class CreateProductValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.ProductName)
            .NotEmpty()
            .WithMessage("Product name is required.")
            .MaximumLength(100);

        RuleFor(x => x.CreatedBy)
            .NotEmpty()
            .WithMessage("Created by is required.")
            .MaximumLength(100);
    }
}