using System;

namespace Helios.Web.Storage.Models.Catalogue
{
    public class CatalogueDiscountData
    {
        public virtual int PageId { get; set; }
        public virtual int PurchaseLimit { get; set; }
        public virtual decimal DiscountBatchSize { get; set; }
        public virtual decimal DiscountAmountPerBatch { get; set; }
        public virtual decimal MinimumDiscountForBonus { get; set; }
        public virtual DateTime? ExpireDate { get; set; }
    }
}
