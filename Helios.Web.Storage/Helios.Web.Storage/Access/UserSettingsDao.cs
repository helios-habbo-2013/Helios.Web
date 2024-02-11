using System.Linq;
using Helios.Web.Storage.Models.Avatar;

namespace Helios.Web.Storage.Access
{
    public class UserSettingsDao
    {
        /// <summary>
        /// Create avatar statistics
        /// </summary>
        public static void CreateOrUpdate(out AvatarSettingsData settingsData, int avatarId)
        {
            settingsData = new AvatarSettingsData
            {
                AvatarId = avatarId
            };

            using (var context = new StorageContext())
            {
                if (!context.AvatarSettingsData.Any(x => x.AvatarId == avatarId))
                {
                    context.AvatarSettingsData.Add(settingsData);
                    context.SaveChanges();
                }
                else
                {
                    settingsData = context.AvatarSettingsData.SingleOrDefault(x => x.AvatarId == avatarId);
                }
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
        public static void Update(AvatarSettingsData settingsData)
        {
            using (var context = new StorageContext())
            {
                context.AvatarSettingsData.Update(settingsData);
                context.SaveChanges();
            }
        }
    }
}
