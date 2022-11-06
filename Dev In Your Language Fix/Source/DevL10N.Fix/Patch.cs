using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace DevelopMode
{
	public class PatchEx
	{
		static PatchEx()
		{
			ResoveField();
		}
		public static FieldInfo defField = null;
		public static FieldInfo labelField = null;
		public static FieldInfo titleField = null;
		public static IEnumerable<CodeInstruction> Transpiler0(IEnumerable<CodeInstruction> instructions)
		{
			return Transpiler(instructions);
		}
		public static IEnumerable<CodeInstruction> Transpiler1(IEnumerable<CodeInstruction> instructions)
		{
			return Transpiler(instructions, 1);
		}
		public static IEnumerable<CodeInstruction> Transpiler2(IEnumerable<CodeInstruction> instructions)
		{
			return Transpiler(instructions, 2);
		}
		public static IEnumerable<CodeInstruction> Transpiler3(IEnumerable<CodeInstruction> instructions)
		{
			return Transpiler(instructions, 3);
		}
		public static IEnumerable<CodeInstruction> TranspilerTitle0(IEnumerable<CodeInstruction> instructions)
		{
			return Transpiler(instructions);
		}
		public static IEnumerable<CodeInstruction> TranspilerTitle1(IEnumerable<CodeInstruction> instructions)
		{
			return Transpiler(instructions, 1);
		}
		public static IEnumerable<CodeInstruction> TranspilerTitle2(IEnumerable<CodeInstruction> instructions)
		{
			return Transpiler(instructions, 2);
		}
		public static IEnumerable<CodeInstruction> TranspilerTitle3(IEnumerable<CodeInstruction> instructions)
		{
			return Transpiler(instructions, 3);
		}
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, int skip = 0, bool useTitle = false)
		{
			foreach (var item in instructions)
			{
				if (item.opcode == OpCodes.Ldfld)
				{
					if (skip != 0)
					{
						skip--;
						continue;
					}
					var field = item.operand as FieldInfo;
					if (field.Name == "defName")
					{
						item.operand = useTitle ? titleField : labelField;
					}
				}
			}
			return instructions;
		}
		static void ResoveField()
		{
			if (defField == null)
				defField = typeof(ThingDef).GetField(nameof(ThingDef.defName));
			if (labelField == null)
				labelField = typeof(ThingDef).GetField(nameof(ThingDef.label));
			if (titleField == null)
				titleField = typeof(BackstoryDef).GetField(nameof(BackstoryDef.title));
		}
	}
}
