using Boardgames.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Boardgames.Data.DataConstrains;

namespace Boardgames.DataProcessor.ImportDto
{
    public class ImportSellersDto
    {
        [Required]
        [MaxLength(SellerNameMaxValue)]
        [MinLength(SellerNameMinValue)]
        public string Name { get; set; } = null!;
        [Required]
        [MaxLength(SellerAddressMaxValue)]
        [MinLength(SellerAddressMinValue)]
        public string Address { get; set; } = null!;
        [Required]
        public string Country { get; set; } = null!;
        [Required]
        [RegularExpression(@"^(www.[-\w\d]+.com)$")]
        public string Website { get; set; } = null!;
        public int[] Boardgames { get; set; } = null!;
    }
}
