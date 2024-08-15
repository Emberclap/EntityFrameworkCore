using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            var context = new ProductShopContext();

            //var inputUsers = File.ReadAllText("../../../Datasets/users.xml");
            //Console.WriteLine(ImportUsers(context, inputUsers));

            //var productsInput = File.ReadAllText("../../../Datasets/products.xml");
            //Console.WriteLine(ImportProducts(context, productsInput));

            //var categoriesInput = File.ReadAllText("../../../Datasets/categories.xml");
            //Console.WriteLine(ImportCategories(context, categoriesInput));

            //var categoryProductsInput = File.ReadAllText("../../../Datasets/categories-products.xml");
            //Console.WriteLine(ImportCategoryProducts(context, categoryProductsInput));

            Console.WriteLine(GetUsersWithProducts(context));
        }


        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UserImportDTo[]),
                new XmlRootAttribute ("Users"));

            using var reader = new StringReader(inputXml);

            var usersImportDTos = (UserImportDTo[])serializer.Deserialize(reader);

            var users = usersImportDTos
                .Select(dto => new User()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Age = dto.Age
                }).ToList();

            context.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}"; ;
        }
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ProductImportDto[]),
              new XmlRootAttribute("Products"));

            using var reader = new StringReader(inputXml);

            var productsImportDTos = (ProductImportDto[])serializer.Deserialize(reader);

            var products = productsImportDTos
                .Select(dto => new Product
                {
                    Name = dto.Name,
                    Price = dto.Price,
                    SellerId = dto.SellerId,
                    BuyerId = dto.BuyerId,
                }).ToList();

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}"; ;
        }
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CategoriesImportDto[]),
               new XmlRootAttribute("Categories"));

            using var reader = new StringReader(inputXml);

            var categoriesImportDTos = (CategoriesImportDto[])serializer.Deserialize(reader);

            var categories = categoriesImportDTos
                .Where(c => c.Name != null)
                .Select(dto => new Category()
                {
                    Name = dto.Name,
                }).ToList();

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}"; ;
        }
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CategoryProductsImportDto[]),
            new XmlRootAttribute("CategoryProducts"));

            using var reader = new StringReader(inputXml);

            var categoriesProductsImportDTos = (CategoryProductsImportDto[])serializer.Deserialize(reader);

            var categories = context.Categories
                .Select(c => c.Id).ToList();

            var procucts = context.Products
                .Select(p => p.Id).ToList();

            var categoriesProducts = categoriesProductsImportDTos
                .Where(c => procucts.Contains(c.ProductId) && categories.Contains(c.CategoryId))
                .Select(dto => new CategoryProduct()
                {
                    CategoryId = dto.CategoryId,
                    ProductId = dto.ProductId,
                }).ToList();

            context.CategoryProducts.AddRange(categoriesProducts);
            context.SaveChanges();

            return $"Successfully imported {categoriesProducts.Count}"; ;
        }
        public static string GetProductsInRange(ProductShopContext context)
        {
            var productsInRange = context.Products
                .Where(c => c.Price >= 500 && c.Price <= 1000)
                .Select(c => new ProductsInRangeExportDto()
                {
                    Name = c.Name,
                    Price = c.Price,
                    BuyerName = $"{c.Buyer.FirstName} {c.Buyer.LastName}"
                })
                .OrderBy(c => c.Price)
                .Take(10)
                .ToList();

            return SerializeToXml(productsInRange, "Products");
        }
        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
              .Where(u => u.ProductsSold.Any())
              .Select(c => new GetSoldProductsDto()
              {
                  FirstName = c.FirstName,
                  LastName = c.LastName,
                  SoldProducts = c.ProductsSold
                    .Select(p => new ProductsDto()
                    {
                        Name = p.Name,
                        Price = p.Price,
                    }).ToArray()
              })
              .OrderBy(u => u.LastName)
              .ThenBy(u => u.FirstName)
              .Take(5)
              .ToList();

            return SerializeToXml(users, "Users");
        }
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {

            var productsInRange = context.Categories
              .Select(c => new GetCategoriesByProductsCountDto()
              {
                  Name= c.Name,
                  Count = c.CategoryProducts.Count(),
                  AveragePrice = c.CategoryProducts.Average(p => p.Product.Price),
                  TotalRevenue = c.CategoryProducts.Sum(p => p.Product.Price),
              })
              .OrderByDescending(c => c.Count)
              .ThenBy(c => c.TotalRevenue)
              .ToList();

            return SerializeToXml(productsInRange, "Categories");
        }
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var usersProduct = context.Users
                            .ToList() 
                            .Where(u => u.ProductsSold.Any())
                            .OrderByDescending(p => p.ProductsSold.Count)
                            .Select(u => new ExportUserDto()
                            {
                                FirstName = u.FirstName,
                                LastName = u.LastName,
                                Age = u.Age,
                                SoldProducts = new ProductsWithCountDTO()
                                {
                                    Count = u.ProductsSold.Count,
                                    Products = u.ProductsSold
                                               .Select(x => new ProductToDTO()
                                               {
                                                   Name = x.Name,
                                                   Price = x.Price
                                               })
                                               .OrderByDescending(y => y.Price)
                                               .ToList()
                                }
                            })
                            .Take(10)
                            .ToList();

            var userProductWithCount = new UserProductWithCount()
            {
                Count = context.Users.Count(u => u.ProductsSold.Any()),
                Users = usersProduct
            };

            return SerializeToXml(userProductWithCount, "Users");
        }
        private static string SerializeToXml<T>(T dto, string xmlRootAttribute, bool omitDeclaration = false)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), new XmlRootAttribute(xmlRootAttribute));
            StringBuilder stringBuilder = new StringBuilder();

            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = omitDeclaration,
                Encoding = new UTF8Encoding(false),
                Indent = true
            };

            using (StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture))
            using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
            {
                XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
                xmlSerializerNamespaces.Add(string.Empty, string.Empty);

                try
                {
                    xmlSerializer.Serialize(xmlWriter, dto, xmlSerializerNamespaces);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return stringBuilder.ToString();
        }
    }
}