using Helios.Web.Storage.Models.Catalogue;
using System.Collections.Generic;
using System.Linq;

namespace Helios.Web.Storage.Access
{
    public static class CatalogueDao
    {
        /// <summary>
        /// Get page data by page id
        /// </summary>
        public static CataloguePageData GetPageData(this StorageContext context, int id)
        {
            return context.CataloguePageData.SingleOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Get all page data
        /// </summary>
        /// <returns></returns>
        public static List<CataloguePageData> GetPages(this StorageContext context)
        {
            return context.CataloguePageData.ToList();
        }

        /// <summary>
        /// Get all catalogue item data
        /// </summary>
        /// <returns></returns>
        public static List<CatalogueItemData> GetItems(this StorageContext context)
        {
            return context.CatalogueItemData.ToList();
        }

        /// <summary>
        /// Get all item packages
        /// </summary>
        /// <returns></returns>
        public static List<CataloguePackageData> GetPackages(this StorageContext context)
        {
            return context.CataloguePackageData.ToList();
        }

        /// <summary>
        /// Get all discount data
        /// </summary>
        /// <returns></returns>
        public static List<CatalogueDiscountData> GetDiscounts(this StorageContext context)
        {
            return context.CatalogueDiscountData.ToList();
        }

        /// <summary>
        /// Get subscription data by page id
        /// </summary>
        public static List<CatalogueSubscriptionData> GetSubscriptionData(this StorageContext context)
        {
            return context.CatalogueSubscriptionData.ToList();
        }
    }
}
