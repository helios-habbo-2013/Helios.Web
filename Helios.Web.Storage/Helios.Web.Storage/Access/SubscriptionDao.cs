using Helios.Web.Storage.Models.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Helios.Web.Storage.Access
{
    public static class SubscriptionDao
    {

        /// <summary>
        /// Get subscription by user id
        /// </summary>
        public static SubscriptionData GetSubscription(this StorageContext context, int avatarId)
        {
            return context.SubscriptionData.SingleOrDefault(x => x.AvatarId == avatarId);

            //using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //{
            //    SubscriptionData subscriptionDataAlias = null;

            //    return session.QueryOver(() => subscriptionDataAlias)
            //        .Where(() => subscriptionDataAlias.avatarId == avatarId)
            //        /*.And(() =>subscriptionDataAlias.ExpireDate > DateTime.Now )*/.SingleOrDefault();
            //}
            //return null;

        }

        /// <summary>
        /// Get subscription gifts
        /// </summary>
        public static List<SubscriptionGiftData> GetSubscriptionGifts(this StorageContext context)
        {
            return context.SubscriptionGiftData.ToList();//.SingleOrDefault(x => x.avatarId == avatarId);

            //using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //{
            //    return session.QueryOver<SubscriptionGiftData>().List() as List<SubscriptionGiftData>;
            //}
            //return null;

        }

        /// <summary>
        /// Create subscription by user id
        /// </summary>
        public static void AddSubscription(this StorageContext context, SubscriptionData subscriptionData)
        {
            context.SubscriptionData.Add(subscriptionData);
            context.SaveChanges();

            //using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //{
            //    using (var transaction = session.BeginTransaction())
            //    {
            //        try
            //        {
            //            session.SaveOrUpdate(subscriptionData);
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
        /// Save subscription by user id
        /// </summary>
        public static void SaveSubscriptionExpiry(this StorageContext context, int avatarId, DateTime expiry)
        {
            context.SubscriptionData.Where(x => x.AvatarId == avatarId).UpdateFromQuery(x => new SubscriptionData { ExpireDate = expiry });
            context.SaveChanges();

            //using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //{
            //    session.Query<SubscriptionData>().Where(x => x.avatarId == avatarId).Update(x => new SubscriptionData { ExpireDate = expiry });
            //}
        }

        /// <summary>
        /// Save club duration by user id
        /// </summary>
        public static void SaveSubscriptionAge(this StorageContext context, int avatarId, long clubAge, DateTime clubAgeLastUpdate)
        {
            context.SubscriptionData.Where(x => x.AvatarId == avatarId)
                .UpdateFromQuery(x => new SubscriptionData { SubscriptionAge = clubAge, SubscriptionAgeLastUpdated = clubAgeLastUpdate });

            context.SaveChanges();

            //using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //{
            //    session.Query<SubscriptionData>().Where(x => x.avatarId == avatarId).Update(x => new SubscriptionData { SubscriptionAge = clubAge, SubscriptionAgeLastUpdated = clubAgeLastUpdate });
            //}
        }

        /// <summary>
        /// Refreshes subscription data
        /// </summary>
        public static void Refresh(this StorageContext context, SubscriptionData data)
        {
            context.Attach(data).Reload();

            //using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //{
            //    session.Refresh(data);
            //}
        }

        /// <summary>
        /// Save how many gifts a user can redeem
        /// </summary>
        public static void SaveGiftsRedeemable(this StorageContext context, int avatarId, int giftsRedeemable)
        {
                //var entity = context.SubscriptionData.Attach(new SubscriptionData { avatarId = avatarId, GiftsRedeemable = giftsRedeemable });
                //entity.Property(x => x.GiftsRedeemable).IsModified = true;
                context.SubscriptionData.Where(x => x.AvatarId == avatarId)
                    .UpdateFromQuery(x => new SubscriptionData { GiftsRedeemable = giftsRedeemable });

                context.SaveChanges();

            //using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //{
            //    session.Query<SubscriptionData>().Where(x => x.avatarId == avatarId).Update(x => new SubscriptionData { GiftsRedeemable = giftsRedeemable });
            //}
        }

    }
}
