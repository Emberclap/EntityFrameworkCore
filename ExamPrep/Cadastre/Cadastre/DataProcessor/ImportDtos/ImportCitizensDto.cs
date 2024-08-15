using Cadastre.Data.Enumerations;
using Cadastre.Data.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static Cadastre.Data.DataConstrains;
namespace Cadastre.DataProcessor.ImportDtos
{
    [JsonObject(nameof(Citizen))]
    public class ImportCitizensDto
    {
        [Required]
        [MinLength(CitizenFirstNameMinValue)]
        [MaxLength(CitizenFirstNameMaxValue)]
        [JsonProperty(nameof(FirstName))]
        public string FirstName { get; set; } = null!;
        [Required]
        [MinLength(CitizenLastNameMinValue)]
        [MaxLength(CitizenLastNameMaxValue)]
        [JsonProperty(nameof(LastName))]
        public string LastName {  get; set; } = null!;
        [Required]
        [JsonProperty(nameof(BirthDate))]
        public string BirthDate { get; set; } = null!;
        [EnumDataType(typeof(MaritalStatus))]
        [Required]
        [JsonProperty(nameof(MaritalStatus))]
        public string MaritalStatus { get; set; } = null!;
        [JsonProperty(nameof(Properties))]
        public int[] Properties { get; set; } = null!;

    }
}
