using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace AddAllSCP.SCP_035_Porcelain_Mask
{
	public static class Card
	{
		public const string Name = "SCP_035_PorcelainMask";

		public static void InitCard()
		{
			NewAbility ability = MaskAbility.InitAbility();

			List<CardMetaCategory> metaCategories = CardUtils.getNormalCardMetadata;
			List<Ability> abilities = new List<Ability> { Ability.PreventAttack };
			List<Trait> traits = new List<Trait>() { Trait.Terrain }; // this makes it not sacrificable

			Texture2D defaultTexture =
				CardUtils.getAndloadImageAsTexture("scp_035_small.png");

			string displayName = "Porcelain Mask";
			string desc = "A highly corrosive and degenerative viscous liquid constantly seeps from the eye and mouth holes.";
			var abIds = new List<AbilityIdentifier>() { ability.id };

			NewCard.Add(Name, displayName, 0, 1,
				metaCategories, CardComplexity.Simple, CardTemple.Nature,
				desc, bloodCost: 1, defaultTex: defaultTexture, abilities: abilities, traits: traits, abilityIdsParam: abIds
			);
		}
	}
}
