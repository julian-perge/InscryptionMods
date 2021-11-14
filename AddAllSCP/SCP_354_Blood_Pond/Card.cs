using System.Collections.Generic;
using APIPlugin;
using CardLoaderPlugin.lib;
using DiskCardGame;
using UnityEngine;

namespace AddAllSCP.SCP_354_Blood_Pond
{
	public static class Card
	{
		public static void InitCard()
		{
			List<CardMetaCategory> metaCategories = CardUtils.getNormalCardMetadata;

			List<Ability> abilities = new List<Ability> { BloodPondAbility.ability };

			Texture2D defaultTexture =
				CardUtils.getAndloadImageAsTexture("BepInEx/plugins/CardLoader/Artwork/scp_354_small.png");

			var name = "SCP_354_BloodPond";
			var displayName = "Blood Pond";

			NewCard.AddToPool(name, displayName, 0, 4,
				metaCategories, CardComplexity.Simple, CardTemple.Nature,
				bloodCost: 2, defaultTexture: defaultTexture, abilities: abilities
			);
		}

		public static void InitCardsSpawnedFromBloodPond()
		{
			List<CardMetaCategory> metaCategories = CardUtils.getNormalCardMetadata;

			List<Ability> abilities = new List<Ability> { Ability.Brittle };

			Texture2D defaultTextureCreature =
				CardUtils.getAndloadImageAsTexture("BepInEx/plugins/CardLoader/Artwork/scp_354_creature_small.png");
			Texture2D defaultTextureEntity =
				CardUtils.getAndloadImageAsTexture("BepInEx/plugins/CardLoader/Artwork/scp_354_entity_small.png");

			var nameCreature = "SCP_354_BloodCreature";
			var displayNameCreature = "Blood Creature";
			var desc = "Spawned from the Blood Pond.";

			NewCard.AddToPool(nameCreature, displayNameCreature, 2, 2,
				metaCategories, CardComplexity.Simple, CardTemple.Nature,
				desc, defaultTexture: defaultTextureCreature, abilities: abilities
			);

			var nameEntity = "SCP_354_BloodEntity";
			var displayNameEntity = "Blood Entity";
			NewCard.AddToPool(nameEntity, displayNameEntity, 3, 3,
				metaCategories, CardComplexity.Simple, CardTemple.Nature,
				desc, defaultTexture: defaultTextureEntity, abilities: abilities
			);
		}
	}
}
