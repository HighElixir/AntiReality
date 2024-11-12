using LudeonTK;
using RimWorld;
using UnityEngine;
using Verse;

namespace HE_AntiReality
{
    public class DebugActions_BathedInEnergy
    {
        [DebugAction("HE AntiReality", "Bathed in Energy Explosion", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void BathedInEnergyExplosion()
        {
            IntVec3 position = UI.MouseCell();
            Map map = Find.CurrentMap;
            if (map != null && position.InBounds(map))
            {
                GenExplosion.DoExplosion(
                    center: position,
                    map: map,
                    radius: 3.5f, // 爆発半径を指定
                    damType: HE_DamageDefOf.Bathed_in_Energy,
                    instigator: null,
                    damAmount: -1,
                    armorPenetration: -1,
                    explosionSound: HE_SoundDefOf.Bathed_in_Energy_Sound, // 適切なサウンドを指定
                    weapon: null,
                    projectile: null,
                    intendedTarget: null,
                    postExplosionSpawnThingDef: null,
                    postExplosionSpawnChance: 0f,
                    postExplosionSpawnThingCount: 0,
                    applyDamageToExplosionCellsNeighbors: false,
                    chanceToStartFire: 0f,
                    damageFalloff: false,
                    direction: null,
                    ignoredThings: null,
                    doVisualEffects: true,
                    propagationSpeed: 1.0f,
                    excludeRadius: 0f
                );
                Messages.Message("Bathed in Energy explosion triggered.", MessageTypeDefOf.TaskCompletion, false);
            }
            else
            {
                Messages.Message("Invalid position for explosion.", MessageTypeDefOf.RejectInput, false);
            }
        }
    }
}
