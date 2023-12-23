using AdminPanel.Models.ProductViewModels;
using AutoMapper;
using Core.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Specifications.Products;
using Services.Helper;

namespace AdminPanel.Helper
{
	public class ProductsHelper
	{
		public static async Task<Pagination<ProductViewModel>> GetPaginatedProductsAsync(ProductSpecification specification, IUnitOfWork unitOfWork, IMapper mapper)
		{
			var countSpecs = new ProductsWithFiltersForCountSpecifications(specification);

			var totalCount = await unitOfWork.Repository<Product>().CountAsync(countSpecs);

			if (totalCount == 0)
				return null;

			int lastPage = (int)Math.Ceiling((double)totalCount / specification.PageSize);

			specification.PageIndex = specification.PageIndex <= lastPage ? specification.PageIndex : lastPage;

			var specs = new ProductsWithTypesAndBrandsSpecifications(specification);

			var products = await unitOfWork.Repository<Product>().GetAllWithSpecificationsAsync(specs);

			var mappedProducts = mapper.Map<IReadOnlyList<ProductViewModel>>(products);

			if (specification.TypeId is not null || specification.BrandId is not null || !string.IsNullOrEmpty(specification.Search))
			{
				specification.PageSize = totalCount;
				specification.PageIndex = 0;
			}

			return new Pagination<ProductViewModel>(specification.PageIndex, specification.PageSize, totalCount, mappedProducts);
		}
	}
}
