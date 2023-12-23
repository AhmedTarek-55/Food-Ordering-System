using AdminPanel.Models.DeliveryMethodViewModels;
using AutoMapper;
using Core.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
    public class DeliveryMethodController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeliveryMethodController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var deliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

            var mappedDeliveryMethods = _mapper.Map<IReadOnlyList<DeliveryMethodViewModel>>(deliveryMethods);

            return View(mappedDeliveryMethods.OrderBy(d => d.Id).ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DeliveryMethodViewModel model)
        {
            if (ModelState.IsValid)
            {

                var deliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

                if (deliveryMethods.Any(method => string.Equals(method.ShortName.Trim(), model.ShortName.Trim(), StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError(nameof(model.ShortName), "Delivery Method is already exist");
                    return View(model);
                }

                try
                {
                    var mappedDeliveryMethod = _mapper.Map<DeliveryMethod>(model);

                    await _unitOfWork.Repository<DeliveryMethod>().Add(mappedDeliveryMethod);

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
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(id);

            _unitOfWork.Repository<DeliveryMethod>().Delete(deliveryMethod);

            await _unitOfWork.Complete();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(id);

            var mappedDeliveryMethod = _mapper.Map<DeliveryMethodViewModel>(deliveryMethod);

            return View(mappedDeliveryMethod);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, DeliveryMethodViewModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var mappedDeliveryMethod = _mapper.Map<DeliveryMethod>(model);

                    _unitOfWork.Repository<DeliveryMethod>().Update(mappedDeliveryMethod);

                    await _unitOfWork.Complete();

                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError(nameof(model.ShortName), "Error Occurred While Updating The Delivery Method.");
                    return View(model);
                }
            }
            return View(model);
        }
    }
}
