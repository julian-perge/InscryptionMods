namespace AddAllSCP.SCP_348_Thinking_Of_You
{
	public static class Card
	{
		public const string Name = "SCP_348_Thinking_Of_You";

		public static void InitCard()
		{
			APIPlugin.NewAbility ability = ThinkingOfYouAbility.InitAbility();
			System.Collections.Generic.List<DiskCardGame.CardMetaCategory> metaCategories =
				APIPlugin.CardUtils.getNormalCardMetadata;

			UnityEngine.Texture2D defaultTexture =
				APIPlugin.CardUtils.getAndloadImageAsTexture("scp_348_small.png");

			const string displayName = "Thinking Of You";
			const string desc =
				"Those who eat from SCP-348 several times often express a feeling of contentment, stating that though they are eating by themselves, they do not feel lonely.";
			var abIds = new System.Collections.Generic.List<APIPlugin.AbilityIdentifier>() { ability.id };

			APIPlugin.NewCard.Add(Name, displayName, 0, 1,
				metaCategories, DiskCardGame.CardComplexity.Simple, CardTemple.Nature,
				desc, bloodCost: 1, defaultTex: defaultTexture, abilityIdsParam: abIds
			);
		}
	}
}
