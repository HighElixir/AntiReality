using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace HE_AntiReality {
    public class CompProperties_StoreFE : CompProperties
    {
        public float maxFE;
        public float requireFE = 0f;
        public float efficiency = 1f;
        public float consumptionPerTick = 0f;
        public CompProperties_StoreFE()
        {
            compClass = typeof(Comp_StoreFE);
        }
    }
}
