using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace DevL10N.Fix.Patch
{
	public class Patch
	{
		static Patch()
		{
			ResoveField();
		}
		public static FieldInfo defField = null;
		public static FieldInfo labelField = null;
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, int skip = 0)
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
						item.operand = labelField;
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
		}
	}
}
