using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace DevL10N.Fix
{
	public static class Vehicle_Patch
	{
		static Type VehicleDef;
		public static Func<PawnKindDef, bool> Filter;
		private class Patcher
		{
			public static IEnumerable<CodeInstruction> SpawnPawnTranspiler(IEnumerable<CodeInstruction> instructions)
			{
				var instructions2 = instructions.ToList();
				for (var i = 0; i < instructions2.Count; i++)
				{
					var instruction = instructions2[i];
					if (instruction.opcode == OpCodes.Call)
					{
						//DefDatabase<PawnKindDef>.AllDefs
						var getMethod = typeof(DefDatabase<PawnKindDef>).GetProperty("AllDefs").GetMethod;
						if (instruction.operand is MethodInfo method && method == getMethod)
						{
							MethodInfo baseWhereMethod = null;
							// DefDatabase<PawnKindDef>.AllDefs.OrderBy((PawnKindDef kd) => kd.defName)
							foreach (var item in typeof(Enumerable).GetMethods().Where(m => m.Name == "Where"))
							{
								var para = item.GetParameters();
								if (para.Length != 2)
									continue;
								if (para[1].ParameterType.GetGenericArguments().Length == 2)
									baseWhereMethod = item;
							}
							instructions2.Insert(i + 1, new CodeInstruction(OpCodes.Ldsfld, typeof(Vehicle_Patch).GetField(nameof(Filter))));
							instructions2.Insert(i + 2, new CodeInstruction(OpCodes.Call, baseWhereMethod.MakeGenericMethod(typeof(PawnKindDef))));
						}
					}
				}
				return instructions2;
			}
			public static IEnumerable<CodeInstruction> DebugHideVehiclesFromPawnSpawnerTranspiler(IEnumerable<CodeInstruction> instructions)
			{
				return new CodeInstruction[] { new CodeInstruction(OpCodes.Ret) };
			}
		}
		public static void Patch(Harmony harmony)
		{
			Type target = AccessTools.TypeByName("Vehicles.Debug");
			VehicleDef = AccessTools.TypeByName("Vehicles.VehicleDef");
			if (target == null || VehicleDef == null)
				return;

			BuildFilter();
			/*
			 * two patches
			 * first Verse.DebugToolsSpawning.SpawnPawn return defs without VehicleDef
			 * second Vehicles.Debug.DebugHideVehiclesFromPawnSpawner do nothing
			 */
			harmony.Patch(typeof(DebugToolsSpawning).GetMethodWithoutFlag("SpawnPawn"),
			transpiler: new HarmonyMethod(typeof(Patcher).GetMethodWithoutFlag(nameof(Patcher.SpawnPawnTranspiler))));

			harmony.Patch(target.GetMethodWithoutFlag("DebugHideVehiclesFromPawnSpawner"),
			transpiler: new HarmonyMethod(typeof(Patcher).GetMethodWithoutFlag(nameof(Patcher.DebugHideVehiclesFromPawnSpawnerTranspiler))));

		}

		private static void BuildFilter()
		{
			ParameterExpression parameter = Expression.Parameter(typeof(PawnKindDef));
			var exp = Expression.Lambda<Func<PawnKindDef, bool>>(Expression.Not(Expression.TypeIs(Expression.Field(parameter,nameof(PawnKindDef.race)), VehicleDef)), parameter);
			Filter = exp.Compile();
		}
	}
}
