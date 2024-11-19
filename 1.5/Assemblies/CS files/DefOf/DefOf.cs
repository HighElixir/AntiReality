using Verse;
using RimWorld;

namespace HE_AntiReality
{
    [DefOf]
    public static class AR_HediffDefOf
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

        static AR_HediffDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(AR_HediffDefOf));
        }
    }

    [DefOf]
    public static class AR_ThoughtDefOf
    {
        public static ThoughtDef WitnessedLostOfRealityDeath;

        public static ThoughtDef AR_ForceDeath;

        static AR_ThoughtDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof(AR_ThoughtDefOf));
    }

    [DefOf]
    public static class AR_DamageDefOf
    {
        public static DamageDef SurgicalCut;

        public static DamageDef Bathed_in_Energy;

        static AR_DamageDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(AR_DamageDefOf));
        }
    }
    [DefOf]
    public static class AR_ThingDefOf
    {
        public static ThingDef AR_DimensionAnchor;
        static AR_ThingDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(AR_ThingDefOf));
        }
    }

    [DefOf]
    public static class AR_SoundDefOf
    {
        public static SoundDef Bathed_in_Energy_Sound;
        static AR_SoundDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(AR_SoundDefOf));
        }
    }

    [DefOf]
    public static class AR_MentalStateDefOf
    {
        public static MentalStateDef AR_WanderConfused;

        static AR_MentalStateDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof(AR_MentalStateDefOf));
    }

    [DefOf]
    public static class AR_StatDefOf
    {
        public static StatDef AR_Resistance;

        static AR_StatDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof(AR_StatDefOf));
    }
}