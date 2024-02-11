namespace Helios.Web.Storage.Models.Catalogue
{
    public class CatalogueSubscriptionData
    {
        public virtual int Id { get; set; }
        public virtual int PageId { get; set; }
        public virtual int PriceCoins { get; set; }
        public virtual int PriceSeasonal { get; set; }
        public virtual SeasonalCurrencyType SeasonalType { get; set; }
        public virtual int Months { get; set; }
    }
}
