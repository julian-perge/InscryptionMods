using BepInEx;
using HarmonyLib;

namespace FirePitAlwaysAbleToUpgrade
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	public class Plugin : BaseUnityPlugin
	{
		private const string PluginGuid = "julianperge.inscryption.act1.firePitAlwaysAbleToUpgrade";
		private const string PluginName = "FirePitAlwaysAbleToUpgrade";
		private const string PluginVersion = "1.3.0";

		internal static BepInEx.Logging.ManualLogSource Log;

		void Awake()
		{
			Log = base.Logger;

			var harmony = new Harmony(PluginGuid);
			harmony.PatchAll();
		}
	}
}
