using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Helios.Web.Storage.Models.Avatar;

namespace Helios.Web.Storage.Models.User
{
    public class PagesData
    {
        public virtual int Id { get; set; }
        public virtual int ParentId { get; set; }
        public virtual int OrderId { get; set; }
        public virtual string Label { get; set; }
        public virtual string Link { get; set; }
        public virtual PageColor Colour { get; set; }
        public virtual int MinimumRank { get; set; }
        public virtual bool RequiresLogin { get; set; }
        public virtual bool RequiresLogout { get; set; }

        #region Contraints

        public virtual PagesData ParentPage { get; set; }

        #endregion
    }

    public enum PageColor
    {
        GREEN,
        BLUE
    }
}
