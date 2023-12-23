using AdminPanel.Models.ProductBrandViewModels;
using AutoMapper;
using Core.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
    public class ProductBrandController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public ProductBrandController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IActionResult> Index()
		{
			var brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();

			var mappedBrands = _mapper.Map<IReadOnlyList<ProductBrandViewModel>>(brands);

			return View(mappedBrands.OrderBy(b => b.Id).ToList());
		}

		[HttpPost]
		public async Task<IActionResult> Create(ProductBrandViewModel model)
		{
			if (ModelState.IsValid)
			{

				var brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();

				if (brands.Any(brand => string.Equals(brand.Name.Trim(), model.Name.Trim(), StringComparison.OrdinalIgnoreCase)))
				{
					ModelState.AddModelError("Name", "Brand is already exist");
					var mappedBrands = _mapper.Map<IReadOnlyList<ProductBrandViewModel>>(brands);
					return View(nameof(Index), mappedBrands.OrderBy(b => b.Id).ToList());
				}

				try
				{
					var mappedBrand = _mapper.Map<ProductBrand>(model);

					await _unitOfWork.Repository<ProductBrand>().Add(mappedBrand);

					await _unitOfWork.Complete();

					return RedirectToAction("Index");
				}
				catch (Exception)
				{
					return RedirectToAction("Index");
				}
			}
			return View(model);
		}

		public async Task<IActionResult> Delete(int id)
		{
			var result = (await _unitOfWork.Repository<Product>().GetAllAsync()).Any(product  => product.ProductBrandId == id);

			var brand = await _unitOfWork.Repository<ProductBrand>().GetByIdAsync(id);

			if (result)
			{
				TempData["Message"] = $"The brand {brand.Name} can not be deleted, because it is refrenced by some products.";
				return RedirectToAction(nameof(Index));
			}

			_unitOfWork.Repository<ProductBrand>().Delete(brand);

			await _unitOfWork.Complete();

			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Edit(int id)
		{
			var brand = await _unitOfWork.Repository<ProductBrand>().GetByIdAsync(id);

			var mappedBrand = _mapper.Map<ProductBrandViewModel>(brand);

			return View(mappedBrand);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, ProductBrandViewModel model)
		{
			if (id != model.Id)
				return NotFound();

			if (ModelState.IsValid)
			{
				try
				{
					var mappedBrand = _mapper.Map<ProductBrand>(model);

					_unitOfWork.Repository<ProductBrand>().Update(mappedBrand);

					await _unitOfWork.Complete();

					return RedirectToAction("Index");
				}
				catch (Exception)
				{
					ModelState.AddModelError(nameof(model.Name), "Error Occurred While Updating The Brand.");
					return View(model);
				}
			}
			return View(model);
		}
	}
}
