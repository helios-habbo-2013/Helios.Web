using System;
using System.Collections.Generic;
using System.Text;

namespace Helios.Web.Storage.Models.Effect
{
    public class EffectSettingData
    {
        public virtual int EffectId { get; set; }
        public virtual int Duration { get; set; }
        public virtual bool IsCostume { get; set; }
    }
}
