using BepInEx;
using HarmonyLib;

namespace FirePitAlwaysAbleToUpgrade
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	public class HarmonyInit : BaseUnityPlugin
	{
		private const string PluginGuid = "julianperge.inscryption.act1.firePitAlwaysAbleToUpgrade";
		private const string PluginName = "FirePitAlwaysAbleToUpgrade";
		private const string PluginVersion = "1.2.0";

		void Awake()
		{
			var harmony = new Harmony(PluginGuid);
			harmony.PatchAll();
		}
	}
}
