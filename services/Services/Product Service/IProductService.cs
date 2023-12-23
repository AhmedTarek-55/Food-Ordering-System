using Core.Entities;
using Infrastructure.Specifications.Products;
using Services.Helper;
using Services.Services.Product_Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductResultDTO> GetProductByIdAsync(int? id);
        Task<Pagination<ProductResultDTO>> GetProductsAsync(ProductSpecification specification);
        Task<IReadOnlyList<ProductType>> GetProductTypesAsync();
        Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync();
    }
}
