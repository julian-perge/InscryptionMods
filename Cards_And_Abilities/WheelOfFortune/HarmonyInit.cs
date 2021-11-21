using System;
using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;

namespace WheelOfFortune
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
	public class HarmonyInit : BaseUnityPlugin
	{
		public const string PluginGuid = "julianperge.inscryption.cards.wheelOfFortune";
		private const string PluginName = "TarotMod-WheelOfFortune";
		private const string PluginVersion = "1.0.2";

		internal static ManualLogSource Log;

		private void Awake()
		{
			Log = base.Logger;

			var cardInfo = Card_WOF.InitCard();

			var harmony = new Harmony(PluginGuid);
			harmony.PatchAll();
		}
	}

	// add this to your deck by scrolling upwards/pressing w key when at the map
	[HarmonyPatch(typeof(DeckReviewSequencer), nameof(DeckReviewSequencer.OnEnterDeckView))]
	public class AddCardsToDeckPatch
	{
		private static bool allowSettingDeck = false;

		[HarmonyPrefix]
		public static void AddCardsToDeck()
		{
			if (allowSettingDeck)
			{
				Console.WriteLine($"Starting to load Exodia cards into deck");
				CardInfo card = CardLoader.GetCardByName(Card_WOF.Name);

				// CardUtils.PrintCardInfo(card);

				SaveManager.SaveFile.CurrentDeck.Cards.Clear();

				SaveManager.SaveFile.CurrentDeck.Cards.Add(card);
				SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName("Geck"));
				SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName("Geck"));
				SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName("Geck"));
			}
		}
	}
}
