namespace AddAllSCP.SCP_049_Plague_Doctor
{
	public class Card
	{
		public const string Name = "SCP_049_PlagueDoctor";

		public static void InitCard()
		{
			APIPlugin.NewAbility ability = TheCureAbility.InitAbility();
			System.Collections.Generic.List<DiskCardGame.CardMetaCategory> metaCategories =
				APIPlugin.CardUtils.getNormalCardMetadata;

			UnityEngine.Texture2D defaultTexture =
				APIPlugin.CardUtils.LoadImageAndGetTexture("scp_049_small.png");

			const string displayName = "Plague Doctor";
			var abIds = new System.Collections.Generic.List<APIPlugin.AbilityIdentifier>() { ability.id };

			APIPlugin.NewCard.Add(Name, displayName, 0, 2,
				metaCategories, DiskCardGame.CardComplexity.Simple, CardTemple.Nature,
				bonesCost: 6, defaultTex: defaultTexture, abilityIdsParam: abIds
			);
		}
	}
}
