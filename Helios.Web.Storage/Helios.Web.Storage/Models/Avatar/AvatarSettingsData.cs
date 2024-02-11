﻿namespace Helios.Web.Storage.Models.Avatar
{
    public class AvatarSettingsData
    {
        public virtual int AvatarId { get; set; }
        public virtual int Respect { get; set; }
        public virtual int DailyRespectPoints { get; set; }
        public virtual int DailyPetRespectPoints { get; set; }
        public virtual bool FriendRequestsEnabled { get; set; }
        public virtual bool FollowingEnabled { get; set; }
        public virtual long OnlineTime { get; set; }
        public virtual bool NameChangeEnabled { get; set; }

        #region Constraints

        public virtual AvatarData AvatarData { get; set; }

        #endregion
    }
}
