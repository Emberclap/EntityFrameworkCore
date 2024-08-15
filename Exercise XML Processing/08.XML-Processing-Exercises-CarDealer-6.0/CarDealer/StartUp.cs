using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.DTOs.Export;
using CarDealer.Models;
using Castle.Core.Resource;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Linq;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            var context = new CarDealerContext();

            //var suppliersInput = File.ReadAllText("../../../Datasets/suppliers.xml");
            //Console.WriteLine(ImportSuppliers(context, suppliersInput));

            //var partsInput = File.ReadAllText("../../../Datasets/parts.xml");
            //Console.WriteLine(ImportParts(context, partsInput));

            //var carsInput = File.ReadAllText("../../../Datasets/cars.xml");
            //Console.WriteLine(ImportCars(context, carsInput));

            //var customersInput = File.ReadAllText("../../../Datasets/customers.xml");
            //Console.WriteLine(ImportCustomers(context, customersInput));

            //var salesInput = File.ReadAllText("../../../Datasets/sales.xml");
            //Console.WriteLine(ImportSales(context, salesInput));

            Console.WriteLine(GetSalesWithAppliedDiscount(context));

        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SupplierDTo[]), 
                new XmlRootAttribute("Suppliers"));

            using var reader = new StringReader(inputXml);

            var suppliersDtos = (SupplierDTo[])serializer.Deserialize(reader);

            var suppliers = suppliersDtos
                .Select(dto => new Supplier()
                {
                    Name = dto.Name,
                    IsImporter = dto.IsImporter
                }).ToList();

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}";
        }
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PartDTo[]),
                new XmlRootAttribute("Parts"));

            using var reader = new StringReader(inputXml);

            var partsDtos = (PartDTo[])serializer.Deserialize(reader);

            var suppliersIds = context.Suppliers
                .Select(s => s.Id)
                .ToList();

            var parts = partsDtos
                .Where(p => suppliersIds.Contains(p.SupplierId))
                .Select(dto => new Part()
                {
                    Name = dto.Name,
                    Price = dto.Price,
                    Quantity = dto.Quantity,
                    SupplierId = dto.SupplierId,
                }).ToList();

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}";
        }
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CarsDTo[]),
               new XmlRootAttribute("Cars"));

            using var reader = new StringReader(inputXml);
            var carsDtos = (CarsDTo[])serializer.Deserialize(reader);

            List<Car> cars = new List<Car>();

            var partsIds = context.Parts
                .Select(p => p.Id)
                .ToList();

            foreach (var dto in carsDtos)
            {
                var car = new Car()
                {
                    Make = dto.Make,
                    Model = dto.Model,
                    TraveledDistance = dto.TraveledDistance,
                };

                

                int[] carPartsId = dto.PartIds
                    .Where(p => partsIds.Contains(p.Id))
                    .Select(p => p.Id)
                    .Distinct()
                    .ToArray();

                var carParts = new List<PartCar>();

                foreach (var id in carPartsId)
                {
                    carParts.Add(new PartCar()
                    {
                        Car = car,
                        PartId = id,
                    });
                }
                car.PartsCars = carParts;
                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CustomerDto[]), 
                new XmlRootAttribute("Customers"));

            using var reader = new StringReader(inputXml);

            var customersDtos = (CustomerDto[])serializer.Deserialize(reader);

            var customers = customersDtos
                .Select(c => new Customer()
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate,
                    IsYoungDriver = c.IsYoungDriver,
                }).ToList();

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}";
        }
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SalesDto[]), 
                new XmlRootAttribute("Sales"));

            using var reader = new StringReader(inputXml);

            var salesDto = (SalesDto[])serializer.Deserialize(reader);

            var carIds = context.Cars
                .Select(c => c.Id)
                .ToList();

            var sales = salesDto
                .Where(s => carIds.Contains(s.CarId))
                .Select(s => new Sale()
                {
                    CarId = s.CarId,
                    CustomerId = s.CustomerId,
                    Discount = s.Discount,
                }).ToList();

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var carsWithDistance = context.Cars
                .Where(c => c.TraveledDistance > 2_000_000)
                .Select(c => new DTOs.Export.GetCarsWithDistanceDto()
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance,
                })
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ToList();

            return SerializeToXml(carsWithDistance, "cars");
        }
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var carsBMW = context.Cars
                .Where(c => c.Make == "BMW")
                .Select(c => new MakeBMWDto()
                {
                    id = c.Id,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance
                })
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .ToList();

            return SerializeToXml(carsBMW, "cars", true );
        }
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new LocalSuppliersDto()
                {
                    id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count,
                })
                .ToList();

            return SerializeToXml(suppliers, "suppliers");
        }
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(c => new GetCarsWithTheirListOfParts()
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance,
                    Parts = c.PartsCars
                    .Select(p => new CarPartsToExport()
                    {
                        Name = p.Part.Name,
                        Price = p.Part.Price,
                    })
                    .OrderByDescending(p => p.Price)
                    .ToArray()
                })
                .OrderByDescending(c => c.TraveledDistance)
                .ThenBy(c=> c.Model)
                .Take(5)
                .ToList();

            return SerializeToXml(cars, "cars");
        }
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customersToExport = context.Customers
                .Where(c => c.Sales.Any()).
                Select(c => new
                {
                    c.Name,
                    SalesCount = c.Sales.Count,
                    SpentMoney = c.Sales.Select(s => new
                    {
                        prices = c.IsYoungDriver 
                        ? s.Car.PartsCars.Sum(p => Math.Round((double)(p.Part.Price) * 0.95, 2))
                        : s.Car.PartsCars.Sum(p => (double)p.Part.Price)
                    }).ToArray(),
                })
                .ToList();

            var customers = customersToExport
                .Select(c => new ExportTotalSalesByCustomer()
                {
                    Name = c.Name,
                    BoughtCars = c.SalesCount,
                    SpentMoney = c.SpentMoney.Sum(p => (decimal)p.prices)
                })
                .OrderByDescending(m => m.SpentMoney)
                .ToList();

           

            return SerializeToXml(customers, "customers");
        }
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Select(s => new SaleWithDiscount()
                {
                    Car = new CarDto()
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TraveledDistance = s.Car.TraveledDistance
                    },
                    Discount = (int)s.Discount,
                    CustomerName = s.Customer.Name,
                    Price = s.Car.PartsCars
                        .Sum(pc => pc.Part.Price),
                    PriceWithDiscount = Math.Round(
                        (double)(s.Car.PartsCars.Sum(p => p.Part.Price)
                                 * (1 - (s.Discount / 100))), 4)
                }).ToArray();

            return SerializeToXml(sales, "sales");
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