using RimWorld;
using System.Collections.Generic;
using Verse;
using UnityEngine;
using System.Linq;

namespace HE_AntiReality
{
    public enum GiveThoughtPattern
    {
        PawnDaed,
        PawnHas,
        PawnClearHediff
    }
    public class HediffCompProperties_GiveThoughtToWitnesses : HediffCompProperties
    {
        public ThoughtDef giveThoughtDef;
        public float radius;
        public bool isWallThrough = false;
        public GiveThoughtPattern pattern;

        public HediffCompProperties_GiveThoughtToWitnesses()
        {
            compClass = typeof(HediffCompGiveThoughtToWitnesses);
        }
    }

    public class HediffCompGiveThoughtToWitnesses : HediffComp
    {
        public HediffCompProperties_GiveThoughtToWitnesses Props => (HediffCompProperties_GiveThoughtToWitnesses)props;

        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {
            Debug.Log($"Sartch Pawns. from {parent.def.label}");
            // ThoughtDefが設定されていない場合、もしくはpatternがPawnDaedでない場合は何もしない
            if (Props.giveThoughtDef == null || Props.pattern != GiveThoughtPattern.PawnDaed) return;

            // 影響を受けるポーンのリストを取得
            IEnumerable<Pawn> witnesses = GetWitnessesAround();
            foreach (Pawn witness in witnesses)
            {
                // ポーンにThoughtを与える
                witness.needs.mood.thoughts.memories.TryGainMemory(Props.giveThoughtDef);
            }
        }

        private IEnumerable<Pawn> GetWitnessesAround()
        {
            
            // 現在のポーンの位置を中心に、指定された半径内のポーンを取得
            Map map = parent.pawn.Map;
            if (map == null) yield break;

            IEnumerable<Pawn> potentialWitnesses = GenRadial.RadialDistinctThingsAround(parent.pawn.Position, map, Props.radius, true)
                                                            .OfType<Pawn>()
                                                            .Where(p => p != parent.pawn && p.Spawned && !p.Dead && !p.Downed);

            foreach (Pawn witness in potentialWitnesses)
            {
                // isWallThroughがfalseならば、視界が通っているか確認
                if (!Props.isWallThrough && !GenSight.LineOfSight(parent.pawn.Position, witness.Position, map))
                    continue;

                yield return witness;
            }
        }
    }

}

