using API.Handle_Responses;
using API.Helper;
using Core.Entities;
using Infrastructure.Specifications.Products;
using Microsoft.AspNetCore.Mvc;
using Services.Helper;
using Services.Interfaces;
using Services.Services.Product_Service.DTO;

namespace API.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Cache(20)]
        public async Task<ActionResult<Pagination<ProductResultDTO>>> GetProducts([FromQuery] ProductSpecification specification)
        {
            var products = await _productService.GetProductsAsync(specification);

            return Ok(products);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [Cache(20)]
        public async Task<ActionResult<ProductResultDTO>> GetProductById(int? id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound(new ApiResponse(404));

            return Ok(product);
        }

        [HttpGet]
        [Route("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
            => Ok(await _productService.GetProductBrandsAsync());

        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
            => Ok(await _productService.GetProductTypesAsync());
    }
}
