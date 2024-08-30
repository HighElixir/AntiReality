using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace HE_AntiReality
{
    public class HediffCompProperties_DeleteAlreadyExistAncors : HediffCompProperties
    {
        public HediffDef hediffDef;

        public HediffCompProperties_DeleteAlreadyExistAncors()
        {
            compClass = typeof(HediffComp_DeleteAlreadyExistAncors);
        }
    }
}