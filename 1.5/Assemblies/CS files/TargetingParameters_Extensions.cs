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
        public static TargetingParameters ForTeleport(Pawn p, float maxDistance)
        {
            return new TargetingParameters
            {
                canTargetLocations = true,
                canTargetSelf = false,
                validator = delegate (TargetInfo target)
                {
                    // セルがスタンド可能か、霧に覆われていないか、歩行可能かをチェック
                    if (!target.Cell.Standable(target.Map) || target.Cell.Fogged(target.Map) || !target.Cell.Walkable(target.Map))
                    {
                        return false;
                    }

                    // セルに壁（建物）がないことを確認
                    Building building = target.Cell.GetEdifice(target.Map);
                    if (building != null && building.def.category == ThingCategory.Building && building.def.building.isEdifice)
                    {
                        return false; // 壁は選べないようにする
                    }

                    // 距離制限をチェック
                    float distance = (target.Cell - p.Position).LengthHorizontal;
                    if (distance > maxDistance)
                    {
                        return false; // 距離制限を超えている場合、選択不可
                    }

                    // 視界が通るかどうかをチェック
                    if (!GenSight.LineOfSight(p.Position, target.Cell, target.Map))
                    {
                        return false; // 視界が通っていない場合、選択不可
                    }

                    return true;
                }
            };
        }
    }
}


