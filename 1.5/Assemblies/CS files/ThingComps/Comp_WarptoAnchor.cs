using PipeSystem;
using RimWorld;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Noise;

namespace HE_AntiReality
{

    public class Comp_WarptoAnchor : Comp_FEUserBase
    {
        // フィールド
        private Pawn userPawn;
        private StringBuilder sb;

        // プロパティ
        public Pawn UserPawn => userPawn; // 次元帰還を使うポーン
        
        private CompProperties_WarptoAnchor Props => (CompProperties_WarptoAnchor)props;

        // メソッド
        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
        }
        public override IEnumerable<Gizmo> CompGetWornGizmosExtra()
        {
            sb = new StringBuilder();
            sb.AppendLine("");
            sb.AppendLine($"貯蓄FE: {StoreFE.CurrentFE.ToString("0.00")}fu");
            sb.AppendLine($"必要FE: {StoreFE.RequiredFE.ToString("0.00")}fu");
            Command_Action teleportCommand = new Command_Action
            {
                defaultLabel = "次元帰還",
                defaultDesc = "任意のディメンションアンカーへ移動する。" + sb.ToString(),
                icon = ContentFinder<Texture2D>.Get("Things/Item/ZeroDimensionStone"),
                action = () =>
                {
                    if (parent.ParentHolder is Pawn_ApparelTracker pawn_ApparelTracker)
                    {
                        userPawn = pawn_ApparelTracker.pawn;
                    }
                    if (!IsCooldownComplete())
                    {
                        Messages.Message("クールダウン中。あと " + GetRemainingCooldown().ToString("0.0") + " 秒", MessageTypeDefOf.RejectInput, false);
                    }
                    else if (!StoreFE.CanUseEffect)
                    {
                        Messages.Message($"転移に必要なFEが不足しています。必要FE：{StoreFE.RequiredFE.ToString("0.00")}fu", MessageTypeDefOf.RejectInput);
                    }
                    else
                    {
                        if (WarpAnchorHelper.FindAllowAnchors(out var anchors))
                        {
                            Find.WindowStack.Add(new Window_SelectDimensionAnchor(anchors, this));
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
                teleportCommand.Disable("使用可能まであと " + GetRemainingCooldown().ToString("0.0") + " 秒");
            }
            if (!StoreFE.CanUseEffect)
            {
                teleportCommand.Disable("FE不足。");
            }
            yield return teleportCommand;

            var tmp = InspectorGizmo();
            if (tmp != null)
            {
                foreach (var command in tmp)
                {
                    if (command != null)
                    {
                        yield return command;
                    }
                }
            }
        }
        public override void Notify_Equipped(Pawn pawn)
        {
            userPawn = pawn;
        }

        public void WarpPawn(Pawn pawn, Thing anchor)
        {
            countTick = Props.cooldownTicks;
            // 現在のジョブを中断させる
            pawn.jobs.StopAll(); // 全てのジョブを停止する
            pawn.jobs.ClearQueuedJobs(); // キューに入っているジョブもクリアする

            // ワープ処理
            Map targetMap = anchor.Map;
            IntVec3 targetPosition = anchor.Position;

            if (targetPosition.InBounds(targetMap) && pawn.Map == targetMap)
            {
                pawn.Position = targetPosition;
            }
            else
            {
                pawn.DeSpawn();
                GenPlace.TryPlaceThing(pawn, targetPosition, targetMap, ThingPlaceMode.Near);
                pawn.SpawnSetup(targetMap, true);
            }
            // ポーンを指定位置で待機状態にする
            pawn.jobs.StartJob(new Job(JobDefOf.Wait, 36, true), JobCondition.InterruptForced);
        }

    }

    public class Window_SelectDimensionAnchor : Window
    {
        private readonly List<Thing> dimensionAnchors;
        private readonly Comp_WarptoAnchor warptoAnchor;
        private Vector2 scrollPosition = Vector2.zero; // スクロール位置の管理

        private const float ButtonHeight = 30f;
        private const float Spacing = 10f;
        private const float ScrollBarWidth = 16f; // スクロールバーの幅

        public Window_SelectDimensionAnchor(List<Thing> anchors, Comp_WarptoAnchor warptoAnchor)
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
            Text.Font = GameFont.Medium;
            Widgets.Label(new Rect(0f, 15f, inRect.width, 30f), "移動先のアンカーを選択");

            Text.Font = GameFont.Small; // フォントを小さくしてボタンのテキストに適応

            // スクロールビューの設定
            float viewHeight = (ButtonHeight + Spacing) * dimensionAnchors.Count; // コンテンツの総高さ
            Rect outRect = new Rect(inRect.x, 50f, inRect.width, inRect.height - 65f); // スクロール可能な外部領域
            Rect viewRect = new Rect(0f, 0f, inRect.width - ScrollBarWidth, viewHeight); // 内部コンテンツの領域

            Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect); // スクロール開始

            float currentY = 0f;
            for (int i = 0; i < dimensionAnchors.Count; i++)
            {
                Rect buttonRect = new Rect(0f, currentY, viewRect.width, ButtonHeight);
                if (Widgets.ButtonText(buttonRect, MakeName(dimensionAnchors[i], i)))
                {
                    // Perform warp using the UserPawn stored in WarptoAnchor
                    if (warptoAnchor.UserPawn != null)
                    {
                        warptoAnchor.WarpPawn(warptoAnchor.UserPawn, dimensionAnchors[i]);
                        warptoAnchor.StoreFE.UseItemSkills();
                        Close();
                    }
                    else
                    {
                        Messages.Message("No suitable pawn found for warping.", MessageTypeDefOf.RejectInput);
                    }
                }
                currentY += ButtonHeight + Spacing;
            }

            Widgets.EndScrollView(); // スクロール終了
        }

        public string MakeName(Thing anchor, int idx)
        {
            var warpSystem = anchor.TryGetComp<CompWarpSystem>();
            if (warpSystem != null && warpSystem.CustamLabel != null)
            {
                return warpSystem.CustamLabel;
            }
            else
            {
                return anchor.Label + idx.ToString();
            }
        }
    }

}
