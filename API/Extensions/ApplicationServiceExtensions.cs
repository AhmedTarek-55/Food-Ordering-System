using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Services.Product_Service.DTO;
using Services.Services;
using API.Handle_Responses;
using Services.Services.Cache_Service;
using Services.Services.Basket_Service.DTO;
using Infrastructure.Basket_Repository;
using Services.Services.Basket_Service;
using Services.Services.Token_Service;
using Services.Services.User_Service;
using Services.Services.Order_Service.DTO;
using Services.Services.Order_Service;
using Services.Services.Payment_Service;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices (this IServiceCollection services) 
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrderService,OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();

            // these configurations fill the Error list in the ApiValidationErrorResponse class
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ActionContext =>
                {
                    var errors = ActionContext.ModelState
                                    .Where(model => model.Value.Errors.Count > 0)
                                    .SelectMany(model => model.Value.Errors)
                                    .Select(error => error.ErrorMessage).ToList();
                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            services.AddAutoMapper(typeof(ProductProfile));
            services.AddAutoMapper(typeof(BasketProfile));
            services.AddAutoMapper(typeof(OrderProfile));

            return services;
        }
    }
}
