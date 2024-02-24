using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Helios.Web.Storage.Models.Avatar;

namespace Helios.Web.Storage.Models.User
{
    public class UserData
    {
        public virtual int Id { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual string Birthday { get; set; }
        public virtual DateTime JoinDate { get; set; }
        public virtual DateTime LastOnline { get; set; }
        public virtual bool DirectEmail { get; set; }

        #region Contraints

        public virtual List<AvatarData> Avatars { get; set; }

        #endregion
    }
}
