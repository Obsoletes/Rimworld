using HarmonyLib;
using System;
using Verse;

namespace DevL10N.Fix.ModPatch
{
	public static class ModPatch
	{
		public static void Patch(Harmony harmony)
		{
			try
			{
				Vehicle_Patch.Patch(harmony);
			}
			catch (Exception ex)
			{
				Log.Error("Mod Vehicle patch failed.");
				Log.Error(ex.ToString());
			}
		}
	}
}
