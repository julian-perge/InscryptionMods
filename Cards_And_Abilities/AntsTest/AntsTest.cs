using System.Collections.Generic;
using System.IO;
using APIPlugin;
using BepInEx;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace AntsTest
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency("cyantist.inscryption.api")]
	[BepInDependency("julianperge.inscryption.cards.healthForAnts")]
	public class AntsTest : BaseUnityPlugin
	{
		public const string PluginGuid = "julianperge.inscryption.cards.antsTest";
		private const string PluginName = "AntsTest";
		private const string PluginVersion = "1.0";

		void Awake()
		{
			Texture2D defaultTex = new Texture2D(2, 2);
			byte[] imgBytes = File.ReadAllBytes("BepInEx/plugins/CardLoader/Artwork/dome_ant.png");
			bool isLoaded = defaultTex.LoadImage(imgBytes);
			defaultTex.LoadImage(imgBytes);

			List<CardMetaCategory> metaCategories = CardUtils.getNormalCardMetadata;

			string name = "DomeAnt";
			string displayedName = "Dome Ant";
			string descryption = "Loves to guard his friends";

			EvolveParams evolveParams = new() { turnsToEvolve = 1, evolution = CardLoader.GetCardByName("AntQueen") };
			List<Tribe> tribes = new() { Tribe.Insect };
			List<Trait> traits = new() { Trait.Ant };

			var antHealthAbility = HealthForAnts.HarmonyInit.antHealthSpecialAbility;
			var sAbIds = new List<SpecialAbilityIdentifier>() { antHealthAbility.id };

			NewCard.Add(
				name, metaCategories, CardComplexity.Advanced, CardTemple.Nature,
				displayedName, 0, 1, descryption,
				evolveParams: evolveParams, cost: 1, tex: defaultTex,
				specialStatIcon: antHealthAbility.statIconInfo.iconType, specialAbilitiesIdsParam: sAbIds,
				tribes: tribes, traits: traits
			);

			Harmony harmony = new Harmony(PluginGuid);
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
				CardInfo card = CardLoader.GetCardByName("DomeAnt");
				// foreach (var specialTriggeredAbility in card.SpecialAbilities)
				// {
				// 	MoreAnts.Log.LogInfo($"-> special ability for [{card.name}] => [{specialTriggeredAbility}]");
				// }

				SaveManager.SaveFile.CurrentDeck.Cards.Clear();

				// SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName("Geck"));
				// SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName("Ant"));
				SaveManager.SaveFile.CurrentDeck.Cards.Add(card);
				// SaveManager.SaveFile.CurrentDeck.Cards.Add(card);
				// SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName("Ant"));
				// SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName("Ant"));
				SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName("Ant"));
				SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName("Ant"));
			}
		}
	}
}
