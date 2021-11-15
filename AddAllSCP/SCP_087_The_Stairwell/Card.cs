using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace AddAllSCP.SCP_087_The_Stairwell
{
	public static class Card
	{
		public const string Name = "SCP_087_TheStairwell";

		
		public static void InitCard()
		{
			NewAbility ability = TheStairwellAbility.InitAbility();
			List<CardMetaCategory> metaCategories = CardUtils.getNormalCardMetadata;

			Texture2D defaultTexture =
				CardUtils.getAndloadImageAsTexture("BepInEx/plugins/CardLoader/Artwork/scp_087_small.png");

			string displayName = "The Stairwell";
			string desc =
				"Subjects report and audio recordings confirm the distressed vocalizations from what is presumed to be a child between the ages of █ and ██";

			var abIds = new List<AbilityIdentifier>() { ability.id };

			NewCard.AddToPool(Name, displayName, 0, 6,
				metaCategories, CardComplexity.Simple, CardTemple.Nature,
				desc, bloodCost: 2, defaultTex: defaultTexture, abilityIdsParam: abIds
			);
		}
	}
}
