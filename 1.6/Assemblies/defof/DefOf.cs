using Verse;
using RimWorld;

namespace AntiReality
{
    [DefOf]
    public static class HediffDefOf
    {
        public static HediffDef LostOfReality;

        //public static HediffDef 

        static HediffDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(HediffDefOf));
        }
    }
}