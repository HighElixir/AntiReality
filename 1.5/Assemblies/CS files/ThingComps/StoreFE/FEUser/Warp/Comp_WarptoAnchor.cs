using PipeSystem;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
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
        private Pawn _userPawn;

        // プロパティ
        public Pawn UserPawn => _userPawn; // 次元帰還を使うポーン

        public CompProperties_WarptoAnchor Props => (CompProperties_WarptoAnchor)props;

        // メソッド
        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
        }
        public override IEnumerable<Gizmo> CompGetWornGizmosExtra()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("");
            sb.AppendLine($"貯蓄FE: {StoreFE.CurrentFE.ToString("0.00")}fu");
            sb.AppendLine($"必要FE: {StoreFE.RequiredFE.ToString("0.00")}fu");

            Command_ActionWithCooldown teleportCommand = new Command_ActionWithCooldown
            {
                cooldownPercentGetter = () =>
                {
                    return (Props.cooldownTicks - countTick) / Props.cooldownTicks;
                },
                defaultLabel = "次元帰還",
                defaultDesc = "任意のディメンションアンカーへ移動する。" + sb.ToString(),
                icon = ContentFinder<Texture2D>.Get("Things/Item/ZeroDimensionStone"),
                action = () =>
                {
                    if (parent.ParentHolder is Pawn_ApparelTracker pawn_ApparelTracker)
                    {
                        _userPawn = pawn_ApparelTracker.pawn;
                    }
                    if (!IsCooldownComplete())
                    {
                        Messages.Message("クールダウン中。あと " + GetRemainingCooldown().ToString("0.0") + " 秒", MessageTypeDefOf.RejectInput, false);
                    }
                    else if (!StoreFE.CanUseEffect())
                    {
                        Messages.Message($"転移に必要なFEが不足しています。必要FE：{StoreFE.RequiredFE.ToString("0.00")}fu", MessageTypeDefOf.RejectInput);
                    }
                    else
                    {
                        if (WarpAnchorHelper.CanUseAnchor(out var anchors, this))
                        {
                            Find.WindowStack.Add(new Window_SelectDimensionAnchor(this, anchors));
                        }
                        else
                        {
                            Messages.Message("利用可能なディメンションアンカーが見つかりませんでした。", MessageTypeDefOf.RejectInput);
                        }
                    }
                }
            };

            if (!IsCooldownComplete())
            {
                teleportCommand.Disable("使用可能まであと " + GetRemainingCooldown().ToString("0.0") + " 秒");
            }
            if (!StoreFE.CanUseEffect())
            {
                teleportCommand.Disable("FE不足。");
            }
            yield return teleportCommand;

            IEnumerator<Command_Action> enumerator = GetInspectorGizmos();
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }
        // デバッグ用のギズモを追加するための処理
        protected IEnumerator<Command_Action> GetInspectorGizmos()
        {
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
            yield break;
        }
        public override void Notify_Equipped(Pawn pawn)
        {
            _userPawn = pawn;
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
        private readonly Dictionary<Thing, float> _anchorsCost = new Dictionary<Thing, float>();
        private readonly List<Thing> _anchors = new List<Thing>();
        private readonly Comp_WarptoAnchor _warptoAnchor;
        private readonly Pawn _pawn;
        private Vector2 _scrollPosition = Vector2.zero; // スクロール位置の管理

        private const float BUTTON_HEIGHT = 30f;
        private const float SPACING = 10f;
        private const float SCROLL_BAR_WIDTH = 16f; // スクロールバーの幅

        public Window_SelectDimensionAnchor(Comp_WarptoAnchor warptoAnchor, Dictionary<Thing, float> anchors)
        {
            _anchorsCost = anchors;
            _anchors = _anchorsCost.Keys.ToList();
            _warptoAnchor = warptoAnchor;
            _pawn = warptoAnchor.UserPawn;

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
            float viewHeight = (BUTTON_HEIGHT + SPACING) * _anchorsCost.Count; // コンテンツの総高さ
            Rect outRect = new Rect(inRect.x, 50f, inRect.width, inRect.height - 65f); // スクロール可能な外部領域
            Rect viewRect = new Rect(0f, 0f, inRect.width - SCROLL_BAR_WIDTH, viewHeight); // 内部コンテンツの領域

            Widgets.BeginScrollView(outRect, ref _scrollPosition, viewRect); // スクロール開始

            float currentY = 0f;
            for (int i = 0; i < _anchorsCost.Count; i++)
            {
                Rect buttonRect = new Rect(0f, currentY, viewRect.width, BUTTON_HEIGHT);
                if (Widgets.ButtonText(buttonRect, MakeName(_anchors[i], i, _anchorsCost[_anchors[i]])))
                {
                    // Perform warp using the UserPawn stored in WarptoAnchor
                    if (_warptoAnchor.UserPawn != null)
                    {
                        _warptoAnchor.WarpPawn(_warptoAnchor.UserPawn, _anchors[i]);
                        _warptoAnchor.StoreFE.UseItemSkills();
                        Close();
                    }
                    else
                    {
                        Messages.Message("No suitable pawn found for warping.", MessageTypeDefOf.RejectInput);
                    }
                }
                currentY += BUTTON_HEIGHT + SPACING;
            }

            Widgets.EndScrollView(); // スクロール終了
        }

        private string MakeName(Thing anchor, int idx, float cost)
        {
            var warpSystem = anchor.TryGetComp<CompWarpSystem>();
            string str = string.Empty;
            if (warpSystem != null && warpSystem.CustomLabel != null)
            {
                str = warpSystem.CustomLabel;
            }
            else
            {
                str = anchor.Label + idx.ToString();
            }
            return $"{str} コスト：{cost}";
        }
    }

}
