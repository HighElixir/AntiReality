using UnityEngine;
using Verse;

namespace HighElixir.Utility.RimWorld
{
    public static class HE_RimUtility
    {
        public static Vector3 MakeMotePosition(Pawn target, float offsetXMin = -1.5f, float offsetXMax = 1.5f, float offsetZMin = -1.5f, float offsetZMax = 1.5f)
        {
            // ランダムなオフセットを生成（-1.5 から +1.5 の範囲）
            float offsetX = Rand.Range(offsetXMin, offsetXMax);
            float offsetZ = Rand.Range(offsetZMin, offsetZMax);

            // 新しい位置を計算
            Vector3 v = target.DrawPos + new Vector3(offsetX, 0, offsetZ);

            // マップ外に出ないように調整
            if (!v.ToIntVec3().InBounds(target.Map))
            {
                v = target.DrawPos;
            }
            return v;
        }
    }
}
