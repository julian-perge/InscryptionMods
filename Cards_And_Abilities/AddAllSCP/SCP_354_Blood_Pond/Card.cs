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
			APIPlugin.NewAbility ability = BloodPondAbility.InitAbility();
			System.Collections.Generic.List<DiskCardGame.CardMetaCategory> metaCategories =
				APIPlugin.CardUtils.getNormalCardMetadata;

			UnityEngine.Texture2D defaultTexture =
				APIPlugin.CardUtils.LoadImageAndGetTexture("scp_354_small.png");

			const string displayName = "Blood Pond";
			var abIds = new System.Collections.Generic.List<APIPlugin.AbilityIdentifier>() { ability.id };

			APIPlugin.NewCard.Add(Name, displayName, 0, 4,
				metaCategories, DiskCardGame.CardComplexity.Simple, CardTemple.Nature,
				bloodCost: 2, defaultTex: defaultTexture, abilityIdsParam: abIds
			);
		}

		private static void InitCardsSpawnedFromBloodPond()
		{
			System.Collections.Generic.List<DiskCardGame.CardMetaCategory> metaCategories =
				APIPlugin.CardUtils.getNormalCardMetadata;
			System.Collections.Generic.List<DiskCardGame.Ability> abilities =
				new System.Collections.Generic.List<DiskCardGame.Ability> { DiskCardGame.Ability.Brittle };

			UnityEngine.Texture2D defaultTextureCreature =
				APIPlugin.CardUtils.LoadImageAndGetTexture("scp_354_blood_creature_small.png");
			UnityEngine.Texture2D defaultTextureEntity =
				APIPlugin.CardUtils.LoadImageAndGetTexture("scp_354_blood_entity_small.png");

			const string displayNameCreature = "Blood Creature";
			const string desc = "Spawned from the Blood Pond.";

			APIPlugin.NewCard.Add(NameBloodCreature, displayNameCreature, 1, 1,
				metaCategories, DiskCardGame.CardComplexity.Simple, CardTemple.Nature,
				desc, defaultTex: defaultTextureCreature, abilities: abilities
			);

			const string displayNameEntity = "Blood Entity";

			APIPlugin.NewCard.Add(NameBloodEntity, displayNameEntity, 1, 1,
				metaCategories, DiskCardGame.CardComplexity.Simple, CardTemple.Nature,
				desc, defaultTex: defaultTextureEntity, abilities: abilities
			);
		}
	}
}
