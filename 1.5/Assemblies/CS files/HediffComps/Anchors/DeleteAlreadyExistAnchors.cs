using RimWorld;
using System.Collections.Generic;
using Verse;

namespace HE_AntiReality
{
    public class HediffCompProperties_DeleteAlreadyExistAnchors : HediffCompProperties
    {
        public HediffCompProperties_DeleteAlreadyExistAnchors() => compClass = typeof(HediffComp_DeleteAlreadyExistAnchors);
    }

    public class HediffComp_DeleteAlreadyExistAnchors : HediffComp
    {
        // 削除対象となるアンカーのリストを定義
        private readonly List<HediffDef> anchors = new List<HediffDef>()
        {
            AR_HediffDefOf.AR_InfinityAnchor,
            AR_HediffDefOf.AR_ZeroDimensionAnchor,
            AR_HediffDefOf.AR_MoltenHeart
        };

        public HediffCompProperties_DeleteAlreadyExistAnchors Props => (HediffCompProperties_DeleteAlreadyExistAnchors)props;

        // 所持Hediffを精査し、最後に追加されたアンカー以外を削除
        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            Pawn p = parent.pawn;
            HediffDef d = parent.def;

            if (d != null)
            {
                // 削除対象のアンカーリストを走査
                foreach (var def in anchors)
                {
                    if (def == d) continue;
                    if (p.health.hediffSet.TryGetHediff(def ,out var h))
                    {
                        p.health.RemoveHediff(h);
                        Messages.Message(AR_Constants.Hint_DeleteAnchor.Translate(h.def.label) , MessageTypeDefOf.TaskCompletion, false);
                    }
                }
            }
        }
    }
}
