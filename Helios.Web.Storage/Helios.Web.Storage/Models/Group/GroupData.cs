using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helios.Web.Storage.Models.Group
{
    public class GroupData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int OwnerId { get; set; }
        public int RoomId { get; set; }
        public string Badge { get; set; }
        public bool Recommended { get; set; }
        public string Background { get; set; }
        public int Views { get; set; }
        public int Topics { get; set; }
        public byte GroupType { get; set; }
        public byte ForumType { get; set; }
        public byte ForumPermissionType { get; set; }
        public string? Alias { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<GroupMembershipData> GroupMemberships { get; set; }
    }
}
