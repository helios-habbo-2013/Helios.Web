﻿using System;

namespace Helios.Web.Storage.Models.Messenger
{
    public class MessengerChatData
    {
        public virtual int AvatarId { get; set; }
        public virtual int FriendId { get; set; }
        public virtual string Message { get; set; }
        public virtual bool IsRead { get; set; }
        public virtual DateTime MessagedAt { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var t = obj as MessengerChatData;

            if (t == null)
                return false;

            if (FriendId == t.FriendId &&
                AvatarId == t.AvatarId)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
