using HarmonyLib;
using Verse;

namespace DevL10N.Fix
{
	[StaticConstructorOnStartup]
	public class DevL10NFix : Mod
	{
		public DevL10NFix(ModContentPack content) : base(content)
		{

		}
		static DevL10NFix()
		{
			var harmony = new Harmony("DevL10N.Fix");
			harmony.PatchAll();
		}
	}
}
