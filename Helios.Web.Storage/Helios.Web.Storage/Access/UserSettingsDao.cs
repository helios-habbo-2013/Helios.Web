using System.Linq;
using Helios.Web.Storage.Models.Avatar;

namespace Helios.Web.Storage.Access
{
    public static class UserSettingsDao
    {
        /// <summary>
        /// Create avatar statistics
        /// </summary>
        public static void CreateOrUpdate(this StorageContext context, out AvatarSettingsData settingsData, int avatarId)
        {
            settingsData = new AvatarSettingsData
            {
                AvatarId = avatarId
            };

            if (!context.AvatarSettingsData.Any(x => x.AvatarId == avatarId))
            {
                context.AvatarSettingsData.Add(settingsData);
                context.SaveChanges();
            }
            else
            {
                settingsData = context.AvatarSettingsData.SingleOrDefault(x => x.AvatarId == avatarId);
            }

            //using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //{
            //    bool rowExists = session.QueryOver<AvatarSettingsData>().Where(x => x.avatarId == avatarId).RowCount() > 0;

            //    if (!rowExists)
            //    {
            //        using (var transaction = session.BeginTransaction())
            //        {
            //            try
            //            {
            //                session.SaveOrUpdate(settingsData);
            //                transaction.Commit();
            //            }
            //            catch
            //            {
            //                transaction.Rollback();
            //            }
            //        }
            //    } 
            //    else
            //    {
            //        settingsData = session.QueryOver<AvatarSettingsData>().Where(x => x.avatarId == avatarId).SingleOrDefault();
            //    }
            //}
        }

        /// <summary>
        /// Save avatar statistics
        /// </summary>
        public static void Update(this StorageContext context, AvatarSettingsData settingsData)
        {
                context.AvatarSettingsData.Update(settingsData);
                context.SaveChanges();
        }
    }
}
