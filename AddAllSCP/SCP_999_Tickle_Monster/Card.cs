using System.Collections.Generic;
using APIPlugin;
using CardLoaderPlugin.lib;
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

			byte[] defaultTextureBytes =
				System.IO.File.ReadAllBytes("BepInEx/plugins/CardLoader/Artwork/scp_999_small.png");
			Texture2D defaultTexture = new Texture2D(2, 2);
			defaultTexture.LoadImage(defaultTextureBytes);
			
			NewCard.Add("SCP_999_TickleMonster", metaCategories, CardComplexity.Simple, CardTemple.Nature, "Tickle Monster",
				0, 2,
				"Simply touching SCP-999’s surface causes an immediate euphoria, which intensifies the longer one is exposed to SCP-999, and lasts long after separation from the creature",
				cost: 1, tex: defaultTexture, abilities: abilities);
		}
		
	}
	
}
