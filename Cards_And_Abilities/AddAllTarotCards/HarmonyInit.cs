using AddAllTarotCards.The_Hanged_Man;
using AddAllTarotCards.Wheel_Of_Fortune;
using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using TheEmperorAndEmpress;
using TheWorld;

namespace AddAllTarotCards
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
	public class HarmonyInit : BaseUnityPlugin
	{
		public const string PluginGuid = "julian.inscryption.cards.tarot";
		private const string PluginName = "TarotCardMod";
		private const string PluginVersion = "1.0.6";

		internal static ManualLogSource Log;

		void Awake()
		{
			Log = base.Logger;

			Card_TheHangedMan.InitCard();
			Card_TheWorld.InitCard();
			Card_WOF.InitCard();
			Cards_Emperor_Empress.InitCards();

			var harmony = new Harmony(PluginGuid);
			harmony.PatchAll();
		}

		// add this to your deck by scrolling upwards/pressing w key when at the map
		[HarmonyPatch(typeof(DeckReviewSequencer), nameof(DeckReviewSequencer.OnEnterDeckView))]
		public class AddCardsToDeckPatch
		{
			private static bool allowSettingDeck = true;

			[HarmonyPrefix]
			public static void AddCards()
			{
				if (allowSettingDeck)
				{
					SaveManager.SaveFile.CurrentDeck.Cards.Clear();

					SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName(Card_TheHangedMan.Name));
					SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName("Geck"));
					SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName("Geck"));
					SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName("Geck"));
				}
			}
		}
	}
}
