﻿using HarmonyLib;
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
	[HarmonyPatch(typeof(DebugToolsPawns), "MaxSkill")]
	public static class DebugToolsPawns_MaxSkill
	{
		[HarmonyTranspiler]
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			return Patch.Transpiler(instructions);
		}
	}
}
