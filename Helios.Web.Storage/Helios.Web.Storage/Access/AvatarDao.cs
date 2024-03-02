﻿using Helios.Web.Storage.Models.Avatar;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Helios.Web.Storage.Access
{
    public static class AvatarDao
    {
        /// <summary>
        /// Login user with SSO ticket
        /// </summary>
        public static bool Login(this StorageContext context, out AvatarData loginData, string ssoTicket)
        {
            AvatarData avatarData = null;

            var row = context.AuthenicationTicketData
                .Include(x => x.AvatarData)
                    .ThenInclude(x => x.User)
                .Where(x =>
                   x.AvatarData != null && x.Ticket == ssoTicket &&
                   (x.ExpireDate == null || x.ExpireDate > DateTime.Now))
               .Take(1)
               .SingleOrDefault();

            if (row != null)
                avatarData = row.AvatarData;

            loginData = avatarData;
            return false;

            //AvatarData avatarData = null;
            //using (var session = SessionFactoryBuilder.Instance.SessionFactory.OpenSession())
            //{
            //    AuthenicationTicketData ticketAlias = null;
            //    AvatarData avatarDataAlias = null;
            //    var row = session.QueryOver(() => ticketAlias)
            //        .JoinQueryOver(() => ticketAlias.AvatarData, () => avatarDataAlias)
            //        .Where(() =>
            //            (ticketAlias.AvatarData != null && ticketAlias.Ticket == ssoTicket) &&
            //            (ticketAlias.avatarId == avatarDataAlias.Id) &&
            //            (ticketAlias.ExpireDate == null || ticketAlias.ExpireDate > DateTime.Now))
            //        .Take(1)
            //    .SingleOrDefault();

            //    if (row != null)
            //        avatarData = row.AvatarData;
            //}
            //loginData = avatarData;
            // return false;
        }

        /// <summary>
        /// Save avatar data
        /// </summary>
        /// <param name="avatarData">the avatar data to save</param>
        public static void Update(this StorageContext context, AvatarData avatarData)
        {
            context.Attach(avatarData)
                .Property(x => x.Credits)
                .IsModified = false; // don't override credits amount

            // context.Attach(avatarData).Property(x => x.User).IsModified = false;

            context.Update(avatarData);
            context.SaveChanges();
        }

        /// <summary>
        /// Get avatar by username
        /// </summary>
        public static AvatarData GetByName(this StorageContext context, string name)
        {
            return context.AvatarData
                .Include(x => x.User)
                .FirstOrDefault(x => x.Name == name);
        }

        /// <summary>
        /// Get avatar by id
        /// </summary>
        public static AvatarData GetById(this StorageContext context, int id)
        {
            return context.AvatarData
                .Include(x => x.User)
                .FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Get avatar name by id
        /// </summary>
        public static string GetNameById(this StorageContext context, int id)
        {
            return context.AvatarData
                .Include(x => x.User)
                .Where(x => x.Id == id).Select(x => x.Name).SingleOrDefault();
        }

        /// <summary>
        /// Get avatar id by name
        /// </summary>
        public static int GetIdByName(this StorageContext context, string name)
        {
            return context.AvatarData
                .Include(x => x.User)
                .Where(x => x.Name == name).Select(x => x.Id).SingleOrDefault();
        }
    }
}
