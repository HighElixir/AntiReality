using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace HE_AntiReality
{
    public class ExceptionDefs
    {
        public HediffDef hediffDef;
        public TraitDef traitDef;
    }
    public class HediffCompProperties_forceDeath : HediffCompProperties
    {
        public List<ExceptionDefs> exceptionDefs;

        public HediffCompProperties_forceDeath()
        {
            compClass = typeof(HediffComp_forceDeath);
        }
    }
}
