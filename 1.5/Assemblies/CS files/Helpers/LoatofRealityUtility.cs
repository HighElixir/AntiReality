using UnityEngine;
using Verse;
using RimWorld;

namespace HE_AntiReality
{
    public static class LostofRealityUtility
    {
        public static void Add(Pawn target, float severity, bool hideMote = false)
        {
            var h1 = new Hediff()
            {
                def = HE_HediffDefOf.LostOfReality,
                Severity = severity
            };
            var hediffSet = target.health.hediffSet;

            if (hediffSet.TryGetHediff(HE_HediffDefOf.LostOfReality, out Hediff h2))
            {
                if (hideMote && GetDecreaseRealityProgresses(h2.Severity) < GetDecreaseRealityProgresses(h2.Severity + severity))
                {
                    MakeMote(target);
                }
                h2.Severity += severity;
            }
            else
            {
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

        public static void MakeMote(Pawn t)
        {
            if (t == null) return;
            Vector3 v = new Vector3(t.Position.x + 1f, t.Position.y, t.Position.z + 1f);
            MoteMaker.ThrowText(v, t.Map, HE_Constants.Hint_LostOfReality.Translate(), HE_Constants.fictionDeepPurple);
        }
    }
}
