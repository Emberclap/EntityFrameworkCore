using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProductShop.DTOs.Export
{

        [XmlType("Products")]
        public class ProductsWithCountDTO
        {
            [XmlElement("count")]
            public int Count { get; set; }

            [XmlArray("products")]
            public List<ProductToDTO> Products { get; set; }
        }


        [XmlType("Product")]
        public class ProductToDTO
        {
            [XmlElement("name")]
            public string Name { get; set; }

            [XmlElement("price")]
            public decimal Price { get; set; }
        }

        [XmlType("User")]
        public class ExportUserDto
        {
            [XmlElement("firstName")]
            public string FirstName { get; set; }

            [XmlElement("lastName")]
            public string LastName { get; set; }

            [XmlElement("age")]
            public int? Age { get; set; }

            [XmlElement("SoldProducts")]
            public ProductsWithCountDTO SoldProducts { get; set; }

        }

        [XmlType("Users")]
        public class UserProductWithCount
        {
            [XmlElement("count")]
            public int Count { get; set; }

            [XmlArray("users")]
            public List<ExportUserDto> Users { get; set; }

        }
    
}
