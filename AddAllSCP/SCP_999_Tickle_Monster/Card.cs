using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace AddAllSCP.SCP_999_Tickle_Monster
{
	public static class Card
	{
		public static void InitCard()
		{
			List<CardMetaCategory> metaCategories = CardUtils.getNormalCardMetadata;
			List<Ability> abilities = new List<Ability> { Ability.DebuffEnemy, Ability.BuffNeighbours };

			Texture2D defaultTexture =
				CardUtils.getAndloadImageAsTexture("BepInEx/plugins/CardLoader/Artwork/scp_999_small.png");

			var name = "SCP_999_TickleMonster";
			var displayName = "Tickle Monster";
			var desc =
				"Simply touching SCP-999’s surface causes an immediate euphoria, which intensifies the longer one is exposed to SCP-999, and lasts long after separation from the creature";

			NewCard.AddToPool(name, displayName, 0, 2,
				metaCategories, CardComplexity.Simple, CardTemple.Nature,
				desc, bloodCost: 1, defaultTex: defaultTexture, abilities: abilities
			);
		}
	}
}
