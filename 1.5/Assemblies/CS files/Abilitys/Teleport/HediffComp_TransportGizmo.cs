using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;
using System;

namespace HE_AntiReality
{
    public class HediffComp_TransportGizmo : HediffComp
    {
        private int countTick = 0; // 最後にテレポートを使用したゲーム内時間（Tick）を記録
        private const int CooldownTicks = 6000; // クールダウンの時間（6000 ticks = 約100秒）
        private const float MaxTeleportDistance = 20f; // テレポートの最大距離（20タイル）

        // HediffCompProperties_TransportGizmo クラスのプロパティを取得
        public HediffCompProperties_TransportGizmo Props
        {
            get
            {
                return (HediffCompProperties_TransportGizmo)props;
            }
        }

        // ギズモ（インターフェース上のボタンなど）を取得するメソッド
        public override IEnumerable<Gizmo> CompGetGizmos()
        {
            // デバッグ設定で神モードの場合、クールダウンをリセット
            if (DebugSettings.godMode)
            {
                countTick = 0;
            }

            // テレポートコマンドを作成
            Command_Action teleportCommand = new Command_Action
            {
                defaultLabel = "座標変換", // コマンドのラベル
                defaultDesc = "視界の通る範囲内に座標を変える", // コマンドの説明
                icon = ContentFinder<Texture2D>.Get("UI/Commands/Teleport"), // テレポートコマンドのアイコンを指定
                action = () =>
                {
                    // クールダウン中の場合、メッセージを表示して処理を終了
                    if (countTick > 0)
                    {
                        Messages.Message("クールダウン中。あと " + ((float)countTick / 60f).ToString("0.0") + " 秒", MessageTypeDefOf.RejectInput, false);
                    }
                    else
                    {
                        // テレポートターゲットの設定を開始
                        TeleportTargetingSource targetingSource = new TeleportTargetingSource(parent.pawn, MaxTeleportDistance);

                        // ターゲット選択プロセスを開始
                        Find.Targeter.BeginTargeting(targetingSource.targetParams, OnTargetSelected(targetingSource));
                    }
                }
            };

            // クールダウン中はコマンドを無効化
            if (countTick != 0)
            {
                teleportCommand.Disable("使用可能まであと " + ((float)countTick / 60f).ToString("0.0") + " 秒");
            }

            // 作成したコマンドをギズモのリストに追加
            yield return teleportCommand;
        }

        // ターゲットが選択された際の処理を定義するメソッド
        private Action<LocalTargetInfo> OnTargetSelected(TeleportTargetingSource targetingSource)
        {
            return target =>
            {
                // ターゲットが有効であるかを確認
                if (targetingSource.ValidateTarget(target))
                {
                    // ターゲットにテレポートを命令する
                    targetingSource.OrderForceTarget(target);
                    // クールダウン時間をリセット
                    countTick = CooldownTicks;
                }
            };
        }

        // 毎ティック（ゲーム内時間の単位）ごとに呼ばれるメソッド
        public override void CompPostTick(ref float severityAdjustment)
        {
            // クールダウンが進行中であれば、カウントを減らす
            if (countTick > 0)
            {
                countTick--;
            }
            // クールダウンが負の値にならないように調整
            if (countTick < 0)
            {
                countTick = 0;
            }
        }
    }
}