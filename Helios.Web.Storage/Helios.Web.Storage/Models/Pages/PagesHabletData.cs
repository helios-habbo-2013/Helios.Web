using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Helios.Web.Storage.Models.Avatar;

namespace Helios.Web.Storage.Models.User
{
    public class PagesHabletData
    {
        public virtual string Page { get; set; }
        public virtual int OrderId { get; set; }
        public virtual string Widget { get; set; }
        public virtual string Column { get; set; }
    }

}
