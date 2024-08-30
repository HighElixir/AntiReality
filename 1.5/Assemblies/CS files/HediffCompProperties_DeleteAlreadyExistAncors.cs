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
        public Hediff hediff;

        public HediffCompProperties_DeleteAlreadyExistAncors()
        {
            this.compClass = typeof(HediffComp_DeleteAlreadyExistAncors);
        }
    }
}