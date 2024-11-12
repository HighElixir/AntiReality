using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace HE_AntiReality
{
    public class CompProperties_WarptoAnchor : CompProperties
    {
        public int cooldownTicks;
        public CompProperties_WarptoAnchor() => compClass = typeof(Comp_WarptoAnchor);
    }
}
