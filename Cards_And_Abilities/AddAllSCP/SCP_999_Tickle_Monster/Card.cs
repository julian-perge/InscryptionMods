namespace AddAllSCP.SCP_999_Tickle_Monster
{
	public static class Card
	{
		public const string Name = "SCP_999_TickleMonster";

		public static void InitCard()
		{
			System.Collections.Generic.List<DiskCardGame.CardMetaCategory> metaCategories =
				APIPlugin.CardUtils.getNormalCardMetadata;
			System.Collections.Generic.List<DiskCardGame.Ability> abilities =
				new System.Collections.Generic.List<DiskCardGame.Ability>
				{
					DiskCardGame.Ability.DebuffEnemy, DiskCardGame.Ability.BuffNeighbours
				};

			UnityEngine.Texture2D defaultTexture =
				APIPlugin.CardUtils.getAndloadImageAsTexture("scp_999_small.png");

			const string displayName = "Tickle Monster";
			const string desc =
				"Simply touching SCP-999’s surface causes an immediate euphoria, which intensifies the longer one is exposed to SCP-999, and lasts long after separation from the creature";

			APIPlugin.NewCard.Add(Name, displayName, 0, 2,
				metaCategories, DiskCardGame.CardComplexity.Simple, CardTemple.Nature,
				desc, bloodCost: 1, defaultTex: defaultTexture, abilities: abilities
			);
		}
	}
}
