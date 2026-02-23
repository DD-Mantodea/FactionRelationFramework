using RimWorld.Planet;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace FactionRelationFramework.Utils
{
    public class ThingUtils
    {
        public static Faction GetThingFaction(Thing thing)
        {
            if (thing.Faction != null)
                return thing.Faction;

            if (thing.ParentHolder is Pawn holder)
                return holder.Faction;

            if (thing.Map != null)
            {
                if (thing.Map.Parent is Settlement settlement1)
                    return settlement1.Faction;

                Faction territoryFaction = thing.Map.ParentFaction;
                if (territoryFaction != null)
                    return territoryFaction;
            }

            if (thing.ParentHolder is Pawn prisoner && prisoner.IsPrisoner)
                return prisoner.HostFaction;

            if (thing.ParentHolder is Pawn_CarryTracker carryTracker)
                return carryTracker.pawn.Faction;

            if (TradeSession.trader is Settlement settlement2)
                return settlement2.Faction;

            return null;
        }
    }
}
