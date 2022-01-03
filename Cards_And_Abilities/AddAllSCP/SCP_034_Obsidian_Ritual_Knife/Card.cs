namespace AddAllSCP.SCP_034_Obsidian_Ritual_Knife
{
	public static class Card
	{
		public const string Name = "SCP_034_ObsidianRitualKnife";

		public static void InitCard()
		{
			APIPlugin.NewAbility ability = RitualKnifeAbility.InitAbility();

			System.Collections.Generic.List<DiskCardGame.CardMetaCategory> metaCategories =
				APIPlugin.CardUtils.getNormalCardMetadata;

			UnityEngine.Texture2D defaultTexture =
				APIPlugin.CardUtils.LoadImageAndGetTexture("scp_034_small.png");

			const string displayName = "Ritual Knife";
			const string desc =
				"SCP-034 is a primitive knife constructed out of pure obsidian. " +
				"Tests reveal that SCP-034 is approximately 1000 years old.";
			var abIds = new System.Collections.Generic.List<APIPlugin.AbilityIdentifier>() { ability.id };

			APIPlugin.NewCard.Add(Name, displayName, 1, 1,
				metaCategories, DiskCardGame.CardComplexity.Simple, CardTemple.Nature,
				desc, bloodCost: 1, defaultTex: defaultTexture, abilityIdsParam: abIds
			);
		}
	}
}
