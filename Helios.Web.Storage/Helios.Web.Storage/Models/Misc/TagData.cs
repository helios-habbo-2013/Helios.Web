﻿using Helios.Web.Storage.Models.Room;

namespace Helios.Web.Storage.Models.Misc
{
    public class TagData
    {
        public virtual int AvatarId { get; set; }
        public virtual int RoomId { get; set; }
        public virtual string Text { get; set; }

        #region Constraints

        public virtual RoomData RoomData { get; set; }

        #endregion
    }
}
