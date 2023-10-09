using DevL10N.Fix;
using DevL10N.Fix.ModPatch;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using Verse;

namespace DevelopMode
{
	public class DevelopModeMod : Mod
	{
		public static XmlSerializer Serializer { get; set; }
		public static HarmonyMethod[] PatchMethod { get; set; }
		public static HarmonyMethod[] TitlePatchMethod { get; set; }
		public static Dictionary<string, Type> CacheType { get; set; }
		public DevelopModeMod(ModContentPack content) : base(content)
		{
			Harmony.DEBUG = true;
			var harmony = new Harmony("DevelopMode.Mod");
			Init();
			var stream = File.OpenRead(Path.Combine(Content.RootDir, "1.4", "Assemblies", "Patch.xml"));
			var config = Serializer.Deserialize(stream) as PatchConfig;
			foreach (var node in config.Nodes)
			{
				try
				{
					HarmonyMethod[] methodSet = PatchMethod;
					string typeName = node.Type, methodName = node.Method;
					if (!string.IsNullOrEmpty(node.TypeMethod))
					{
						var i = node.TypeMethod.IndexOf('_');
						if (i != -1)
						{
							typeName = node.TypeMethod.Substring(0, i);
							methodName = node.TypeMethod.Substring(i + 1);
						}
					}
					if (string.IsNullOrEmpty(typeName) || string.IsNullOrEmpty(methodName))
					{
						Log.Warning("Patch.xml is not correct.");
						continue;
					}
					if (node.Skip.HasValue && node.Skip > 3)
					{
						Log.Warning($"Patch.xml({node.Type}) is not correct.");
						continue;
					}
					if (node.UseTitle == true)
					{
						methodSet = TitlePatchMethod;
					}
					if (!CacheType.TryGetValue(typeName, out Type type))
					{
						type = AccessTools.TypeByName(typeName);
						CacheType.Add(typeName, type);
					}
					if (!string.IsNullOrEmpty(node.NestedType))
					{
						var nesteds = node.NestedType.Split(',');
						foreach (var nested in nesteds)
						{
							type = type.GetNestedType(nested, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
						}
					}
					MethodInfo method = null;
#if DEBUG
					Log.Message($"({type.FullName}).({methodName}) >>> {(methodSet == PatchMethod ? "PatchMethod" : "TitlePatchMethod")}[{node.Skip.GetValueOrDefault(0)}]");
#endif
					method = type.GetMethodWithoutFlag(methodName);
					harmony.Patch(method, transpiler: methodSet[node.Skip.GetValueOrDefault(0)]);
				}
				catch (Exception ex)
				{
					Log.Error("Patch.xml has error.");
					Log.Error(ex.ToString());
					continue;
				}
				CacheType.Clear();
			}
			ModPatch.Patch(harmony);

		}
		static void Init()
		{
			Serializer = new XmlSerializer(typeof(PatchConfig));
			PatchMethod = new HarmonyMethod[4];
			Type patch = typeof(PatchEx);
			PatchMethod[0] = new HarmonyMethod(patch.GetMethod(nameof(PatchEx.Transpiler0)));
			PatchMethod[1] = new HarmonyMethod(patch.GetMethod(nameof(PatchEx.Transpiler1)));
			PatchMethod[2] = new HarmonyMethod(patch.GetMethod(nameof(PatchEx.Transpiler2)));
			PatchMethod[3] = new HarmonyMethod(patch.GetMethod(nameof(PatchEx.Transpiler3)));
			TitlePatchMethod = new HarmonyMethod[4];
			TitlePatchMethod[0] = new HarmonyMethod(patch.GetMethod(nameof(PatchEx.TranspilerTitle0)));
			TitlePatchMethod[1] = new HarmonyMethod(patch.GetMethod(nameof(PatchEx.TranspilerTitle1)));
			TitlePatchMethod[2] = new HarmonyMethod(patch.GetMethod(nameof(PatchEx.TranspilerTitle2)));
			TitlePatchMethod[3] = new HarmonyMethod(patch.GetMethod(nameof(PatchEx.TranspilerTitle3)));
			CacheType = new Dictionary<string, Type>();
			CacheType.Add("DebugActionsMapManagement", typeof(DebugActionsMapManagement));
			CacheType.Add("DebugThingPlaceHelper", typeof(DebugThingPlaceHelper));
			CacheType.Add("DebugToolsGeneral", typeof(DebugToolsGeneral));
			CacheType.Add("DebugToolsPawns", typeof(DebugToolsPawns));
			CacheType.Add("DebugToolsSpawning", typeof(DebugToolsSpawning));
		}
	}
}
