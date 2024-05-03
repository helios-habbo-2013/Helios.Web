using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helios.Web.Storage.Models.Group
{
    public class GroupMembershipData
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public string MemberRank { get; set; }
        public bool IsPending { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual GroupData Group { get; set; }
    }
}
