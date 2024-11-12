using System;
using Verse;
using RimWorld;

namespace HE_AntiReality
{
    [DefOf]
    public static class HE_HediffDefOf
    {
        public static HediffDef Non_Existent_Hands;

        public static HediffDef AR_InfinityAnchor;

        public static HediffDef AR_ZeroDimensionAnchor;

        public static HediffDef AR_MultiImplantor;

        public static HediffDef AR_MoltenHeart;

        public static HediffDef ExistanceFusion_wound;

        public static HediffDef LostOfReality;

        public static HediffDef AR_OverLoad;

        //public static HediffDef 

        static HE_HediffDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(HE_HediffDefOf));
        }
    }
    [DefOf]
    public static class HE_DamageDefOf
    {
        public static DamageDef SurgicalCut;

        public static DamageDef Bathed_in_Energy;

        static HE_DamageDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(HE_DamageDefOf));
        }
    }
    [DefOf]
    public static class HE_ThingDefOf
    {
        public static ThingDef AR_DimensionAnchor;
        static HE_ThingDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(HE_ThingDefOf));
        }
    }

    [DefOf]
    public static class HE_SoundDefOf
    {
        public static SoundDef Bathed_in_Energy_Sound;
        static HE_SoundDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(HE_SoundDefOf));
        }
    }
}