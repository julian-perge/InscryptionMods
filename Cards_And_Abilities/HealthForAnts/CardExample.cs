using System.Collections.Generic;
using System.IO;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace HealthForAnts
{
	public class CardExample
	{
		public static void InitCard()
		{
			var newAbility = HarmonyInit.antHealthSpecialAbility;

			var defaultTex = new Texture2D(2, 2);
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

			var sAbIds = new List<SpecialAbilityIdentifier>() { newAbility.id };

			NewCard.Add(
				name, metaCategories, CardComplexity.Advanced, CardTemple.Nature,
				displayedName, 0, 1, descryption,
				evolveParams: evolveParams, cost: 1, tex: defaultTex,
				specialStatIcon: newAbility.statIconInfo.iconType, specialAbilitiesIdsParam: sAbIds,
				tribes: tribes, traits: traits
			);
		}
	}
}
