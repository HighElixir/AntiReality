using HE_AntiReality;
using System.Collections.Generic;
using System;
using UnityEngine;
using Verse;
using RimWorld;

public class HediffComp_TransportGizmo : HediffComp
{
    private int countTick = 0;
    private const int CooldownTicks = 6000;
    private const float MaxTeleportDistance = 20f;

    public HediffCompProperties_TransportGizmo Props
    {
        get
        {
            return (HediffCompProperties_TransportGizmo)props;
        }
    }

    public override IEnumerable<Gizmo> CompGetGizmos()
    {
        if (DebugSettings.godMode)
        {
            countTick = 0;
        }
        Command_Action teleportCommand = new Command_Action
        {
            defaultLabel = "座標変換",
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
            },
            onHover = () =>
            {
                GenDraw.DrawRadiusRing(parent.pawn.Position, MaxTeleportDistance);
            }
        };

        if (countTick != 0)
        {
            teleportCommand.Disable("使用可能まであと " + ((float)countTick / 60f).ToString("0.0") + " 秒");
        }

        yield return teleportCommand;
    }

    private Action<LocalTargetInfo> OnTargetSelected(TeleportTargetingSource targetingSource)
    {
        GenDraw.DrawRadiusRing(parent.pawn.Position, MaxTeleportDistance);
        return target =>
        {
            if (!targetingSource.ValidateTarget(target))
            {
                Messages.Message("無効なターゲットです。", MessageTypeDefOf.RejectInput, false);
                return;
            }

            targetingSource.OrderForceTarget(target);
            countTick = CooldownTicks;
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
