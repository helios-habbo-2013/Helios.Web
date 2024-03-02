using System.Collections.Generic;
using System.Linq;
using Helios.Web.Storage.Models.Navigator;
using Microsoft.EntityFrameworkCore;

namespace Helios.Web.Storage.Access
{
    public static class NavigatorDao
    {
        /// <summary>
        /// Get list of public items
        /// </summary>
        public static List<PublicItemData> GetPublicItems(this StorageContext context)
        {
            //using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //{
            //    return session.QueryOver<PublicItemData>().List() as List<PublicItemData>;//.Where(x => x.Room != null).ToList();
            //}

            return context.PublicItemData
                .Include(x => x.Room).ThenInclude(x => x.Category)
                .Include(x => x.Room).ThenInclude(x => x.OwnerData)
                .ToList();
        }

        /// <summary>
        /// Get list of room categories
        /// </summary>
        public static List<NavigatorCategoryData> GetCategories(this StorageContext context)
        {
            //using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //{
            //    return session.QueryOver<NavigatorCategoryData>().List() as List<NavigatorCategoryData>;
            //}

            return context.NavigatorCategoryData.ToList();
        }
    }
}