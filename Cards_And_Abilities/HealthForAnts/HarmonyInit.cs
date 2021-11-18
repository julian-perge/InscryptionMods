using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace HealthForAnts
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency("cyantist.inscryption.api")]
	public class HarmonyInit : BaseUnityPlugin
	{
		public const string PluginGuid = "julianperge.inscryption.cards.healthForAnts";
		private const string PluginName = "HealthForAnts";
		private const string PluginVersion = "1.0";
		internal static ManualLogSource Log;

		void Awake()
		{
			Log = base.Logger;

			HealthForAnts.InitStatIconAndAbility();

			Harmony harmony = new Harmony(PluginGuid);
			harmony.PatchAll();
		}
	}
}
