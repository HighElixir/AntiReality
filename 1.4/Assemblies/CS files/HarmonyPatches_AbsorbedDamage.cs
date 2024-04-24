using RimWorld;
using Verse;
using HarmonyLib;
using System.Reflection;
using System;

namespace HE_AntiReality
{


    [StaticConstructorOnStartup]
    public static class HarmonyPatches_AbsorbedDamage
    {
        static HarmonyPatches_AbsorbedDamage()
        {
            // Harmonyのインスタンスを作成
            Harmony harmony = new Harmony("rimworld.HE.AntiReality");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            // パッチ適用
            //harmony.Patch(
            //    original: AccessTools.Method(typeof(Pawn_HealthTracker), nameof(Pawn_HealthTracker.PreApplyDamage)),
            //    prefix: new HarmonyMethod(typeof(HarmonyPatches_AbsorbedDamage), nameof(PreApplyDamage_Prefix))
            //);
        }

        // ダメージが適用される前に呼び出されるメソッド
        [HarmonyPatch(typeof(Pawn_HealthTracker), nameof(Pawn_HealthTracker.PreApplyDamage))]
        [HarmonyPrefix]
        public static bool PreApplyDamage_Prefix(DamageInfo dinfo, ref bool absorbed, Pawn_HealthTracker __instance)
        {
            Pawn pawn = Traverse.Create(root: __instance).Field(name: "pawn").GetValue<Pawn>();
            if (!pawn?.Faction?.IsPlayer ?? true) return true;

            if (pawn != null)
            {
                Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HE_HediffDefOf.AR_InfinityAnchor, false);
                if (firstHediffOfDef != null)
                {
                    absorbed = true;
                    return false;
                }
            }
            return true;
        }
    }

}