using BepInEx;
using HarmonyLib;

namespace FirePitIncreaseStatBoost
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	public class HarmonyInit : BaseUnityPlugin
	{
		
		private const string PluginGuid = "com.julianperge";
		private const string PluginName = "firePitIncreaseStatBoost";
		private const string PluginVersion = "1.2";
		
		void Awake()
		{
			var harmony = new Harmony(PluginGuid);
			harmony.PatchAll();
		}

	}
}
