using System.Collections.Generic;
using APIPlugin;
using CardLoaderPlugin.lib;
using DiskCardGame;
using UnityEngine;

namespace AddAllSCP.SCP_049_Plague_Doctor
{
	public static class Card
	{
		public static void InitCard()
		{
			List<CardMetaCategory> metaCategories = CardUtils.getRareCardMetadata;
			List<Ability> abilities = new List<Ability> { DoubleDeathTweaked.ability };

			Texture2D defaultTexture =
				CardUtils.getAndloadImageAsTexture("BepInEx/plugins/CardLoader/Artwork/scp_049_small.png");

			var name = "SCP_049_PlagueDoctor";
			var displayName = "Plague Doctor";

			NewCard.AddToPool(name, displayName, 0, 2,
				metaCategories, CardComplexity.Advanced, CardTemple.Undead,
				bonesCost: 6, defaultTexture: defaultTexture, abilities: abilities
			);
		}
	}
}
