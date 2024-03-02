using Helios.Web.Storage.Models.Avatar;
using Helios.Web.Storage.Models.Messenger;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Helios.Web.Storage.Access
{
    public static class MessengerDao
    {
        /// <summary>
        /// Search messenger for names starting with the query
        /// </summary>
        /// <returns></returns>
        public static List<AvatarData> SearchMessenger(this StorageContext context, string query, int ignoreavatarId, int searchResultLimit = 30)
        {
            /*using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            {
                AvatarData avatarDataAlias = null;

                return session.QueryOver<AvatarData>(() => avatarDataAlias)
                    //.Where(Restrictions.On<AvatarData>(x => x.Name).IsInsensitiveLike(query, MatchMode.Start))
                    .WhereRestrictionOn(() => avatarDataAlias.Name).IsLike(query, MatchMode.Start)
                    .And(() => avatarDataAlias.Id != ignoreavatarId)
                    .Take(searchResultLimit)
                    .List() as List<AvatarData>;
            }*/

            return context.AvatarData.Where(x => x.Name.ToLower().StartsWith(query.ToLower())
                && x.Id != ignoreavatarId)
                .Take(searchResultLimit).ToList();

        }

        /// <summary>
        /// Get the requests for the user
        /// </summary>
        public static List<MessengerRequestData> GetRequests(this StorageContext context, int avatarId)
        {
            /*using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            {
                MessengerRequestData messengerRequestAlias = null;
                AvatarData avatarDataAlias = null;

                return session.QueryOver(() => messengerRequestAlias)
                    .JoinQueryOver(() => messengerRequestAlias.FriendData, () => avatarDataAlias)
                    .Where(() => messengerRequestAlias.avatarId == avatarId)
                    .List() as List<MessengerRequestData>;
            }*/


            return context.MessengerRequestData.Where(x => x.AvatarId == avatarId)
                .Include(x => x.FriendData)
                .ToList();

        }

        /// <summary>
        /// Get the friends for the user
        /// </summary>
        public static List<MessengerFriendData> GetFriends(this StorageContext context, int avatarId)
        {
            /*using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            {
                MessengerFriendData messengerFriendAlias = null;
                AvatarData avatarDataAlias = null;

                return session.QueryOver(() => messengerFriendAlias)
                    .JoinQueryOver(() => messengerFriendAlias.FriendData, () => avatarDataAlias)
                    .Where(() => messengerFriendAlias.avatarId == avatarId)
                    .List() as List<MessengerFriendData>;
            }*/

            return context.MessengerFriendData.Where(x => x.AvatarId == avatarId)
                .Include(x => x.FriendData)
                .ToList();

        }

        /// <summary>
        /// Get the messenger categories for the user
        /// </summary>
        public static List<MessengerCategoryData> GetCategories(this StorageContext context, int avatarId)
        {
            //using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //{
            //    MessengerCategoryData messengerCategoryAlias = null;

            //    return session.QueryOver(() => messengerCategoryAlias)
            //        .Where(() => messengerCategoryAlias.avatarId == avatarId)
            //        .List() as List<MessengerCategoryData>;
            //}

            return context.MessengerCategoryData.Where(x => x.AvatarId == avatarId).ToList();
        }

        /// <summary>
        /// Get if the user supports friend requests
        /// </summary>
        public static bool GetAcceptsFriendRequests(this StorageContext context, int avatarId)
        {
            //using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //{
            //    AvatarSettingsData settingsAlias = null;
            //    AvatarData avatarDataAlias = null;

            //    return session.QueryOver(() => settingsAlias)
            //        .JoinEntityAlias(() => avatarDataAlias, () => settingsAlias.avatarId == avatarDataAlias.Id)
            //        .Where(() => avatarDataAlias.Id == avatarId && settingsAlias.FriendRequestsEnabled)
            //        .List().Count > 0;
            //}

            return context.MessengerRequestData
                .Include(x => x.FriendData)
                .ThenInclude(x => x.Settings)
                .Any(x => x.FriendData.Settings.FriendRequestsEnabled);

        }

        /// <summary>
        /// Deletes friend requests
        /// </summary>
        public static void DeleteRequests(this StorageContext context, int avatarId, int friendId)
        {
            //    using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //    {
            //        session.Query<MessengerRequestData>().Where(x => 
            //            (x.FriendId == friendId && x.avatarId == avatarId) || 
            //            (x.FriendId == avatarId && x.avatarId == friendId))
            //        .Delete();
            //    }

            context.MessengerRequestData.RemoveRange(
                context.MessengerRequestData.Where(x =>
                    x.FriendId == friendId && x.AvatarId == avatarId ||
                    x.FriendId == avatarId && x.AvatarId == friendId)
                );
        }

        /// <summary>
        /// Delete all requests by user id
        /// </summary>
        public static void DeleteAllRequests(this StorageContext context, int avatarId)
        {
            //using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //{
            //    session.Query<MessengerRequestData>().Where(x =>
            //        (x.FriendId == avatarId || x.avatarId == avatarId))
            //    .Delete();
            //}

            context.MessengerRequestData.RemoveRange(
                context.MessengerRequestData.Where(x =>
                    x.FriendId == avatarId || x.AvatarId == avatarId)
                );
        }

        /// <summary>
        /// Deletes friends
        /// </summary>
        public static void DeleteFriends(this StorageContext context, int avatarId, int friendId)
        {
            //using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //{
            //    session.Query<MessengerFriendData>().Where(x =>
            //        (x.FriendId == friendId && x.avatarId == avatarId) ||
            //        (x.FriendId == avatarId && x.avatarId == friendId))
            //    .Delete();
            //}

            context.MessengerFriendData.RemoveRange(
                context.MessengerFriendData.Where(x =>
                   x.FriendId == friendId && x.AvatarId == avatarId ||
                   x.FriendId == avatarId && x.AvatarId == friendId)
                );
        }

        /// <summary>
        /// Save a request
        /// </summary>
        public static void SaveRequest(this StorageContext context, MessengerRequestData messengerRequestData)
        {
            //using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //{
            //    using (var transaction = session.BeginTransaction())
            //    {
            //        try
            //        {
            //            session.Save(messengerRequestData);
            //            transaction.Commit();
            //        }
            //        catch
            //        {
            //            transaction.Rollback();
            //        }
            //    }
            //}

            context.MessengerRequestData.Add(messengerRequestData);
            context.SaveChanges();
        }

        /// <summary>
        /// Save a request
        /// </summary>
        public static void SaveFriend(this StorageContext context, MessengerFriendData messengerFriendData)
        {
            //using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //{
            //    using (var transaction = session.BeginTransaction())
            //    {
            //        try
            //        {
            //            session.Save(messengerFriendData);
            //            transaction.Commit();
            //        }
            //        catch
            //        {
            //            transaction.Rollback();
            //        }
            //    }
            //}

            context.MessengerFriendData.Add(messengerFriendData);
            context.SaveChanges();
        }

        /// <summary>
        /// Save a message
        /// </summary>
        public static void SaveMessage(this StorageContext context, MessengerChatData messengerChatData)
        {
            //using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //{
            //    using (var transaction = session.BeginTransaction())
            //    {
            //        try
            //        {
            //            session.Save(messengerChatData);
            //            transaction.Commit();
            //        }
            //        catch
            //        {
            //            transaction.Rollback();
            //        }
            //    }
            //}

            context.MessengerChatData.Add(messengerChatData);
            context.SaveChanges();
        }

        /// <summary>
        /// Deletes friend requests
        /// </summary>
        public static void SetReadMessages(this StorageContext context, int avatarId)
        {
            //using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //{
            //    session.Query<MessengerChatData>().Where(x => x.FriendId == avatarId && !x.IsRead).Update(x => new MessengerChatData { IsRead = true });
            //}

            context.MessengerChatData
                .Where(x => x.FriendId == avatarId && !x.IsRead)
                .ToList()
                .ForEach(x =>
                {
                    x.IsRead = true;
                });

            context.SaveChanges();
        }

        /// <summary>
        /// Deletes friend requests
        /// </summary>
        public static List<MessengerChatData> GetUneadMessages(this StorageContext context, int avatarId)
        {
            //using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //{
            //    return session.QueryOver<MessengerChatData>().Where(x => x.FriendId == avatarId && !x.IsRead).List() as List<MessengerChatData>;
            //}

            return context.MessengerChatData.Where(x => x.FriendId == avatarId && !x.IsRead).ToList();
        }
    }
}
