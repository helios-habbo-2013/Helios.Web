using Helios.Web.Storage.Models.User;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Helios.Web.Storage.Access
{
    public static class UserDao
    {
        /// <summary>
        /// Save avatar data
        /// </summary>
        /// <param name="userData">the avatar data to save</param>
        public static void Update(this StorageContext context, UserData userData)
        {
                context.Update(userData);

                context.SaveChanges();
        }

        /// <summary>
        /// Get avatar by username
        /// </summary>
        public static UserData GetByName(this StorageContext context, string email)
        {
                return context.UserData
                    .Include(x => x.Avatars)
                    .FirstOrDefault(x => x.Email == email);
        }

        /// <summary>
        /// Get avatar by id
        /// </summary>
        public static UserData GetById(this StorageContext context, int id)
        {
                return context.UserData
                    .Include(x => x.Avatars)
                    .FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Get avatar name by id
        /// </summary>
        public static string GetEmailById(this StorageContext context, int id)
        {
                return context.UserData
                    .Include(x => x.Avatars)
                    .Where(x => x.Id == id)
                    .Select(x => x.Email)
                    .SingleOrDefault();
        }

        /// <summary>
        /// Get avatar id by name
        /// </summary>
        public static int GetIdByEmail(this StorageContext context, string email)
        {
                return context.UserData
                    .Include(x => x.Avatars)
                    .Where(x => x.Email == email)
                    .Select(x => x.Id).SingleOrDefault();
        }
    }
}
