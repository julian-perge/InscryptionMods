using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace AddAllSCP.SCP_096_Shy_Guy
{
	public static class Card
	{
		public const string Name = "SCP_096_ShyGuy";

		public static void InitCard()
		{
			NewAbility ability = TheSightAbility.InitAbility();
			List<CardMetaCategory> metaCategories = CardUtils.getRareCardMetadata;
			List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = CardUtils.getRareAppearance;

			Texture2D defaultTexture =
				CardUtils.getAndloadImageAsTexture("BepInEx/plugins/CardLoader/Artwork/scp_096_hide_small.png");

			Texture2D altTexture =
				CardUtils.getAndloadImageAsTexture("BepInEx/plugins/CardLoader/Artwork/scp_096_attack_small.png");

			var displayName = "Shy Guy";
			var desc = "SCP-096 - Shy Guy. He may seem harmless, but no one has ever seen his face and survived.";

			var abilityIds = new List<AbilityIdentifier>() { ability.id };

			NewCard.Add(Name, displayName, 0, 6,
				metaCategories, CardComplexity.Simple, CardTemple.Nature,
				desc, bloodCost: 2, appearanceBehaviour: appearanceBehaviour,
				defaultTex: defaultTexture, altTex: altTexture, abilityIdsParam: abilityIds
			);
		}
	}
}
