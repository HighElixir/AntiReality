using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using UnityEngine;
using Verse;

namespace HE_AntiReality
{
    public class HediffCompProperties_TransportGizmo : HediffCompProperties
    {
        public HediffCompProperties_TransportGizmo()
        {
            this.compClass = typeof(HediffComp_TransportGizmo);
        }
    }
}
