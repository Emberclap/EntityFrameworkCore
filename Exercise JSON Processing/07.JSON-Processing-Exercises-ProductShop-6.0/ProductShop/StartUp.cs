using Castle.DynamicProxy.Generators.Emitters;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProductShop.Data;
using ProductShop.Models;
using System.Linq;
using System.Text.Json;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            var context = new ProductShopContext();


            //var inputUsers = File.ReadAllText("../../../Datasets/users.json");
            //Console.WriteLine(ImportUsers(context, inputUsers));

            //var inputProducts = File.ReadAllText("../../../Datasets/products.json");
            //Console.WriteLine(ImportProducts(context, inputProducts));

            //var inputCategories = File.ReadAllText("../../../Datasets/categories.json");
            //Console.WriteLine(ImportCategories(context, inputCategories));

            //var inputCategoryProducts = File.ReadAllText("../../../Datasets/categories-products.json");
            //Console.WriteLine(ImportCategoryProducts(context, inputCategoryProducts));


            //Console.WriteLine(GetProductsInRange(context));

            //Console.WriteLine(GetSoldProducts(context));

            //Console.WriteLine(GetCategoriesByProductsCount(context));

            Console.WriteLine(GetUsersWithProducts(context));


        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<List<User>>(inputJson);

            
            context.Users.AddRange(users);
            context.SaveChanges();
            
            return $"Successfully imported {users.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
      {
            var products = JsonConvert.DeserializeObject<List<Product>>(inputJson);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
      }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<List<Category>>(inputJson);
            categories.RemoveAll(c => c.Name == null);

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoryProducts = JsonConvert.DeserializeObject<List<CategoryProduct>>(inputJson);

            context.CategoriesProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(p => new
                {
                    name = p.Name,
                    price = p.Price,
                    seller =  $"{p.Seller.FirstName} {p.Seller.LastName}"
                })
                .OrderBy(p =>  p.price)
                .ToList();

            return JsonConvert.SerializeObject(products, Formatting.Indented);
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold
                    .Any(p => p.Buyer.Id != null))
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldProducts = u.ProductsSold
                        .Select(p => new
                        {
                            name = p.Name,
                            price = p.Price,
                            buyerFirstName = p.Buyer.FirstName,
                            buyerLastName = p.Buyer.LastName,
                        })

                })
                .OrderBy(u => u.lastName)
                .ThenBy(u => u.firstName)
                .ToList();

            return JsonConvert.SerializeObject(users, Formatting.Indented);
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoriesProducts.Count,
                    averagePrice = c.CategoriesProducts.Select(p => p.Product.Price).Average().ToString("0.00"),
                    totalRevenue = c.CategoriesProducts.Select(p => p.Product.Price).Sum().ToString("0.00")
                })
                .OrderByDescending(c => c.productsCount)
                .ToList();

            

            return JsonConvert.SerializeObject(categories, Formatting.Indented);
        }

        public static string GetUsersWithProducts(ProductShopContext context) 
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer.Id != null))
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = u.ProductsSold
                        .Where(p => p.Buyer.Id != null && p.Price != null)
                        .Select(p => new
                        {
                            name = p.Name,
                            price = p.Price,
                        })
                })
                .OrderByDescending(p => p.soldProducts.Count())
                .ToList();

            var output = new
            {
                usersCount = users.Count,
                users = users.Select(u => new
                {
                    u.firstName,
                    u.lastName,
                    u.age,
                    soldProducts = new
                    {
                        count = u.soldProducts.Count(),
                        products = u.soldProducts
                    }
                })
            };

            return JsonConvert.SerializeObject(output, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}