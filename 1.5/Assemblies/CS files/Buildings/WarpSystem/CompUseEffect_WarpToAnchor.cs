using Verse;
using RimWorld;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using PipeSystem;

namespace HE_AntiReality
{
    public class CompUseEffect_WarpToAnchor : CompUseEffect
    {
        public override void DoEffect(Pawn user)
        {
            base.DoEffect(user);

            var anchors = FindAllDimensionAnchors();
            if (anchors.Count > 0)
            {
                var closestAnchor = anchors.OrderBy(anchor => (anchor.Position - user.Position).LengthHorizontalSquared).First();
                WarpPawn(user, closestAnchor.Position, closestAnchor.Map);
                Messages.Message($"Warping to Dimension Anchor at {closestAnchor.Position} on map {closestAnchor.Map}", MessageTypeDefOf.PositiveEvent);
            }
            else
            {
                Messages.Message("No Dimension Anchor found!", MessageTypeDefOf.RejectInput);
            }
        }

        private List<Thing> FindAllDimensionAnchors()
        {
            List<Thing> allAnchors = new List<Thing>();

            foreach (var map in Find.Maps)
            {
                var anchors = map.listerThings.ThingsOfDef(HE_ThingDefOf.AR_DimensionAnchor);
                List<Thing> allowAnchors = new List<Thing>();
                foreach (var item in anchors)
                {
                    var comp = item.TryGetComp<CompResourceTrader>();
                    if (comp != null && comp.ResourceOn)
                    {
                        allowAnchors.Add(item);
                    }
                    if (comp == null)
                    {
                        Messages.Message("Anchor dosen't have CompWarpSystem.", MessageTypeDefOf.RejectInput);
                    }
                }
                allAnchors.AddRange(allowAnchors);
            }

            return allAnchors;
        }

        private void WarpPawn(Pawn pawn, IntVec3 targetPosition, Map targetMap)
        {
            if (pawn.Map == targetMap)
            {
                // Check if the target position is valid
                if (targetPosition.InBounds(targetMap))
                {
                    pawn.Position = targetPosition;
                }
            }
            else
            {
                pawn.DeSpawn();
                GenPlace.TryPlaceThing(pawn, targetPosition, targetMap, ThingPlaceMode.Near);
                pawn.SpawnSetup(targetMap, true);
            }
        }
    }
}
