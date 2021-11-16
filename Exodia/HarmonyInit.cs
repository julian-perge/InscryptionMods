using System;
using APIPlugin;
using BepInEx;
using DiskCardGame;
using HarmonyLib;

namespace Exodia
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
	public class HarmonyInit : BaseUnityPlugin
	{
		public const string PluginGuid = "julianperge.inscryption.cards.exodia";
		private const string PluginName = "Exodia and the left and right arms to card pool";
		private const string PluginVersion = "1.2";

		private void Awake()
		{
			Exodia.Card.InitCards();

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
				CardInfo card = CardLoader.GetCardByName(Exodia.Card.Name);
				CardInfo cardLeftArm = CardLoader.GetCardByName(Exodia.Card.NameLeftArm);
				CardInfo cardRightArm = CardLoader.GetCardByName(Exodia.Card.NameRightArm);

				CardUtils.PrintCardInfo(card);

				SaveManager.SaveFile.CurrentDeck.Cards.Clear();

				SaveManager.SaveFile.CurrentDeck.Cards.Add(card);
				SaveManager.SaveFile.CurrentDeck.Cards.Add(cardLeftArm);
				SaveManager.SaveFile.CurrentDeck.Cards.Add(cardRightArm);
			}
		}
	}
}
