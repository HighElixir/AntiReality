using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace HE_AntiReality
{
    // FE（Fiction Energy）を使用するベースクラス
    public abstract class Comp_FEUserBase : ThingComp
    {
        // FEのストレージコンポーネントを保持
        public Comp_StoreFE StoreFE { get; private set; }

        // 最後にスキルを使用したTick数（クールダウン管理用）
        public int countTick;

        // 初期化メソッド。CompPropertiesの初期設定を行う
        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);

            // 親アイテムからComp_StoreFEコンポーネントを取得
            StoreFE = parent.GetComp<Comp_StoreFE>();
            if (StoreFE == null)
            {
                // エラーログ。Comp_StoreFEが存在しない場合
                Log.Error($"DefName:{parent.def.defName} に CompProperties_StoreFE が設定されていません。");
            }
        }

        // 毎ティック（Tick）呼ばれるメソッド
        public override void CompTick()
        {
            base.CompTick();

            // クールダウンが進行中なら、カウントを減らす
            if (countTick > 0)
            {
                countTick--;
            }
        }

        // クールダウンが完了しているかどうかを確認するメソッド
        public bool IsCooldownComplete() => countTick <= 0; // クールダウンが0以下で完了


        // クールダウンの残り時間を取得するメソッド（秒単位で返す）
        public float GetRemainingCooldown() => countTick / 60f; // Tickを秒に変換

        // デフォルトの説明文とアイコン
        string defaultDesc = "FEをセット：";
        Texture2D icon;

        // インスペクタに表示されるGizmo（デバッグ用）を返すメソッド
        public List<Command_Action> InspectorGizmo()
        {
            // アイコンをロード（初回のみ）
            if (icon == null)
            {
                icon = ContentFinder<Texture2D>.Get(HE_Constants.debugGizmoIconPath);
            }

            // StoreFEが存在し、デバッグモードの場合のみGizmoを表示
            if (StoreFE != null && DebugSettings.godMode)
            {
                // コマンドリストを作成
                List<Command_Action> actions = new List<Command_Action>();

                // FEを100%に設定するアクション
                actions.Add(new Command_Action
                {
                    defaultLabel = "100%",
                    defaultDesc = defaultDesc + "100%",
                    icon = icon,
                    action = () =>
                    {
                        StoreFE.CurrentFE = StoreFE.MaxFE; // FEを最大値に設定
                    }
                });

                // FEを50%に設定するアクション
                actions.Add(new Command_Action
                {
                    defaultLabel = "50%",
                    defaultDesc = defaultDesc + "50%",
                    icon = icon,
                    action = () =>
                    {
                        StoreFE.CurrentFE = StoreFE.MaxFE * 0.5f; // FEを50%に設定
                    }
                });

                // FEを0%に設定するアクション
                actions.Add(new Command_Action
                {
                    defaultLabel = "0%",
                    defaultDesc = defaultDesc + "0%",
                    icon = icon,
                    action = () =>
                    {
                        StoreFE.CurrentFE = 0; // FEを0に設定
                    }
                });

                // 作成したアクションリストを返す
                return actions;
            }

            // 条件が合わない場合はnullを返す
            return null;
        }

        public string GetExposeKey(string exposeKey) => $"HE_{nameof(Comp_FEUserBase)}_{exposeKey}";
        public override void PostExposeData()
        {
            Scribe_Values.Look(ref countTick, GetExposeKey(nameof(countTick)), 0);
        }
    }
}
