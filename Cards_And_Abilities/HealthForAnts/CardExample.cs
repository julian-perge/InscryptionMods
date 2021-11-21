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
			byte[] imgBytes = File.ReadAllBytes("dome_ant.png");
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
				name, displayedName, 0, 1, metaCategories, CardComplexity.Advanced, CardTemple.Nature,
				descryption, evolveParams: evolveParams, bloodCost: 1, defaultTex: defaultTex,
				specialStatIcon: newAbility.statIconInfo.iconType, specialAbilitiesIdsParam: sAbIds,
				tribes: tribes, traits: traits
			);
		}
	}
}
