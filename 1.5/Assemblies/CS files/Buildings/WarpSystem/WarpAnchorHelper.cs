using System.Collections.Generic;
using Verse;
using PipeSystem;

namespace HE_AntiReality
{
    /// <summary>
    /// ディメンションアンカーのキャッシュ用クラス
    /// </summary>
    public static class WarpAnchorHelper
    {
        // フィールド
        private static bool dicDirty = true;
        private static Dictionary<Thing, CompResourceTrader> anchors = new Dictionary<Thing, CompResourceTrader>();
        private static List<Thing> things = new List<Thing>();
        private static List<CompResourceTrader> traders = new List<CompResourceTrader>();
        // プロパティ
        public static List<Thing> AllAnchors
        {
            get
            {
                if (dicDirty) UpdateLists();
                return things;
            }
        }
        public static List<CompResourceTrader> AllTraders
        {
            get
            {
                if (dicDirty) UpdateLists();
                return traders;
            }
        }

        // メソッド
        public static void AddAnchor(Thing thing)
        {
            if (anchors.ContainsKey(thing)) { return; }
            if (thing.def == HE_ThingDefOf.AR_DimensionAnchor && thing.TryGetComp<CompResourceTrader>(out var trader))
            {
                anchors.Add(thing, trader);
                dicDirty = true;
            }
        }

        public static void RemoveAnchor(Thing thing)
        {
            if (anchors.ContainsKey(thing))
            {
                anchors.Remove(thing);
                dicDirty = true;
            }
        }
        private static void UpdateLists()
        {
            things = Helpers.DictionaryKeysToList(anchors);
            traders = Helpers.DictionaryToList(anchors);
            dicDirty = false;
        }

        public static bool FindAllowedAnchors(out List<Thing> result)
        {
            result = new List<Thing>();
            if (anchors.Count > 0)
            {
                foreach (var anchor in anchors)
                {
                    // アンカーが設置されているか（スポーンされているか）を確認
                    if (!anchor.Key.Spawned)
                    {
                        continue; // 設置されていない場合、スキップ
                    }
                    if (anchor.Value != null && anchor.Value.ResourceOn)
                    {
                        result.Add(anchor.Key); // アンカーが稼働中ならリストに追加
                    }
                }

                return result.Count > 0;
            }
            return false;
        }

        /// <summary>
        /// 距離に応じてコストを決定する
        /// 研究の進み具合で低減される（未実装）
        /// Note：現状は元のコストがそのまま返される
        /// </summary>
        /// <param name="anchorCostPairs"></param>
        /// <param name="anchors"></param>
        /// <returns></returns>
        public static bool CanUseAnchor(out Dictionary<Thing, float> anchorCostPairs, Comp_WarptoAnchor warptoAnchor)
        {
            anchorCostPairs = new Dictionary<Thing, float>();
            if(!FindAllowedAnchors(out var anchors))
            {
                return false;
            }
            bool res = false;
            IntVec3 vec3 = warptoAnchor.UserPawn.Position;
            foreach (Thing t in anchors)
            {
                float f = warptoAnchor.StoreFE.RequiredFE;
                anchorCostPairs[t] = f;
                if (f <= warptoAnchor.StoreFE.CurrentFE)
                {
                    res = true;
                }
            }

            return res;
        }
    }
}
