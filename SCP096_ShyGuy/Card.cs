using System.Collections.Generic;
using APIPlugin;
using CardLoaderPlugin.lib;
using DiskCardGame;
using UnityEngine;

namespace ThePaleManCard
{
	public static class Card
	{
		public static void InitCard()
		{
			List<CardMetaCategory> metaCategories = CardUtils.getRareCardMetadata;
			List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = CardUtils.getRareAppearance;

			byte[] defaultTextureBytes =
				System.IO.File.ReadAllBytes("BepInEx/plugins/CardLoader/Artwork/scp_096_hide_small.png");
			Texture2D defaultTexture = new Texture2D(2, 2);
			defaultTexture.LoadImage(defaultTextureBytes);

			byte[] altBytes = System.IO.File.ReadAllBytes("BepInEx/plugins/CardLoader/Artwork/scp_096_attack_small.png");
			Texture2D altTexture = new Texture2D(2, 2);
			altTexture.LoadImage(altBytes);

			NewCard.Add("SCP096_ShyGuy", metaCategories, CardComplexity.Simple, CardTemple.Nature, "Shy Guy",
				0, 6,
				description: "SCP-096 - Shy Guy. He may seem harmless, but no one has ever seen his face and survived...",
				cost: 1, appearanceBehaviour: appearanceBehaviour, tex: defaultTexture, altTex: altTexture,
				abilities: new List<Ability> { TheSightAbility.ability });
		}
	}
}
