using UnityEngine;
using Verse;
using RimWorld;
using AntiReality.Utility;

namespace AntiReality
{
    public class LostofRealityUtility
    {
        private HediffDef _lostofRealityCache;
        private MakeMoteUtility _makeMoteUtility = new MakeMoteUtility();

        public LostofRealityUtility()
        {
            _lostofRealityCache = HediffDefOf.LostOfReality;
        }
        public void Add(Pawn target, float severity, bool hideMote = false)
        {
            if (target == null) return;

            var hediffSet = target.health.hediffSet;
            float randomizedSeverity = severity * Random.Range(0.7f, 1.4f);

            if (hediffSet.TryGetHediff(_lostofRealityCache, out Hediff existingHediff))
            {
                int before = GetSeverityProgresses(existingHediff.Severity);
                int after = GetSeverityProgresses(existingHediff.Severity + randomizedSeverity);

                if (!hideMote && before < after)
                {
                    MakeMote(target);
                }

                existingHediff.Severity = Mathf.Min(existingHediff.Severity + randomizedSeverity, 1.0f);
            }
            else
            {
                Hediff newHediff = HediffMaker.MakeHediff(_lostofRealityCache, target);
                newHediff.Severity = randomizedSeverity;
                target.health.AddHediff(newHediff);
            }
        }

        public int GetSeverityProgresses(float severity)
        {
            if (severity < 0.2f) return 0;
            if (severity < 0.4f) return 1;
            if (severity < 0.6f) return 2;
            if (severity < 0.8f) return 3;
            return 4;
        }

        private void MakeMote(Pawn t)
        {
            if (t == null) return;

            Vector3 v = _makeMoteUtility.MakeMoteToPawn(t);

            // モートを生成
            MoteMaker.ThrowText(v, t.Map, Constants.Hint_LostOfReality.Translate(), Constants.fictionDeepPurple);
        }
    }
}
