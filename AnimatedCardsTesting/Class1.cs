using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace AnimatedCardsTesting
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	public class HarmonyInit : BaseUnityPlugin
	{
		private const string PluginGuid = "julianperge.inscryption.cards.animatedCardsTesting";
		private const string PluginName = "AnimatedCardsTesting";
		private const string PluginVersion = "0.0.1";

		internal static ManualLogSource Log;

		void Awake()
		{
			Log = base.Logger;

			var harmony = new Harmony(PluginGuid);
			harmony.PatchAll();
		}
	}
}
