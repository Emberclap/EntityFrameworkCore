namespace Invoices.Data
{
    using Models.Enums;
    public static class DataConstraints
    {
        public const int ProductNameMinLength = 9;
        public const int ProductNameMaxLength = 30;
        public const string ProductPriceMinValue = "5,00";
        public const string ProductPriceMaxValue = "1000,00";
        public const int ProductCategoryTypeMinValue = (int)CategoryType.ADR;
        public const int ProductCategoryTypeMaxValue = (int)CategoryType.Tyres;


        public const int AddressStreetNameMinValue = 10;
        public const int AddressStreetNameMaxValue = 20;
        public const int AddressCityMinValue = 5;
        public const int AddressCityMaxValue = 15;
        public const int AddressCountryMinValue = 5;
        public const int AddressCountryMaxValue = 15;


        public const int InvoiceNumberMinRange = 1_000_000_000;
        public const int InvoiceNumberMaxRange = 1_500_000_000;
        public const int InvoiceCurrencyTypeMinValue = (int)CurrencyType.BGN;
        public const int InvoiceCurrencyTypeMaxValue = (int)CurrencyType.USD;

        public const int ClientNameMinLength = 10;
        public const int ClientNameMaxLength = 25;
        public const int ClientNumberVatMinLength = 10;
        public const int ClientNumberVatMaxLength = 15;
    }
}
