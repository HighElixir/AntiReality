using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace HE_AntiReality
{
    public static class TargetingParameters_Extensions
    {
        // Pawnに対してテレポートのターゲティングパラメーターを設定するメソッド
        public static TargetingParameters ForTeleport(Pawn p, float maxDistance)
        {
            return new TargetingParameters
            {
                canTargetLocations = true, // ロケーションをターゲット可能にする
                canTargetSelf = false, // 自分自身はターゲットにできないようにする
                validator = delegate (TargetInfo target)
                {
                    // セルがスタンド可能か、霧に覆われていないか、歩行可能かをチェック
                    if (!target.Cell.Standable(target.Map) || target.Cell.Fogged(target.Map) || !target.Cell.Walkable(target.Map))
                    {
                        return false; // スタンド不可、霧に覆われている、または歩行不可のセルは無効
                    }

                    // セルに壁（建物）がないことを確認
                    Building building = target.Cell.GetEdifice(target.Map);
                    if (building != null && building.def.category == ThingCategory.Building && building.def.building.isEdifice)
                    {
                        return false; // 壁が存在する場合、そのセルは無効
                    }

                    // 距離制限をチェック
                    float distance = (target.Cell - p.Position).LengthHorizontal;
                    if (distance > maxDistance)
                    {
                        return false; // 最大距離を超えている場合は無効
                    }

                    // 視界が通るかどうかをチェック
                    if (!GenSight.LineOfSight(p.Position, target.Cell, target.Map))
                    {
                        return false; // 視界が通らないセルは無効
                    }

                    return true; // すべての条件を満たしていれば有効
                }
            };
        }
    }
}
