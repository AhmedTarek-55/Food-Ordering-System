using Core.Entities;
using System;

namespace Infrastructure.Specifications.Products
{
    public class ProductsWithTypesAndBrandsSpecifications : BaseSpecifications<Product>
    {
        public ProductsWithTypesAndBrandsSpecifications(ProductSpecification productSpecification)
            : base(product =>
                (string.IsNullOrEmpty(productSpecification.Search) || product.Name.Trim().ToLower().Contains(productSpecification.Search)) &&
                (!productSpecification.BrandId.HasValue || product.ProductBrandId == productSpecification.BrandId) &&
                (!productSpecification.TypeId.HasValue || product.ProductTypeId == productSpecification.TypeId)
            )
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
            AddOrderBy(p => p.Name);
            ApplyPagination(productSpecification.PageSize * (productSpecification.PageIndex - 1), productSpecification.PageSize);

            if (!string.IsNullOrEmpty(productSpecification.Sort))
            {
                switch(productSpecification.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;
					case "IdAsc":
						AddOrderBy(p => p.Id);
						break;
					default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
        }

        public ProductsWithTypesAndBrandsSpecifications(int? id) : base(x => x.Id == id)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }

        public ProductsWithTypesAndBrandsSpecifications() : base( product => product.ProductTypeId != 0 && product.ProductBrandId != 0)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }
    }
}
