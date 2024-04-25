using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace HE_AntiReality
{
    public class HediffCompProperties_forceDeath : HediffCompProperties
    {
        public List<HediffDef> exceptionHediffDefs;
        public List<TraitDef> exceptionTraitDefs;

        public HediffCompProperties_forceDeath()
        {
            compClass = typeof(HediffComp_forceDeath);
        }
    }
}
