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
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			var ils = instructions.ToList();
			for (int i = 0; i < ils.Count; i++)
			{
				if (ils[i].opcode == OpCodes.Ldfld)
				{
					// typeof(ils[i].operand)==System.Reflection.MonoField
					// what's this?
					// i have no choices
					var s = ils[i].operand.ToString();
					if (s.Contains("defName"))
					{
						ils[i].operand = labelField;
					}
				}
			}
			return ils;
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
