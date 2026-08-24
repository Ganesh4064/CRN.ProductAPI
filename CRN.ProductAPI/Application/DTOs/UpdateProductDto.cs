namespace CRN.ProductAPI.Application.DTOs;

public class UpdateProductDto
{
    public string ProductName { get; set; } = string.Empty;

    public string? ModifiedBy { get; set; }
}