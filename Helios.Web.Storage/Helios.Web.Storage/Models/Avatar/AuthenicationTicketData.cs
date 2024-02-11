using System;

namespace Helios.Web.Storage.Models.Avatar
{
    public class AuthenicationTicketData
    {
        public virtual int AvatarId { get; set; }
        public virtual string Ticket { get; set; }
        public virtual DateTime? ExpireDate { get; set; }

        #region One to Many relationship

        public virtual AvatarData AvatarData { get; set; }

        #endregion
    }
}
