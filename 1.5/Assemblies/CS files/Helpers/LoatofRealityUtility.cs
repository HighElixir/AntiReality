using UnityEngine;
using Verse;
using RimWorld;
using HighElixir.Utility.RimWorld;

namespace HE_AntiReality
{
    public static class LostofRealityUtility
    {
        public static void Add(Pawn target, float severity, bool hideMote = false)
        {
            var l = AR_HediffDefOf.LostOfReality;
            var s = severity * (1f - target.GetStatValue(AR_StatDefOf.AR_Resistance));
            var hediffSet = target.health.hediffSet;


            if (hediffSet.TryGetHediff(l, out Hediff h2))
            {
                if (hideMote && GetDecreaseRealityProgresses(h2.Severity) < GetDecreaseRealityProgresses(h2.Severity + severity))
                {
                    MakeMote(target);
                }
                h2.Severity += s;
            }
            else
            {
                var h1 = new Hediff()
                {
                    def = l,
                    Severity = s
                };
                hediffSet.AddDirect(h1);
            }
        }

        private static int GetDecreaseRealityProgresses(float severity)
        {
            if (severity < 0.2f) return 0;
            if (severity < 0.4f) return 1;
            if (severity < 0.6f) return 2;
            if (severity < 0.8f) return 3;
            return 4;
        }

        private static void MakeMote(Pawn t)
        {
            if (t == null) return;

            Vector3 v = HE_RimUtility.MakeMotePosition(t);

            // モートを生成
            MoteMaker.ThrowText(v, t.Map, AR_Constants.Hint_LostOfReality.Translate(), AR_Constants.fictionDeepPurple);
        }
    }
}
