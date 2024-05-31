using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace HE_AntiReality
{
    public class Hediffs
    {
        public HediffDef hediffDef;

        public bool skipIfAlreadyExists = true;
    }
    public class HediffCompProperties_GiveHediffs : HediffCompProperties
    {
        public List<Hediffs> hediffs;

        public HediffCompProperties_GiveHediffs()
        {
            this.compClass = typeof(HediffComp_GiveHediffs);
        }
    }


}
