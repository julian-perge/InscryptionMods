using System;
using System.Collections.Generic;
using APIPlugin;
using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;

namespace NestCard
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	public class HarmonyInit : BaseUnityPlugin
	{
		public const string PluginGuid = "julianperge.inscryption";
		private const string PluginName = "nestCard";
		private const string PluginVersion = "1.0";

		internal static ManualLogSource Log;

		void Awake()
		{
			Log = base.Logger;

			NewAbility ability = NestAbility.InitAbility();
			var abilities = new List<Ability>() { ability.ability };
			new CustomCard("Stinkbug_Talking") { abilities = abilities };

			var harmony = new Harmony(PluginGuid);
			harmony.PatchAll();
		}
	}

	// add this to your deck by scrolling upwards/pressing w key when at the map
	[HarmonyPatch(typeof(DeckReviewSequencer), nameof(DeckReviewSequencer.OnEnterDeckView))]
	public class AddCardsToDeckPatch
	{
		private static bool allowSettingDeck = true;

		[HarmonyPrefix]
		public static void AddCardsToDeck()
		{
			if (allowSettingDeck)
			{
				Console.WriteLine($"Starting to load Exodia cards into deck");
				CardInfo card = CardLoader.GetCardByName("Stinkbug_Talking");

				CardUtils.PrintCardInfo(card);

				SaveManager.SaveFile.CurrentDeck.Cards.Clear();

				SaveManager.SaveFile.CurrentDeck.Cards.Add(card);
				SaveManager.SaveFile.CurrentDeck.Cards.Add(card);
				SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName("Bee"));
				SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName("Bee"));
				SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName("Bee"));
				SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName("Bee"));
			}
		}
	}

	// [HarmonyPatch(typeof(SelectableCardArray), nameof(SelectableCardArray.SpawnAndPlaceCards))]
	// public class SpawnAndPlacePatch
	// {
	// 	[HarmonyPrefix]
	// 	public static void Prefix(List<CardInfo> cards, CardPile pile, int numRows, bool triggerSpecialBehaviours, bool forPositiveEffect)
	// 	{
	// 		HarmonyInit.Log.LogInfo($"Inside SelectableCardArray.SpawnAndPlaceCards. NumRows {numRows} TriggerSpecialBehaviours {triggerSpecialBehaviours} ForPositiveEffect {forPositiveEffect} ");
	// 		if (cards is not null)
	// 		{
	// 			foreach (var cardInfo in cards)
	// 			{
	// 				HarmonyInit.Log.LogInfo($"Card -> [{cardInfo.displayedName}]");
	// 			}
	// 		}
	//
	// 		if (pile is not null) 
	// 		{
	// 			if (pile.cards is not null)
	// 			{
	// 				foreach (var pileCard in pile.cards)
	// 				{
	// 					HarmonyInit.Log.LogInfo($"-> Card transform name -> [{pileCard.name}]");
	// 				}
	// 			}
	// 			
	// 			HarmonyInit.Log.LogInfo($"-> Pile GO -> [{pile.gameObject}]");
	// 		}
	// 	}
	// }
}
