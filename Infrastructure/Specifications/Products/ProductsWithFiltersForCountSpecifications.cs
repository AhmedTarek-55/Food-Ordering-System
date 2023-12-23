using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications.Products
{
    public class ProductsWithFiltersForCountSpecifications : BaseSpecifications<Product>
    {
        public ProductsWithFiltersForCountSpecifications(ProductSpecification productSpecification)
            : base(product =>
                (string.IsNullOrEmpty(productSpecification.Search) || product.Name.Trim().ToLower().Contains(productSpecification.Search)) &&
                (!productSpecification.BrandId.HasValue || product.ProductBrandId == productSpecification.BrandId) &&
                (!productSpecification.TypeId.HasValue || product.ProductTypeId == productSpecification.TypeId)
            )
        {
        }
    }
}
