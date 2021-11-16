using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace AddAllSCP.SCP_034_Obsidian_Ritual_Knife
{
	public static class Card
	{
		public const string Name = "SCP_034_ObsidianRitualKnife";

		public static void InitCard()
		{
			NewAbility ability = RitualKnifeAbility.InitAbility();

			List<CardMetaCategory> metaCategories = CardUtils.getNormalCardMetadata;

			Texture2D defaultTexture =
				CardUtils.getAndloadImageAsTexture("BepInEx/plugins/CardLoader/Artwork/scp_034_small.png");

			string displayName = "Ritual Knife";
			string desc =
				"SCP-034 is a primitive knife constructed out of pure obsidian. Tests reveal that SCP-034 is approximately 1000 years old.";
			var abIds = new List<AbilityIdentifier>() { ability.id };

			NewCard.Add(Name, displayName, 1, 1,
				metaCategories, CardComplexity.Simple, CardTemple.Nature,
				desc, bloodCost: 1, defaultTex: defaultTexture, abilityIdsParam: abIds
			);
		}
	}
}
