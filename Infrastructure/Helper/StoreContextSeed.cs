using Core.Context;
using Core.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Helper
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (context.ProductBrands is not null && !context.ProductBrands.Any())
                {
                    var brandsData = File.ReadAllText("../Infrastructure/Seed Data/brands.json");
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                    if (brands != null)
                    {
                        foreach (var brand in brands)
                            await context.ProductBrands.AddAsync(brand);

                        await context.SaveChangesAsync();
                    }
                }

                if (context.ProductTypes is not null && !context.ProductTypes.Any())
                {
                    var typesData = File.ReadAllText("../Infrastructure/Seed Data/types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                    if (types != null)
                    {
                        foreach (var type in types)
                            await context.ProductTypes.AddAsync(type);

                        await context.SaveChangesAsync();
                    }
                }

                if (context.Products is not null && !context.Products.Any())
                {
                    var productsData = File.ReadAllText("../Infrastructure/Seed Data/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                    if (products != null)
                    {
                        foreach (var product in products)
                            await context.Products.AddAsync(product);

                        await context.SaveChangesAsync();

                    }
                }

                if (context.DeliveryMethods is not null && !context.DeliveryMethods.Any())
                {
                    var deliveryMethodsData = File.ReadAllText("../Infrastructure/Seed Data/delivery.json");
                    var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);

                    if (deliveryMethods != null)
                    {
                        foreach (var item in deliveryMethods)
                            await context.DeliveryMethods.AddAsync(item);

                        await context.SaveChangesAsync();
                    }
                }
            }
        
            
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}