using AdminPanel.Models.ProductTypeViewModels;
using AutoMapper;
using Core.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
    public class ProductTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductTypeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var types = await _unitOfWork.Repository<ProductType>().GetAllAsync();

            var mappedTypes = _mapper.Map<IReadOnlyList<ProductTypeViewModel>>(types);

            return View(mappedTypes.OrderBy(t => t.Id).ToList());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var types = await _unitOfWork.Repository<ProductType>().GetAllAsync();

                if (types.Any(type => string.Equals(type.Name.Trim(), model.Name.Trim(), StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError("Name", "Type is already exist");

                    var mappedTypes = _mapper.Map<IReadOnlyList<ProductTypeViewModel>>(types);
                    return View(nameof(Index), mappedTypes.OrderBy(t => t.Id).ToList());
                }

                try
                {
                    var mappedType = _mapper.Map<ProductType>(model);

                    await _unitOfWork.Repository<ProductType>().Add(mappedType);

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
            var result = (await _unitOfWork.Repository<Product>().GetAllAsync()).Any(product => product.ProductTypeId == id);

            var type = await _unitOfWork.Repository<ProductType>().GetByIdAsync(id);

            if (result)
            {
                TempData["Message"] = $"The type {type.Name} can not be deleted, because it is refrenced by some products.";
                return RedirectToAction(nameof(Index));
            }

            _unitOfWork.Repository<ProductType>().Delete(type);

            await _unitOfWork.Complete();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var type = await _unitOfWork.Repository<ProductType>().GetByIdAsync(id);

            var mappedType = _mapper.Map<ProductTypeViewModel>(type);

            return View(mappedType);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductTypeViewModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var mappedType = _mapper.Map<ProductType>(model);

                    _unitOfWork.Repository<ProductType>().Update(mappedType);

                    await _unitOfWork.Complete();

                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError(nameof(model.Name), "Error Occurred While Updating The Type.");
                    return View(model);
                }
            }
            return View(model);
        }
    }
}
