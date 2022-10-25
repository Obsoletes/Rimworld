using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace DevL10N.Fix.Patch
{
	[HarmonyPatch(typeof(DebugToolsSpawning), "SpawnAtDevelopmentalStages")]
	public static class DebugToolsSpawning_SpawnAtDevelopmentalStages
	{
		[HarmonyTranspiler]
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			Log.Message("patch");
			return Patch.Transpiler(instructions);
		}
	}
}
