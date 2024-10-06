using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace HE_AntiReality
{
    public class HediffCompProperties_DeleteAlreadyExistAnchors : HediffCompProperties
    {
        public HediffDef hediffDef;

        public HediffCompProperties_DeleteAlreadyExistAnchors()
        {
            compClass = typeof(HediffComp_DeleteAlreadyExistAnchors);
        }
    }
}