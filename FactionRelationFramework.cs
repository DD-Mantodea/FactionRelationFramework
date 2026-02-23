using FactionRelationFramework.Content.Factions;
using FactionRelationFramework.Utils;
using HarmonyLib;
using LudeonTK;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Verse;

namespace FactionRelationFramework
{
    public class FactionRelationFramework(ModContentPack content) : Mod(content)
    {
        public static Harmony Harmony = new("Mantodea.FactionRelationFramework");
    }
}
