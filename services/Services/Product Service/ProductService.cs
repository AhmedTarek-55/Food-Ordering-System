using AutoMapper;
using Core.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Specifications.Products;
using Services.Helper;
using Services.Interfaces;
using Services.Services.Product_Service.DTO;

namespace Services.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
            => await _unitOfWork.Repository<ProductBrand>().GetAllAsync();

        public async Task<ProductResultDTO> GetProductByIdAsync(int? id)
        {
            var specs = new ProductsWithTypesAndBrandsSpecifications(id);
            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpecificationsAsync(specs);
            var mappedProduct = _mapper.Map<ProductResultDTO>(product);
            return mappedProduct;
        }

        public async Task<Pagination<ProductResultDTO>> GetProductsAsync(ProductSpecification specification)
        {
            var specs = new ProductsWithTypesAndBrandsSpecifications(specification);

            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecificationsAsync(specs);

            var countSpecs = new ProductsWithFiltersForCountSpecifications(specification);

            var totalCount = await _unitOfWork.Repository<Product>().CountAsync(countSpecs);

            var mappedProducts = _mapper.Map<IReadOnlyList<ProductResultDTO>>(products);

            return new Pagination<ProductResultDTO>(specification.PageIndex, specification.PageSize, totalCount, mappedProducts);
        }

        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
            => await _unitOfWork.Repository<ProductType>().GetAllAsync();
    }
}
