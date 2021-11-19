using System.Collections.Generic;
using System.IO;
using APIPlugin;
using BepInEx;
using DiskCardGame;
using HealthForAnts;
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

			var antHealthAbility = HarmonyInit.antHealthSpecialAbility;
			var sAbIds = new List<SpecialAbilityIdentifier>() { antHealthAbility.id };

			NewCard.Add(
				name, metaCategories, CardComplexity.Advanced, CardTemple.Nature,
				displayedName, 0, 1, descryption,
				evolveParams: evolveParams, cost: 1, tex: defaultTex,
				specialStatIcon: antHealthAbility.statIconInfo.iconType, specialAbilitiesIdsParam: sAbIds,
				tribes: tribes, traits: traits
			);
		}
	}
}
