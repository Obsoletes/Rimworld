using System.Collections.Generic;
using Verse;

namespace DevL10N.Fix
{
	public class DefMap
	{
		public DefMap(string Postfix = null)
		{
			Map = new Dictionary<string, Def>();
			postfix = Postfix;
		}
		public bool DefNameToLabel(string defName, out string result)
		{
			var name = Transform(defName);
			var needAddPostfix = name != defName;
			if (Map.TryGetValue(name, out var def))
			{
				result = needAddPostfix ? AddPostfix(def.label) : def.label;
				return true;
			}
			result = defName;
			return false;
		}
		public void Add(string defName, Def def)
		{
			Map.Add(defName, def);
		}
		public Dictionary<string, Def> Map { get; set; }
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
