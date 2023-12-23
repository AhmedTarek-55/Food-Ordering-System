using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Configurations
{
	public class ProductBrandConfigurations : IEntityTypeConfiguration<ProductBrand>
	{
		void IEntityTypeConfiguration<ProductBrand>.Configure(EntityTypeBuilder<ProductBrand> builder)
		{
			builder.HasIndex(brand => brand.Name).IsUnique();
		}
	}
}
