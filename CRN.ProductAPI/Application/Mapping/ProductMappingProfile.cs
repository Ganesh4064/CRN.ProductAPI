using AutoMapper;
using CRN.ProductAPI.Application.DTOs;
using CRN.ProductAPI.Domain.Entities;

namespace CRN.ProductAPI.Application.Mapping;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        // Product
        CreateMap<Product, ProductDto>();
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();

        // Item
        CreateMap<Item, ItemDto>();
        CreateMap<CreateItemDto, Item>();
        CreateMap<UpdateItemDto, Item>();
    }
}