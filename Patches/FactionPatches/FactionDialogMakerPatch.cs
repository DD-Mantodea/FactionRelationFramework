using FactionRelationFramework.Patches.FactionPatches;
using HarmonyLib;
using RimWorld;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Reflection.Emit;
using Verse;
using FactionRelationFramework.Patches.FactionPatches.Custom;
using FactionRelationFramework.Utils;
using System.Linq;
using FactionRelationFramework.Extensions;
using System.Diagnostics;

namespace FactionRelationFramework.Patches
{
    [StaticConstructorOnStartup]
    public static class FactionDialogMakerPatch
    {
        static FactionDialogMakerPatch()
        {
            var harmony = FactionRelationFramework.Harmony;

            harmony.Patch(AccessTools.Method(typeof(FactionDialogMaker), nameof(FactionDialogMaker.FactionDialogFor)), 
                transpiler: new(typeof(FactionDialogMakerPatch), nameof(FactionDialogForTranspiler)));

            harmony.Patch(typeof(FactionDialogMaker).GetMethod("RequestMilitaryAidOption", BindingFlags.NonPublic | BindingFlags.Static),
                transpiler: new(typeof(FactionDialogMakerPatch), nameof(RequestMilitaryAidOptionTranspiler)));

            harmony.Patch(typeof(FactionDialogMaker).GetMethod("RequestOrbitalTraderOption", BindingFlags.NonPublic | BindingFlags.Static),
                transpiler: new(typeof(FactionDialogMakerPatch), nameof(RequestOrbitalTraderOptionTranspiler)));

            harmony.Patch(typeof(FactionDialogMaker).GetMethod("RequestTraderOption", BindingFlags.NonPublic | BindingFlags.Static),
                transpiler: new(typeof(FactionDialogMakerPatch), nameof(RequestTraderOptionTranspiler)));
        }

        public static IEnumerable<CodeInstruction> FactionDialogForTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator ilg)
        {
            var codesCache = new List<CodeInstruction>(instructions);

            var codes = instructions.ToList();

            var skipCustom = ilg.DefineLabel();

            var addedPatch = false;

            ilg.DeclareLocal(typeof(DiaNode));

            return codesCache.SelectMany(c =>
            {
                var ret = new List<CodeInstruction>();

                var code = codes[codesCache.IndexOf(c)];

                if (code.opcode == OpCodes.Ret)
                {
                    ret.Add(new(OpCodes.Stloc_S, 14));

                    ret.Add(new(OpCodes.Ldloca_S, 14));

                    ret.Add(new(OpCodes.Ldarg_0));

                    ret.Add(new(OpCodes.Ldarg_1));

                    ret.Add(new(OpCodes.Call, typeof(FactionDialogMakerPatch).GetMethod(nameof(FactionDialogFor))));

                    ret.Add(new(OpCodes.Ldloc_S, 14));

                    ret.Add(code);
                }

                else if (code.Calls(typeof(FactionDialogMaker).GetMethod("<FactionDialogFor>g__AddAndDecorateOption|0_0", BindingFlags.Static | BindingFlags.NonPublic)) && !addedPatch)
                {
                    if (codes.TryGetNext(code, out var next) && next.opcode == OpCodes.Ldloc_0)
                    {
                        ret.Add(code);

                        ret.Add(new(OpCodes.Ldloc_1) { labels = [..next.labels] });

                        ret.Add(new(OpCodes.Ldarg_1));

                        ret.Add(new(OpCodes.Ldarg_0));

                        ret.Add(new(OpCodes.Call, typeof(FactionDialogMakerPatch).GetMethod(nameof(CanRequestCustomOption))));

                        ret.Add(new(OpCodes.Brfalse_S, skipCustom));

                        ret.Add(new(OpCodes.Ldloc_1));

                        ret.Add(new(OpCodes.Ldarg_1));

                        ret.Add(new(OpCodes.Ldarg_0));

                        ret.Add(new(OpCodes.Call, typeof(FactionDialogMakerPatch).GetMethod(nameof(RequestCustomOption))));

                        ret.Add(new(OpCodes.Ldc_I4_1));

                        ret.Add(new(OpCodes.Ldloca_S, 0));

                        ret.Add(new CodeInstruction(OpCodes.Call, typeof(FactionDialogMaker).GetMethod("<FactionDialogFor>g__AddAndDecorateOption|0_0", BindingFlags.Static | BindingFlags.NonPublic)));

                        next.labels = [skipCustom];

                        addedPatch = true;
                    }

                    else ret.Add(code);
                }

                else ret.Add(code);

                return ret;
            });
        }

        public static IEnumerable<CodeInstruction> RequestMilitaryAidOptionTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            return codes.SelectMany(code =>
            {
                var ret = new List<CodeInstruction>();

                if (code.opcode == OpCodes.Ret)
                {
                    ret.Add(new(OpCodes.Stloc_3));

                    ret.Add(new(OpCodes.Ldloca, 3));

                    ret.Add(new(OpCodes.Ldarg_0));

                    ret.Add(new(OpCodes.Ldarg_1));

                    ret.Add(new(OpCodes.Ldarg_2));

                    ret.Add(new(OpCodes.Call, typeof(FactionDialogMakerPatch).GetMethod(nameof(RequestMilitaryAidOption))));

                    ret.Add(new(OpCodes.Ldloc_3));
                }

                ret.Add(code);

                return ret;
            });
        }

        public static IEnumerable<CodeInstruction> RequestOrbitalTraderOptionTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            return codes.SelectMany(code =>
            {
                var ret = new List<CodeInstruction>();

                if (code.opcode == OpCodes.Ret)
                {
                    ret.Add(new(OpCodes.Stloc_3));

                    ret.Add(new(OpCodes.Ldloca, 3));

                    ret.Add(new(OpCodes.Ldarg_0));

                    ret.Add(new(OpCodes.Ldarg_1));

                    ret.Add(new(OpCodes.Ldarg_2));

                    ret.Add(new(OpCodes.Call, typeof(FactionDialogMakerPatch).GetMethod(nameof(RequestOrbitalTraderOption))));

                    ret.Add(new(OpCodes.Ldloc_3));
                }

                ret.Add(code);

                return ret;
            });
        }

        public static IEnumerable<CodeInstruction> RequestTraderOptionTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            return codes.SelectMany(code =>
            {
                var ret = new List<CodeInstruction>();

                if (code.opcode == OpCodes.Ret)
                {
                    ret.Add(new(OpCodes.Stloc_3));

                    ret.Add(new(OpCodes.Ldloca, 3));

                    ret.Add(new(OpCodes.Ldarg_0));

                    ret.Add(new(OpCodes.Ldarg_1));

                    ret.Add(new(OpCodes.Ldarg_2));

                    ret.Add(new(OpCodes.Call, typeof(FactionDialogMakerPatch).GetMethod(nameof(RequestTraderOption))));

                    ret.Add(new(OpCodes.Ldloc_3));
                }

                ret.Add(code);

                return ret;
            });
        }

        public static void FactionDialogFor(ref DiaNode root, Pawn negotiator, Faction faction)
        {
            foreach (var kind in FactionRelationUtils.GetAllCustomFactionRelationKinds())
                kind.FactionDialogFor(ref root, negotiator, faction);
        }

        public static void RequestMilitaryAidOption(ref DiaOption diaOption, Map map, Faction faction, Pawn negotiator)
        {
            var relationKind = faction.PlayerRelationKind;

            if (FactionRelationUtils.CustomFactionRelationKindExist(relationKind))
                FactionRelationUtils.GetCustomFactionRelationKind(relationKind).RequestMilitaryAidOption(ref diaOption, map, faction, negotiator);
        }

        public static void RequestOrbitalTraderOption(ref DiaOption diaOption, Map map, Faction faction, Pawn negotiator)
        {
            var relationKind = faction.PlayerRelationKind;

            if (FactionRelationUtils.CustomFactionRelationKindExist(relationKind))
                FactionRelationUtils.GetCustomFactionRelationKind(relationKind).RequestOrbitalTraderOption(ref diaOption, map, faction, negotiator);
        }

        public static void RequestTraderOption(ref DiaOption diaOption, Map map, Faction faction, Pawn negotiator)
        {
            var relationKind = faction.PlayerRelationKind;

            if (FactionRelationUtils.CustomFactionRelationKindExist(relationKind))
                FactionRelationUtils.GetCustomFactionRelationKind(relationKind).RequestTraderOption(ref diaOption, map, faction, negotiator);
        }

        public static DiaOption RequestCustomOption(Map map, Faction faction, Pawn negotiator)
        {
            var relationKind = faction.PlayerRelationKind;

            if (FactionRelationUtils.CustomFactionRelationKindExist(relationKind))
                return FactionRelationUtils.GetCustomFactionRelationKind(relationKind).RequestCustomOption(map, faction, negotiator);

            return new();
        }

        public static bool CanRequestCustomOption(Map map, Faction faction, Pawn negotiator)
        {
            var relationKind = faction.PlayerRelationKind;

            if (FactionRelationUtils.CustomFactionRelationKindExist(relationKind))
                return FactionRelationUtils.GetCustomFactionRelationKind(relationKind).CanRequestCustomOption(map, faction, negotiator);

            return false;
        }
    }
}
