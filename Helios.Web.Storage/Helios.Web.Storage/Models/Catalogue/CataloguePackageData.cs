namespace Helios.Web.Storage.Models.Catalogue
{
    public class CataloguePackageData
    {
        public virtual int Id { get; set; }
        public virtual string SaleCode { get; set; }
        public virtual int? DefinitionId { get; set; }
        public virtual string SpecialSpriteId { get; set; }
        public virtual int Amount { get; set; }
    }
}
