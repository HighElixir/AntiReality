using PipeSystem;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace HE_AntiReality
{
    public class Comp_FEUserBase : ThingComp
    {
        public Comp_StoreFE StoreFE { get; private set; }
        public int countTick; // 最後にテレポートを使用したゲーム内時間（Tick）

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);

            // Comp_StoreFE コンポーネントを取得
            StoreFE = parent.GetComp<Comp_StoreFE>();
            if (StoreFE == null)
            {
                Log.Error($"DefName:{parent.def.defName} に CompProperties_StoreFE が設定されていません。");
            }
        }

        public override void CompTick()
        {
            base.CompTick();
            if (countTick > 0)
            {
                countTick--;
            }
        }

        // クールダウンが完了したかどうかを確認するメソッド
        public bool IsCooldownComplete()
        {
            return countTick <= 0;
        }

        // クールダウンの残り時間を取得するメソッド
        public float GetRemainingCooldown()
        {
            return countTick / 60f; // 秒に変換
        }

        string defaultDesc = "FEをセット：";
        Texture2D icon;
        public List<Command_Action> InspectorGizmo()
        {
            if (icon == null)
            {
                icon = ContentFinder<Texture2D>.Get("UI/Commands/skill_icon_01_nobg");
            }
            if (StoreFE != null && DebugSettings.godMode)
            {
                List<Command_Action> actions = new List<Command_Action>();
                actions.Add(new Command_Action
                {
                    defaultLabel = "100%",
                    defaultDesc = defaultDesc + "100%",
                    icon = icon,
                    action = () =>
                    {
                        StoreFE.CurrentFE = StoreFE.MaxFE;
                    }

                });
                actions.Add(new Command_Action
                {
                    defaultLabel = "50%",
                    defaultDesc = defaultDesc + "50%",
                    icon = icon,
                    action = () =>
                    {
                        StoreFE.CurrentFE = StoreFE.MaxFE * 0.5f;
                    }

                });
                actions.Add(new Command_Action
                {
                    defaultLabel = "0%",
                    defaultDesc = defaultDesc + "0%",
                    icon = icon,
                    action = () =>
                    {
                        StoreFE.CurrentFE = 0;
                    }

                });
                return actions;
            }
            return null;
        }
    }
}
