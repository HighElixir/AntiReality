using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;
using System;

namespace HE_AntiReality
{
    public class HediffComp_TransportGizmo : HediffComp
    {
        private int countTick = 0; // 最後にテレポートを使用したゲーム内時間（Tick）
        private const int CooldownTicks = 6000; // クールダウンの時間（600 ticks = 10秒）
        private const float MaxTeleportDistance = 20f; // テレポートの最大距離

        public HediffCompProperties_TransportGizmo Props
        {
            get
            {
                return (HediffCompProperties_TransportGizmo)props;
            }
        }

        public override IEnumerable<Gizmo> CompGetGizmos()
        {
            Command_Action teleportCommand = new Command_Action
            {
                defaultLabel = "座標替え",
                defaultDesc = "視界の通る範囲内に座標を変える",
                icon = ContentFinder<Texture2D>.Get("UI/Commands/Teleport"),
                action = () =>
                {
                    if (countTick > 0)
                    {
                        Messages.Message("クールダウン中。あと " + ((float)countTick / 60f).ToString("0.0") + " 秒", MessageTypeDefOf.RejectInput, false);
                    }
                    else
                    {
                        TeleportTargetingSource targetingSource = new TeleportTargetingSource(parent.pawn, MaxTeleportDistance);

                        Find.Targeter.BeginTargeting(targetingSource.targetParams, OnTargetSelected(targetingSource));
                    }
                }
            };

            // クールダウン中は無効化
            if (countTick != 0)
            {
                teleportCommand.Disable("クールダウン中です");
            }

            yield return teleportCommand;
        }

        private Action<LocalTargetInfo> OnTargetSelected(TeleportTargetingSource targetingSource)
        {
            return target =>
            {
                if (targetingSource.ValidateTarget(target))
                {
                    targetingSource.OrderForceTarget(target);
                    countTick = CooldownTicks; // 使用時間を更新
                }
            };
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            if (countTick > 0)
            {
                countTick--;
            }
            if (countTick < 0)
            {
                countTick = 0;
            }
        }
    }
}
