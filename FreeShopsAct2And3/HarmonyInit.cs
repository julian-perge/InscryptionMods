using BepInEx;
using HarmonyLib;

namespace FreeShopsAct2And3
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	public class HarmonyInit : BaseUnityPlugin
	{
		private const string PluginGuid = "com.julianperge";
		private const string PluginName = "startWithMaxCurrency";
		private const string PluginVersion = "1.0";

		void Awake()
		{
			var harmony = new Harmony(PluginGuid);
			harmony.PatchAll();
		}
	}
}
