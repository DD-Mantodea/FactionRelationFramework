using RimWorld;
using RimWorld.Planet;
using System;
using System.Reflection;
using Verse;

namespace FactionRelationFramework.Patches.FactionPatches.Custom
{
    public abstract class CustomFaction : Faction
    {
        public CustomFaction() { }

        public CustomFaction(Faction faction) 
        {
            LoadFrom(faction);
        }

        public virtual CustomFaction LoadFrom(Faction faction)
        {
            foreach (var factionP in typeof(Faction).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var customP = typeof(CustomFaction).GetProperty(factionP.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                if (customP != null && customP.CanWrite && factionP.GetValue(faction) != null)
                    customP.SetValue(this, factionP.GetValue(faction));
            }

            foreach (var factionF in typeof(Faction).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var customF = typeof(CustomFaction).GetField(factionF.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                if (customF != null && factionF.GetValue(faction) != null)
                    customF.SetValue(this, factionF.GetValue(faction));
            }

            return this;
        }

        public virtual bool PreCheckKindThresholds(ref FactionRelation relation, bool canSendLetter, string reason, GlobalTargetInfo lookTarget, out bool sentLetter) { sentLetter = false; return true; }

        public virtual bool PreTryAffectGoodwillWith(ref bool __result, Faction other, int goodwillChange, bool canSendMessage = true, bool canSendHostilityLetter = true, HistoryEventDef reason = null, GlobalTargetInfo? lookTarget = null) { return true; }
    
        public virtual void Notify_CustomRelationKindChanged(Faction other, FactionRelationKind previousKind, FactionRelationKind kind, bool canSendLetter, string reason, GlobalTargetInfo lookTarget, out bool sentLetter) { sentLetter = false; }
    }
}
