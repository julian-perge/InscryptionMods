using AddAllTarotCards.The_Emperor_And_Empress;
using AddAllTarotCards.The_Hanged_Man;
using AddAllTarotCards.Wheel_Of_Fortune;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using TheWorld;

namespace AddAllTarotCards
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
	public class HarmonyInit : BaseUnityPlugin
	{
		public const string PluginGuid = "julian.inscryption.cards.tarot";
		private const string PluginName = "TarotCardMod";
		private const string PluginVersion = "1.0.7";

		internal static ManualLogSource Log;

		void Awake()
		{
			Log = base.Logger;

			Card_TheHangedMan.InitCard();
			// Card_TheMagician.InitCard(); WIP
			Card_TheWorld.InitCard();
			Card_WOF.InitCard();
			Cards_Emperor_Empress.InitCards();

			var harmony = new Harmony(PluginGuid);
			harmony.PatchAll();
		}
	}
}
