using HarmonyLib;
using System.Collections.Generic;
using Verse;

namespace DevL10N.Fix.Patch
{
	[HarmonyPatch(typeof(DebugThingPlaceHelper), nameof(DebugThingPlaceHelper.TryPlaceOptionsUnminified))]
	public static class DebugThingPlaceHelper_TryPlaceOptionsUnminified
	{
		[HarmonyTranspiler]
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			return Patch.Transpiler(instructions);
		}
	}
}
