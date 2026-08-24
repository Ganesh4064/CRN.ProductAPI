using CRN.ProductAPI.Application.DTOs;
using FluentValidation;

namespace CRN.ProductAPI.Application.Validators;

public class UpdateProductValidator : AbstractValidator<UpdateProductDto>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.ProductName)
            .NotEmpty()
            .WithMessage("Product name is required.")
            .MaximumLength(100)
            .WithMessage("Product name cannot exceed 100 characters.");

        RuleFor(x => x.ModifiedBy)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.ModifiedBy))
            .WithMessage("ModifiedBy cannot exceed 100 characters.");
    }
}