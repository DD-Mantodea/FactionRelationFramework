using FactionRelationFramework.Extensions;
using FactionRelationFramework.Utils;
using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Verse;

namespace FactionRelationFramework.Patches.FactionPatches
{
    [StaticConstructorOnStartup]
    public static class FactionUIUtilityPatch
    {
        static FactionUIUtilityPatch()
        {
            var harmony = FactionRelationFramework.Harmony;

            harmony.Patch(typeof(FactionUIUtility).GetMethod("DrawFactionRow", BindingFlags.NonPublic | BindingFlags.Static),
                prefix: new HarmonyMethod(typeof(FactionUIUtilityPatch), nameof(PreDrawFactionRow)), 
                postfix: new HarmonyMethod(typeof(FactionUIUtilityPatch), nameof(PostDrawFactionRow)),
                transpiler: new HarmonyMethod(typeof(FactionUIUtilityPatch), nameof(DrawFactionRowTranspiler)));
        }

        public static bool PreDrawFactionRow(ref float __result, Faction faction, float rowY, Rect fillRect)
        {
            var relationKind = faction.IsPlayer ? -1 : (int)faction.PlayerRelationKind;

            if(FactionRelationUtils.TryGetCustomFactionRelationKind(relationKind, out var custom))
            {
                return custom.PreDrawFactionRow(ref __result, faction, rowY, fillRect);
            }

            return true;
        }

        public static void PostDrawFactionRow(ref float __result, Faction faction, float rowY, Rect fillRect)
        {
            var relationKind = faction.IsPlayer ? -1 : (int)faction.PlayerRelationKind;

            if (FactionRelationUtils.TryGetCustomFactionRelationKind(relationKind, out var custom))
            {
                custom.PostDrawFactionRow(ref __result, faction, rowY, fillRect);
            }
        }

        public static IEnumerable<CodeInstruction> DrawFactionRowTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator ilg)
        {
            var codes = new List<CodeInstruction>(instructions);

            Label l = ilg.DefineLabel();

            Label endSwitch = ilg.DefineLabel();

            return codes.SelectMany(code =>
            {
                var ret = new List<CodeInstruction>();

                if (code.opcode == OpCodes.Br && codes.TryGetLast(code, out var last) && last.opcode == OpCodes.Switch)
                {
                    endSwitch = (Label)code.operand;

                    ret.Add(new(OpCodes.Br, l));
                }
                else if (code.opcode == OpCodes.Stloc_S && codes.TryGetAfter(code, 2, out var after) && after.OperandIs("\n\n") && (code.operand as LocalBuilder).LocalIndex == 22)
                {
                    ret.Add(code);

                    ret.Add(new(OpCodes.Br_S, endSwitch));

                    ret.Add(new CodeInstruction(OpCodes.Ldarg_0) { labels = [l] });

                    ret.Add(new(OpCodes.Call, typeof(FactionUIUtilityPatch).GetMethod(nameof(GetFactionRelationKindTip))));

                    ret.Add(code);
                }
                else ret.Add(code);

                return ret;
            });
        }

        public static string GetFactionRelationKindTip(Faction faction)
        {
            var relationKind = faction.IsPlayer ? -1 : (int)faction.PlayerRelationKind;

            if (FactionRelationUtils.TryGetCustomFactionRelationKind(relationKind, out var custom))
            {
                return custom.Tip(faction);
            }

            return "";
        }
    }
}
