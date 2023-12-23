using AdminPanel.Helper;
using AdminPanel.Models.ProductViewModels;
using AutoMapper;
using Core.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Specifications.Products;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Helper;

namespace AdminPanel.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
    public class ProductController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IActionResult> Index(int? brandId, int? typeId, string? SearchValue, int pageIndex)
		{
			ProductSpecification specification = new ProductSpecification
			{
				BrandId = brandId,
				TypeId = typeId,
				Search = SearchValue,
				PageIndex = pageIndex > 0 ? pageIndex : 1,
				Sort = "IdAsc"
			};

			var paginatedProducts = await ProductsHelper.GetPaginatedProductsAsync(specification, _unitOfWork, _mapper);

			if (paginatedProducts == null)
				return View(new Pagination<ProductViewModel>(0, 0, 1, new List<ProductViewModel>()));

			return View(paginatedProducts);
		}

		public IActionResult Filter()
		{
			return View();
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(ProductViewModel model)
		{
			if (ModelState.IsValid)
			{
				if (model.Image != null)
					model.PictureUrl = PictureSettings.UploadFile(model.Image, "products");

				else
				{
					model.Image = ImageReader.ReadImage("images\\products\\ProductDefaultImage.png");
					model.PictureUrl = PictureSettings.UploadFile(model.Image, "products");
				}

				var mappedProduct = _mapper.Map<Product>(model);

				await _unitOfWork.Repository<Product>().Add(mappedProduct);

				await _unitOfWork.Complete();

				return RedirectToAction(nameof(Index));
			}
			return View(model);
		}

		public async Task<IActionResult> Edit(int id)
		{
			var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);

			var mappedProduct = _mapper.Map<ProductViewModel>(product);

			return View(mappedProduct);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, ProductViewModel model)
		{
			if (id != model.Id)
				return NotFound();

			if (ModelState.IsValid)
			{
				if (model.Image != null)
				{
					PictureSettings.DeleteFile(model.PictureUrl);
					model.PictureUrl = PictureSettings.UploadFile(model.Image, "products");
				}

				var mappedProduct = _mapper.Map<Product>(model);

				_unitOfWork.Repository<Product>().Update(mappedProduct);

				var result = await _unitOfWork.Complete();

				if (result > 0)
					return RedirectToAction("Index");

				return View(model);
			}
			return View(model);
		}

		public async Task<IActionResult> Delete(int id)
		{
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);

            var mappedProduct = _mapper.Map<ProductViewModel>(product);

			return View(mappedProduct);
		}

		[HttpPost]
		public async Task<IActionResult> Delete(int id, ProductViewModel model)
		{
			if (id != model.Id)
				return NotFound();

			if (ModelState.IsValid)
			{
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);

                if (product.PictureUrl != null)
					PictureSettings.DeleteFile(model.PictureUrl);

				_unitOfWork.Repository<Product>().Delete(product);

				var result = await _unitOfWork.Complete();

				if (result > 0)
					return RedirectToAction("Index");

				return View(model);
			}
			return View(model);
		}
	}
}
