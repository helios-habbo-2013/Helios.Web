using Helios.Web.Storage.Models.Misc;
using Helios.Web.Storage.Models.Room;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Helios.Web.Storage.Access
{
    public static class RoomDao
    {
        public static List<RoomData> SearchRooms(this StorageContext context, string query, int roomLimit = 50)
        {
            //using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //{
            //    RoomData roomDataAlias = null;
            //    AvatarData avatarDataAlias = null;
            //    return session.QueryOver<RoomData>(() => roomDataAlias)
            //        .JoinQueryOver<AvatarData>(() => roomDataAlias.OwnerData, () => avatarDataAlias)
            //            .Where(() => avatarDataAlias.Id == roomDataAlias.OwnerId)
            //        .Where(
            //            Restrictions.On(() => roomDataAlias.Name).IsInsensitiveLike(query, MatchMode.Anywhere) ||
            //            Restrictions.On(() => avatarDataAlias.Name).IsInsensitiveLike(query, MatchMode.Anywhere))
            //        .OrderBy(() => roomDataAlias.UsersNow).Desc
            //        .OrderBy(() => roomDataAlias.Rating).Desc
            //        .Take(roomLimit)
            //        .List<RoomData>() as List<RoomData>;
            //}

                return context.RoomData
                    .Include(x => x.OwnerData)
                    .Include(x => x.Category)
                    .Include(x => x.Tags)
                    .Where(x =>
                        x.Name.ToLower().Contains(query.ToLower()) ||
                        x.OwnerData.Name.ToLower().Contains(query.ToLower()))
                    .OrderByDescending(x => x.UsersNow)
                    .OrderByDescending(x => x.Rating)
                    .Take(roomLimit)
                    .ToList();
        }

        /// <summary>
        /// Search rooms by tag
        /// </summary>
        public static List<RoomData> SearchTags(this StorageContext context, string tag, int roomLimit = 50)
        {
            //using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //{
            //    RoomData roomDataAlias = null;
            //    TagData tagAlias = null;
            //    AvatarData ownerAlias = null;

            //    return session.QueryOver<RoomData>(() => roomDataAlias)
            //        .JoinQueryOver<TagData>(() => roomDataAlias.Tags, () => tagAlias)
            //        .JoinQueryOver<AvatarData>(() => roomDataAlias.OwnerData, () => ownerAlias)
            //        .Where(() => 
            //            tagAlias.Text == tag &&
            //            ownerAlias.Name != null)
            //        .OrderBy(() => roomDataAlias.UsersNow).Desc
            //        .OrderBy(() => roomDataAlias.Rating).Desc
            //        .Take(roomLimit)
            //        .List<RoomData>() as List<RoomData>;
            //}

                return context.RoomData
                    .Include(x => x.OwnerData)
                    .Include(x => x.Tags)
                    .Include(x => x.Category)
                    .Where(x =>
                        x.Tags.Any(t => t.Text == tag))
                    .OrderByDescending(x => x.UsersNow)
                    .OrderByDescending(x => x.Rating)
                    .Take(roomLimit)
                    .ToList();
        }

        /// <summary>
        /// Get the list of users' own rooms
        /// </summary>
        public static List<RoomData> GetPopularFlats(this StorageContext context, int resultsLimit = 50)
        {
            //using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //{
            //    RoomData roomDataAlias = null;
            //    AvatarData avatarDataAlias = null;

            //    return session.QueryOver<RoomData>(() => roomDataAlias)
            //        .JoinQueryOver<AvatarData>(() => roomDataAlias.OwnerData, () => avatarDataAlias)
            //            .Where(() => avatarDataAlias.Id == roomDataAlias.OwnerId)
            //        .OrderBy(() => roomDataAlias.UsersNow).Desc
            //        .OrderBy(() => roomDataAlias.Rating).Desc
            //        .Take(resultsLimit)
            //        .List() as List<RoomData>;
            //}

                return context.RoomData
                    .Include(x => x.OwnerData)
                    .Include(x => x.Category)
                    .Include(x => x.Tags)
                    .Where(x => x.OwnerId > 0)
                    .OrderByDescending(x => x.UsersNow)
                    .OrderByDescending(x => x.Rating)
                    .Take(resultsLimit)
                    .ToList();

        }

        /// <summary>
        /// Get the list of users' own rooms
        /// </summary>
        public static List<RoomData> GetUserRooms(this StorageContext context, int avatarId)
        {
            //using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //{
            //    RoomData roomDataAlias = null;

            //    return session.QueryOver<RoomData>(() => roomDataAlias)
            //        .Where(() => roomDataAlias.OwnerId == avatarId)
            //        .OrderBy(() => roomDataAlias.UsersNow).Desc
            //        .OrderBy(() => roomDataAlias.Rating).Desc
            //        .List() as List<RoomData>;
            //}


                return context.RoomData
                    .Include(x => x.OwnerData)
                    .Include(x => x.Category)
                    .Include(x => x.Tags)
                    .Where(x => x.OwnerId == avatarId)
                    .OrderByDescending(x => x.UsersNow)
                    .OrderByDescending(x => x.Rating)
                    .ToList();

        }

        /// <summary>
        /// Get the list of users' own rooms
        /// </summary>
        public static List<TagData> GetRoomTags(this StorageContext context, int roomId)
        {
            //using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //{
            //    TagData tagDataAlias = null;
            //    return session.QueryOver<TagData>(() => tagDataAlias).Where(() => tagDataAlias.RoomId == roomId).List() as List<TagData>;
            //}

                return context.TagData.Where(x => x.RoomId == roomId).ToList();
        }

        /// <summary>
        /// Count the rooms the user has.
        /// </summary>
        public static int CountUserRooms(this StorageContext context, int avatarId)
        {
            //using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //{
            //    return session.QueryOver<RoomData>().Where(x => x.OwnerId == avatarId).RowCount();
            //}

                return context.RoomData.Count(x => x.OwnerId == avatarId);
        }

        /// <summary>
        /// Get the room model data
        /// </summary>
        /// <returns></returns>
        public static List<RoomModelData> GetModels(this StorageContext context)
        {
            //using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //{
            //    return session.QueryOver<RoomModelData>().List() as List<RoomModelData>;
            //}

                return context.RoomModelData.ToList();
        }

        /// <summary>
        /// Get data for room
        /// </summary>
        public static RoomData GetRoomData(this StorageContext context, int roomId)
        {
            //using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //{
            //    return session.QueryOver<RoomData>().Where(x => x.Id == roomId).Take(1).SingleOrDefault();
            //}
  
                return context.RoomData
                    .Include(x => x.OwnerData)
                    .Include(x => x.Category)
                    .Include(x => x.Tags)
                    .FirstOrDefault(x => x.Id == roomId);
        }

        /// <summary>
        /// Save room data
        /// </summary>
        public static void SaveRoom(this StorageContext context, RoomData data)
        {
                context.RoomData.Update(data);
                context.SaveChanges();

            //using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //{
            //    using (var transaction = session.BeginTransaction())
            //    {
            //        try
            //        {
            //            session.Update(data);
            //            transaction.Commit();
            //            session.Refresh(data);
            //        }
            //        catch
            //        {
            //            transaction.Rollback();
            //        }
            //    }
            //}
        }

        /// <summary>
        /// New room data
        /// </summary>
        public static void NewRoom(this StorageContext context, RoomData data)
        {
                context.RoomData.Add(data);
                context.SaveChanges();

            //using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //{
            //    using (var transaction = session.BeginTransaction())
            //    {
            //        try
            //        {
            //            session.Save(data);
            //            transaction.Commit();
            //        }
            //        catch
            //        {
            //            transaction.Rollback();
            //        }
            //    }
            //}
        }

        /// <summary>
        /// Reset all visitors
        /// </summary>
        public static void ResetVisitorCounts(this StorageContext context)
        {
                context.RoomData.Where(x => x.UsersNow > 0).UpdateFromQuery(x => new RoomData { UsersNow = 0 });

            //using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //{
            //    session.Query<RoomData>().Where(x => x.UsersNow > 0 || x.UsersNow < 0).Update(x => new RoomData { UsersNow = 0 });
            //}
        }

        /// <summary>
        /// Update users count
        /// </summary>
        public static void SetVisitorCount(this StorageContext context, int roomId, int visitorsNow)
        {
            context.RoomData.Where(x => x.Id == roomId).UpdateFromQuery(x => new RoomData { UsersNow = visitorsNow });
            //using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //{
            //    session.Query<RoomData>().Where(x => x.Id == roomId).Update(x => new RoomData { UsersNow = visitorsNow });
            //}
        }
    }
}
