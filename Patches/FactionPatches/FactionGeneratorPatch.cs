using FactionRelationFramework.Content.Factions;
using FactionRelationFramework.Content.Factions.DefExtensions;
using FactionRelationFramework.Patches.FactionPatches.Custom;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace FactionRelationFramework.Patches.FactionPatches
{
    [StaticConstructorOnStartup]
    public static class FactionGeneratorPatch
    {
        static FactionGeneratorPatch()
        {
            var harmony = FactionRelationFramework.Harmony;

            harmony.Patch(AccessTools.Method(typeof(FactionGenerator), nameof(FactionGenerator.NewGeneratedFaction), [typeof(PlanetLayer), typeof(FactionGeneratorParms)]), 
                transpiler: new HarmonyMethod(typeof(FactionGeneratorPatch), nameof(NewGeneratedFactionTranspiler)));
        }

        public static IEnumerable<CodeInstruction> NewGeneratedFactionTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            return codes.SelectMany(code =>
            {
                var ret = new List<CodeInstruction>();

                if (code.opcode == OpCodes.Newobj && code.OperandIs(typeof(Faction).GetConstructor([])))
                {
                    ret.Add(new(OpCodes.Ldarg_1));

                    ret.Add(new(OpCodes.Call, typeof(FactionGeneratorPatch).GetMethod(nameof(GenerateFaction))));
                }
                else ret.Add(code);

                return ret;
            });
        }

        public static Faction GenerateFaction(FactionGeneratorParms parms)
        {
            var def = parms.factionDef;

            var clazz = def.GetModExtension<FactionDefExtension>()?.factionClass;

            if (clazz != null && CustomFactionManager.CustomFactions.Keys.Contains(clazz))
            {
                return Activator.CreateInstance(CustomFactionManager.CustomFactions[clazz]) as Faction;
            }
            else return new CommonCustomFaction();
        }
    }
}
