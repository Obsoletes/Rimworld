using HarmonyLib;
using System.Collections.Generic;
using Verse;

namespace DevL10N.Fix.Patch
{
	[HarmonyPatch(typeof(DebugToolsSpawning), "CreateMealWithSpecifics")]
	public static class DebugToolsSpawning_CreateMealWithSpecifics
	{
		[HarmonyTranspiler]
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			return Patch.Transpiler(instructions);
		}
	}
}
