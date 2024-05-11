using System.Collections.Generic;
using Verse;
using System;

namespace DevL10N.Fix
{
	public class DefMap
	{
		public static Def EmptyDef = new Def();
		public DefMap(string Postfix = null)
		{
			Map = new Dictionary<string, string>();
			postfix = Postfix;
		}
		public bool DefNameToLabel(string defName, out string result)
		{
			var name = Transform(defName);
			var needAddPostfix = name != defName;
			if (Map.TryGetValue(name, out var defTitle))
			{
				result = needAddPostfix ? AddPostfix(defTitle) : defTitle;
				return true;
			}
			result = defName;
			return false;
		}
		public void Add<T>(string defName, T def, int debugIndex = 0, Func<T, string> selector = null) where T : Def
		{
			if (defName == EmptyDef.defName)
				return;
			if (selector == null)
				selector = d => d.label;
			if (Map.TryGetValue(defName, out var _))
			{
				Log.Warning($"duplicate def defType:({debugIndex}) defName:({defName}) Mod:({def.modContentPack.Name})");
			}
			else
				Map.Add(defName, selector(def));
		}
		public Dictionary<string, string> Map { get; set; }
		private readonly string postfix;
		private string Transform(string defName)
		{
			if (string.IsNullOrEmpty(defName))
				return defName;
			if (!string.IsNullOrEmpty(postfix) && defName.EndsWith(postfix))
				return defName.Replace(postfix, string.Empty);
			return defName;
		}
		private string AddPostfix(string label)
		{
			return $"{label}{postfix}";
		}
	}
}
