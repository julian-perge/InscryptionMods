using System.Collections.Generic;
using APIPlugin;
using CardLoaderPlugin.lib;
using DiskCardGame;
using UnityEngine;

namespace AddAllSCP.SCP_096_Shy_Guy
{
	public static class Card
	{
		public static void InitCard()
		{
			List<CardMetaCategory> metaCategories = CardUtils.getRareCardMetadata;
			List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = CardUtils.getRareAppearance;
			var abilities = new List<Ability> { TheSightAbility.ability };

			Texture2D defaultTexture =
				CardUtils.getAndloadImageAsTexture("BepInEx/plugins/CardLoader/Artwork/scp_096_hide_small.png");

			Texture2D altTexture =
				CardUtils.getAndloadImageAsTexture("BepInEx/plugins/CardLoader/Artwork/scp_096_attack_small.png");

			var name = "SCP_096_ShyGuy";
			var displayName = "Shy Guy";
			var desc = "SCP-096 - Shy Guy. He may seem harmless, but no one has ever seen his face and survived.";
			
			NewCard.AddToPool(name, displayName, 0, 6,
				metaCategories, CardComplexity.Simple, CardTemple.Nature,
				description: desc,
				bloodCost: 1, appearanceBehaviour: appearanceBehaviour, defaultTexture: defaultTexture, altTexture: altTexture,
				abilities: abilities
			);
		}
	}
}
