using Verse;
using RimWorld;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using PipeSystem;

namespace HE_AntiReality
{
    public class WarptoAnchor : ThingWithComps
    {
        private int countTick = 0; // 最後にテレポートを使用したゲーム内時間（Tick）
        private const int CooldownTicks = 6000; // クールダウンの時間（6000 ticks = 100秒）
        private const int RequiredHitPoints = 11; // 必要なヒットポイント
        public override void Tick()
        {
            base.Tick();
            if (countTick > 0)
            {
                countTick--;
            }
        }
        public override IEnumerable<Gizmo> GetGizmos()
        {
            Command_Action teleportCommand = new Command_Action
            {
                defaultLabel = "次元帰還",
                defaultDesc = "任意のディメンションアンカーへ移動する。",
                icon = ContentFinder<Texture2D>.Get("Things/Item/ZeroDimensionStone"),
                action = () =>
                {
                    if (countTick > 0)
                    {
                        Messages.Message("クールダウン中。あと " + ((float)countTick / 60f).ToString("0.0") + " 秒", MessageTypeDefOf.RejectInput, false);
                    }
                    else if (HitPoints < RequiredHitPoints)
                    {
                        Messages.Message("転移に必要なFEが不足しています。", MessageTypeDefOf.RejectInput);
                    }
                    else
                    {
                        var anchors = FindAllDimensionAnchors();
                        if (anchors != null && anchors.Count > 0)
                        {
                            Find.WindowStack.Add(new Window_SelectDimensionAnchor(anchors, this));
                            countTick = CooldownTicks;
                        }
                        else
                        {
                            Messages.Message("利用可能なディメンションアンカーが見つかりませんでした。", MessageTypeDefOf.RejectInput);
                        }
                    }
                }
            };

            if (countTick != 0)
            {
                teleportCommand.Disable("使用可能まであと " + ((float)countTick / 60f).ToString("0.0") + " 秒");
            }
            if (HitPoints < RequiredHitPoints)
            {
                teleportCommand.Disable("FE不足。");
            }
            yield return teleportCommand;
        }
        private List<Thing> FindAllDimensionAnchors()
        {
            List<Thing> allAnchors = new List<Thing>();

            foreach (var map in Find.Maps)
            {
                var anchors = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("AR_DimensionAnchor"));
                List<Thing> allowAnchors = new List<Thing>();
                foreach (var item in anchors)
                {
                    var comp = item.TryGetComp<CompResourceTrader>();
                    if (comp != null && comp.ResourceOn)
                    {
                        allowAnchors.Add(item);
                    }
                    else if (comp == null)
                    {
                        Messages.Message("Anchor doesn't have CompResourceTrader.", MessageTypeDefOf.RejectInput);
                    }
                }
                allAnchors.AddRange(allowAnchors);
            }

            return allAnchors;
        }
        public void WarpPawn(Pawn pawn, IntVec3 targetPosition, Map targetMap)
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

    public class Window_SelectDimensionAnchor : Window
    {
        private readonly List<Thing> dimensionAnchors;
        private readonly WarptoAnchor warptoAnchor;
        public Window_SelectDimensionAnchor(List<Thing> anchors, WarptoAnchor warptoAnchor)
        {
            dimensionAnchors = anchors;
            this.warptoAnchor = warptoAnchor;

            doCloseButton = true;
            forcePause = true;
            absorbInputAroundWindow = true;
            closeOnClickedOutside = true;
            resizeable = false;
            draggable = true;
        }
        public override void DoWindowContents(Rect inRect)
        {
            float buttonHeight = 30f;
            float spacing = 10f;
            float currentY = 0f;

            foreach (var anchor in dimensionAnchors)
            {
                Rect buttonRect = new Rect(0, currentY, inRect.width, buttonHeight);
                if (Widgets.ButtonText(buttonRect, anchor.Label))
                {
                    var anchorPosition = anchor.Position;
                    var anchorMap = anchor.Map;
                    // Perform warp
                    var pawn = Find.CurrentMap.mapPawns.FreeColonists.FirstOrDefault(); // You need to define which pawn to warp
                    if (pawn != null)
                    {
                        warptoAnchor.WarpPawn(pawn, anchorPosition, anchorMap);
                        Close();
                    }
                    else
                    {
                        Messages.Message("No suitable pawn found for warping.", MessageTypeDefOf.RejectInput);
                    }
                }
                currentY += buttonHeight + spacing;
            }
        }

    }
}
