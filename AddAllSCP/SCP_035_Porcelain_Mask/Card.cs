using System.Collections.Generic;
using APIPlugin;
using CardLoaderPlugin.lib;
using DiskCardGame;
using UnityEngine;

namespace AddAllSCP.SCP_035_Porcelain_Mask
{
	public static class Card
	{
		public static void InitCard()
		{
			List<CardMetaCategory> metaCategories = CardUtils.getNormalCardMetadata;
			List<Ability> abilities = new List<Ability> { MaskAbility.ability, Ability.PreventAttack };

			Texture2D defaultTexture =
				CardUtils.getAndloadImageAsTexture("BepInEx/plugins/CardLoader/Artwork/scp_035_small.png");

			string name = "SCP_035_PorcelainMask";
			string displayName = "Porcelain Mask";
			string desc = "A highly corrosive and degenerative viscous liquid constantly seeps from the eye and mouth holes.";

			NewCard.AddToPool(name, displayName, 0, 1,
				metaCategories, CardComplexity.Simple, CardTemple.Nature, 
				bloodCost: 1, defaultTexture: defaultTexture, abilities: abilities
			);
		}
	}
}
