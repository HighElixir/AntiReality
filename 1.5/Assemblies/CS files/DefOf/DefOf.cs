﻿using System;
using Verse;
using RimWorld;

namespace HE_AntiReality
{
    [DefOf]
    public static class HE_HediffDefOf
    {
        public static HediffDef Non_Existent_Hands;

        public static HediffDef AR_InfinityAnchor;

        public static HediffDef AR_MultiImplantor;

        public static HediffDef ExistanceFusion_wound;

        static HE_HediffDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(HE_HediffDefOf));
        }
    }
    [DefOf]
    public static class HE_DamageDefOf
    {
        public static DamageDef SurgicalCut;

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
}