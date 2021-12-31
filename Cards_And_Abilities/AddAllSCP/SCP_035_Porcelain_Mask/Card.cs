namespace AddAllSCP.SCP_035_Porcelain_Mask
{
	public static class Card
	{
		public const string Name = "SCP_035_PorcelainMask";

		public static void InitCard()
		{
			APIPlugin.NewAbility ability = MaskAbility.InitAbility();

			System.Collections.Generic.List<DiskCardGame.CardMetaCategory> metaCategories =
				APIPlugin.CardUtils.getNormalCardMetadata;
			System.Collections.Generic.List<DiskCardGame.Ability> abilities =
				new System.Collections.Generic.List<DiskCardGame.Ability> { DiskCardGame.Ability.PreventAttack };
			System.Collections.Generic.List<DiskCardGame.Trait>
				traits = new System.Collections.Generic.List<DiskCardGame.Trait>()
				{
					DiskCardGame.Trait.Terrain
				}; // this makes it not sacrificable

			UnityEngine.Texture2D defaultTexture =
				APIPlugin.CardUtils.getAndloadImageAsTexture("scp_035_small.png");

			const string displayName = "Porcelain Mask";
			const string desc =
				"A highly corrosive and degenerative viscous liquid constantly seeps from the eye and mouth holes.";
			var abIds = new System.Collections.Generic.List<APIPlugin.AbilityIdentifier>() { ability.id };

			APIPlugin.NewCard.Add(Name, displayName, 0, 1,
				metaCategories, DiskCardGame.CardComplexity.Simple, CardTemple.Nature,
				desc, bloodCost: 1, defaultTex: defaultTexture, abilities: abilities, traits: traits, abilityIdsParam: abIds
			);
		}
	}
}
