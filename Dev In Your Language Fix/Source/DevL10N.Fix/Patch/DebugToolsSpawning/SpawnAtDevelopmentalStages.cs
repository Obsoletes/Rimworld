using HarmonyLib;
using System.Collections.Generic;
using Verse;

namespace DevL10N.Fix.Patch
{
	[HarmonyPatch(typeof(DebugToolsSpawning), "SpawnAtDevelopmentalStages")]
	public static class DebugToolsSpawning_SpawnAtDevelopmentalStages
	{
		[HarmonyTranspiler]
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			return Patch.Transpiler(instructions, 1);
		}
	}
}
