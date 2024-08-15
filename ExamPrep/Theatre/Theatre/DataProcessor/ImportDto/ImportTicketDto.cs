using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Theatre.Data.Models;
using static Theatre.Data.DataConstraints;
namespace Theatre.DataProcessor.ImportDto
{
    public class ImportTicketDto
    {
        [Range(typeof(decimal), TicketPriceMinValue, TicketPriceMaxValue)]
        public decimal Price { get; set; }

        [Range(TicketRowNumberMinValue, TicketRowNumberMaxValue)]
        public sbyte RowNumber { get; set; }
        public int PlayId { get; set; }

    }
}
