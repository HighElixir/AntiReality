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
        public override void PostExposeData()
        {
            Scribe_Values.Look(ref countTick, ExposeUtility.GetExposeKey(typeof(Comp_FEUserBase), nameof(countTick)), 0);
        }
    }
}
