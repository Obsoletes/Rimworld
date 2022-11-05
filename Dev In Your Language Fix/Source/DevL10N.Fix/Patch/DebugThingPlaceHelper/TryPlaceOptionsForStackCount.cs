using HarmonyLib;
using System.Collections.Generic;
using Verse;

namespace DevL10N.Fix.Patch
{
	[HarmonyPatch(typeof(DebugThingPlaceHelper), nameof(DebugThingPlaceHelper.TryPlaceOptionsForStackCount))]
	public static class DebugThingPlaceHelper_TryPlaceOptionsForStackCount
	{
		[HarmonyTranspiler]
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			return Patch.Transpiler(instructions);
		}
	}
}
