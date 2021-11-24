using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace SummonerCard
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
	[BepInDependency("julianperge.inscryption.sigiladay", BepInDependency.DependencyFlags.HardDependency)]
	public class HarmonyInit : BaseUnityPlugin
	{
		public const string PluginGuid = "julian.inscryption.cards.testing";
		private const string PluginName = "TestingDeck";
		private const string PluginVersion = "1.0.0";

		internal static ManualLogSource Log;

		void Awake()
		{
			Log = base.Logger;

			var harmony = new Harmony(PluginGuid);
			harmony.PatchAll();
		}
	}
}
