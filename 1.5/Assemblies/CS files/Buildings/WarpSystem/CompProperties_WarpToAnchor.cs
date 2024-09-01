using RimWorld;
using Verse;

namespace HE_AntiReality
{
    public class CompProperties_WarpToAnchor : CompProperties_UseEffect
    {
        public CompProperties_WarpToAnchor()
        {
            this.compClass = typeof(CompUseEffect_WarpToAnchor);
        }
    }
}
