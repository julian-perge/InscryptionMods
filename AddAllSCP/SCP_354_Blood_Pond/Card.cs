using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using EasyFeedback.APIs;
using UnityEngine;

namespace AddAllSCP.SCP_354_Blood_Pond
{
	public static class Card
	{
		public const string NameBloodCreature = "SCP_354_BloodCreature";
		public const string NameBloodEntity = "SCP_354_BloodEntity";
		
		public static void InitCardsAndAbilities()
		{
			InitCard();
			InitCardsSpawnedFromBloodPond();
		}

		public const string Name = "SCP_354_BloodPond";

		private static void InitCard()
		{
			NewAbility ability = BloodPondAbility.InitAbility();
			List<CardMetaCategory> metaCategories = CardUtils.getNormalCardMetadata;
			
			Texture2D defaultTexture =
				CardUtils.getAndloadImageAsTexture("BepInEx/plugins/CardLoader/Artwork/scp_354_small.png");

			var displayName = "Blood Pond";
			var abIds = new List<AbilityIdentifier>() { ability.id };

			NewCard.AddToPool(Name, displayName, 0, 4,
				metaCategories, CardComplexity.Simple, CardTemple.Nature,
				bloodCost: 2, defaultTex: defaultTexture, abilityIdsParam: abIds
			);
		}

		private static void InitCardsSpawnedFromBloodPond()
		{
			List<CardMetaCategory> metaCategories = CardUtils.getNormalCardMetadata;
			List<Ability> abilities = new List<Ability> { Ability.Brittle };

			Texture2D defaultTextureCreature =
				CardUtils.getAndloadImageAsTexture("BepInEx/plugins/CardLoader/Artwork/scp_354_blood_creature_small.png");
			Texture2D defaultTextureEntity =
				CardUtils.getAndloadImageAsTexture("BepInEx/plugins/CardLoader/Artwork/scp_354_blood_entity_small.png");
			
			var displayNameCreature = "Blood Creature";
			var desc = "Spawned from the Blood Pond.";

			NewCard.AddToPool(NameBloodCreature, displayNameCreature, 1, 1,
				metaCategories, CardComplexity.Simple, CardTemple.Nature,
				desc, defaultTex: defaultTextureCreature, abilities: abilities
			);

			var displayNameEntity = "Blood Entity";
			
			NewCard.AddToPool(NameBloodEntity, displayNameEntity, 1, 1,
				metaCategories, CardComplexity.Simple, CardTemple.Nature,
				desc, defaultTex: defaultTextureEntity, abilities: abilities
			);
		}
	}
}
