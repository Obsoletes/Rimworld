using HarmonyLib;
using System.Collections.Generic;
using Verse;

namespace DevL10N.Fix.Patch
{
	[HarmonyPatch(typeof(DebugThingPlaceHelper), nameof(DebugThingPlaceHelper.TryPlaceOptionsForBaseMarketValue))]
	public static class DebugThingPlaceHelper_TryAbandonOptionsForStackCount
	{
		[HarmonyTranspiler]
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			return Patch.Transpiler(instructions);
		}
	}
}
