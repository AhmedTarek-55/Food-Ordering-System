using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Configurations
{
	public class ProductTypeConfigurations : IEntityTypeConfiguration<ProductType>
	{
		void IEntityTypeConfiguration<ProductType>.Configure(EntityTypeBuilder<ProductType> builder)
		{
			builder.HasIndex(type => type.Name).IsUnique();
		}
	}
}
