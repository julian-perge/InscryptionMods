using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace AddAllSCP.SCP_049_Plague_Doctor
{
	public class Card
	{
		public const string Name = "SCP_049_PlagueDoctor";

		public static void InitCard()
		{
			NewAbility ability = TheCureAbility.InitAbility();
			List<CardMetaCategory> metaCategories = CardUtils.getNormalCardMetadata;

			Texture2D defaultTexture =
				CardUtils.getAndloadImageAsTexture("BepInEx/plugins/CardLoader/Artwork/scp_049_small.png");

			var displayName = "Plague Doctor";
			var abIds = new List<AbilityIdentifier>() { ability.id };

			NewCard.Add(Name, displayName, 0, 2,
				metaCategories, CardComplexity.Simple, CardTemple.Nature,
				bonesCost: 6, defaultTex: defaultTexture, abilityIdsParam: abIds
			);
		}
	}
}
