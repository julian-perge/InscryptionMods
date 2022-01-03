namespace AddAllSCP.SCP_087_The_Stairwell
{
	public static class Card
	{
		public const string Name = "SCP_087_TheStairwell";


		public static void InitCard()
		{
			APIPlugin.NewAbility ability = TheStairwellAbility.InitAbility();
			System.Collections.Generic.List<DiskCardGame.CardMetaCategory> metaCategories =
				APIPlugin.CardUtils.getNormalCardMetadata;

			UnityEngine.Texture2D defaultTexture =
				APIPlugin.CardUtils.LoadImageAndGetTexture("scp_087_small.png");

			const string displayName = "The Stairwell";
			const string desc =
				"Subjects report and audio recordings confirm the distressed vocalizations from what is presumed to be a child between the ages of █ and ██";

			var abIds = new System.Collections.Generic.List<APIPlugin.AbilityIdentifier>() { ability.id };

			APIPlugin.NewCard.Add(Name, displayName, 0, 6,
				metaCategories, DiskCardGame.CardComplexity.Simple, CardTemple.Nature,
				desc, bloodCost: 2, defaultTex: defaultTexture, abilityIdsParam: abIds
			);
		}
	}
}
