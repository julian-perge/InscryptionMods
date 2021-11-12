using BepInEx;
using HarmonyLib;

namespace PeltsAreAlwaysFree
{

	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	public class HarmonyInit : BaseUnityPlugin
	{
		
		private const string PluginGuid = "com.julianperge";
		private const string PluginName = "peltsAreAlwaysFree";
		private const string PluginVersion = "1.0";
		
		void Awake()
		{
			var harmony = new Harmony(PluginGuid);
			harmony.PatchAll();
		}
	}

}
