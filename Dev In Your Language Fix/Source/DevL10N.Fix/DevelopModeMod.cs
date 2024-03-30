using DevL10N.Fix;
using HarmonyLib;
using LudeonTK;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Verse;

namespace DevelopMode
{
	public class DevelopModeMod : Mod
	{
		const int THING_DEF = 0;
		const int PAWNKINK_DEF = 1;
		const int XENOTYPE_DEF = 2;
		const int SKILL_DEF = 3;
		const int MENTALBREAK_DEF = 4;
		const int TERRAIN_DEF = 5;
		const int INSPIRATION_DEF = 6;
		const int DAMAGE_DEF = 7;

		const int LENGTH = DAMAGE_DEF + 1;
		public static DefMap[] DefMaps { get; set; }
		public static bool IsInit = false;
		public static string FindLabelByDefName(string defName)
		{
			if (!IsInit)
			{
				Init();
				IsInit = true;
			}

			foreach (var map in DefMaps)
			{
				if (map.DefNameToLabel(defName, out var result))
					return result;
			}
			return defName;

		}
		public DevelopModeMod(ModContentPack content) : base(content)
		{
#if DEBUG
			Harmony.DEBUG = true;
#endif
			var harmony = new Harmony("DevelopMode.Mod");
			var method = typeof(Dialog_Debug).GetMethodWithoutFlag(nameof(Dialog_Debug.ButtonDebugPinnable));
			DefMaps = new DefMap[LENGTH];
			harmony.Patch(method, transpiler: new HarmonyMethod(typeof(DevelopModeMod).GetMethod(nameof(Transpiler))));
		}
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			var list = new List<CodeInstruction>(instructions);
			for (int i = 0; i <= list.Count; i++)
			{
				if (list[i].opcode == OpCodes.Ldarg_1)
				{
					i++;
					return list.Take(i).Append(new CodeInstruction(OpCodes.Call, typeof(DevelopModeMod).GetMethod(nameof(FindLabelByDefName)))).Concat(list.Skip(i)).ToList();
				}
			}
			Log.Error("Not found patch entity");
			return instructions;
		}
		private static void Init()
		{
			DefMaps[THING_DEF] = new DefMap(" (minified)");
			foreach (var def in DefDatabase<ThingDef>.AllDefs)
			{
				DefMaps[THING_DEF].Add(def.defName, def);
			}
			DefMaps[PAWNKINK_DEF] = new DefMap();
			foreach (var def in DefDatabase<PawnKindDef>.AllDefs)
			{
				DefMaps[PAWNKINK_DEF].Add(def.defName, def);
			}
			DefMaps[XENOTYPE_DEF] = new DefMap();
			foreach (var def in DefDatabase<XenotypeDef>.AllDefs)
			{
				DefMaps[XENOTYPE_DEF].Add(def.defName, def);
			}
			DefMaps[SKILL_DEF] = new DefMap();
			foreach (var def in DefDatabase<SkillDef>.AllDefs)
			{
				DefMaps[SKILL_DEF].Add(def.defName, def);
			}
			DefMaps[MENTALBREAK_DEF] = new DefMap(" [NO]");
			foreach (var def in DefDatabase<MentalBreakDef>.AllDefs)
			{
				DefMaps[MENTALBREAK_DEF].Add(def.defName, def);
			}
			DefMaps[TERRAIN_DEF] = new DefMap();
			foreach (var def in DefDatabase<TerrainDef>.AllDefs)
			{
				DefMaps[TERRAIN_DEF].Add(def.defName, def);
			}
			DefMaps[INSPIRATION_DEF] = new DefMap();
			foreach (var def in DefDatabase<InspirationDef>.AllDefs)
			{
				DefMaps[INSPIRATION_DEF].Add(def.defName, def);
			}
			DefMaps[DAMAGE_DEF] = new DefMap(" (Destroy part)");
			foreach (var def in DefDatabase<DamageDef>.AllDefs)
			{
				DefMaps[DAMAGE_DEF].Add(def.defName, def);
			}
		}
	}
}
