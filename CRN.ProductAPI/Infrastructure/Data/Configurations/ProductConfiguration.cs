using CRN.ProductAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRN.ProductAPI.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> entity)
    {
        entity.ToTable("Product");

        entity.HasKey(p => p.Id);

        entity.Property(p => p.ProductName)
            .IsRequired()
            .HasMaxLength(255);

        entity.Property(p => p.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        entity.Property(p => p.CreatedOn)
            .IsRequired();

        entity.Property(p => p.ModifiedBy)
            .HasMaxLength(100);

        entity.Property(p => p.ModifiedOn);

        entity.HasIndex(p => p.ProductName);

        entity.HasMany(p => p.Items)
            .WithOne(i => i.Product)
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}