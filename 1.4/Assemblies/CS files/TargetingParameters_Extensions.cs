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
        public static TargetingParameters ForTeleport()
        {
            return new TargetingParameters
            {
                canTargetLocations = true,
                canTargetSelf = false,
                validator = delegate (TargetInfo target)
                {
                    return target.Cell.Standable(target.Map) && !target.Cell.Fogged(target.Map) && target.Cell.Walkable(target.Map);
                }
            };
        }
    }
}
