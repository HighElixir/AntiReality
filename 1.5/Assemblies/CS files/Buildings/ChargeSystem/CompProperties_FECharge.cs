using PipeSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace HE_AntiReality
{
    public class CompProperties_FECharge : CompProperties_ResourceTrader
    {
        public float chargepertick = 0.1f;
        public float chargeEfficiency = 1f;
        public ThingCategoryDef category;
        public CompProperties_FECharge() { compClass = typeof(CompChargeFE); }
    }
}
