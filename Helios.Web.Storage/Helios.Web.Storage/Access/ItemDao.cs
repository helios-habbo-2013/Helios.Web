using Helios.Web.Storage.Models.Item;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Helios.Web.Storage.Access
{
    public static class ItemDao
    {
        /// <summary>
        /// Get list of all definition data
        /// </summary>
        public static List<ItemDefinitionData> GetDefinitions(this StorageContext context)
        {
                return context.ItemDefinitionData.ToList();
        }

        /// <summary>
        /// Get list of all item data for user inventory
        /// </summary>
        public static List<ItemData> GetInventoryItems(this StorageContext context, int avatarId)
        {
                return context.ItemData
                    .Include(x => x.OwnerData)
                    .Where(x => x.OwnerId == avatarId && x.RoomId == null).ToList();
        }

        /// <summary>
        /// Get room items
        /// </summary>
        public static List<ItemData> GetRoomItems(this StorageContext context, int roomId)
        {
            return context.ItemData
                .Include(x => x.OwnerData)
                .Where(x => x.RoomId == roomId)
                .ToList();
        }

        /// <summary>
        /// Get single item
        /// </summary>
        /// <returns>the item</returns>
        public static ItemData GetItem(this StorageContext context, string itemId)
        {
                return context.ItemData
                    .Include(x => x.OwnerData)
                    .SingleOrDefault(x => x.Id == itemId);
        }

        /// <summary>
        /// Save item definition
        /// </summary>
        /// <param name="itemDefinition"></param>
        public static void SaveDefinition(this StorageContext context, ItemDefinitionData itemDefinition)
        {
                context.Update(itemDefinition);
                context.SaveChanges();
        }

        /// <summary>
        /// Save item data
        /// </summary>
        public static void SaveItem(this StorageContext context, ItemData item)
        {
                context.Update(item);
                context.SaveChanges();
                context.Entry(item).Reference(x => x.OwnerData).Load();
        }


        /// <summary>
        /// Create items and refresh it with their filled in database ID's
        /// </summary>
        public static void CreateItems(this StorageContext context, List<ItemData> items)
        {

            context.AddRange(items);
            context.SaveChanges();

            foreach (var item in items)
                context.Entry(item).Reference(x => x.OwnerData).Load();
        }

        /// <summary>
        /// Create item and refresh it with their filled in database ID's
        /// </summary>
        public static void CreateItem(this StorageContext context, ItemData item)
        {
                context.Add(item);
                context.SaveChanges();
                context.Entry(item).Reference(x => x.OwnerData).Load();
        }

        /// <summary>
        /// Delete item
        /// </summary>
        /// <param name="item"></param>
        public static void DeleteItem(this StorageContext context, ItemData item)
        {
            context.ItemData.Remove(item);
            context.SaveChanges();
        }
    }
}
