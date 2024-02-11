using Helios.Web.Storage.Models.User;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Helios.Web.Storage.Access
{
    public class UserDao
    {
        /// <summary>
        /// Save avatar data
        /// </summary>
        /// <param name="userData">the avatar data to save</param>
        public static void Update(UserData userData)
        {
            using (var context = new StorageContext())
            {
                context.Update(userData);

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Get avatar by username
        /// </summary>
        public static UserData GetByName(string email)
        {
            using (var context = new StorageContext())
            {
                return context.UserData
                    .Include(x => x.Avatars)
                    .FirstOrDefault(x => x.Email == email);
            }
        }

        /// <summary>
        /// Get avatar by id
        /// </summary>
        public static UserData GetById(int id)
        {
            using (var context = new StorageContext())
            {
                return context.UserData
                    .Include(x => x.Avatars)
                    .FirstOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Get avatar name by id
        /// </summary>
        public static string GetEmailById(int id)
        {
            using (var context = new StorageContext())
            {
                return context.UserData
                    .Include(x => x.Avatars)
                    .Where(x => x.Id == id)
                    .Select(x => x.Email)
                    .SingleOrDefault();
            }
        }

        /// <summary>
        /// Get avatar id by name
        /// </summary>
        public static int GetIdByEmail(string email)
        {
            using (var context = new StorageContext())
            {
                return context.UserData
                    .Include(x => x.Avatars)
                    .Where(x => x.Email == email)
                    .Select(x => x.Id).SingleOrDefault();
            }
        }
    }
}
