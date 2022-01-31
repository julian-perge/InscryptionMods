namespace AddAllSCP.SCP_096_Shy_Guy;

public static class Card
{
	public const string Name = "SCP_096_ShyGuy";

	public static void InitCard()
	{
		APIPlugin.NewAbility ability = TheSightAbility.InitAbility();
		System.Collections.Generic.List<DiskCardGame.CardMetaCategory> metaCategories =
			APIPlugin.CardUtils.getRareCardMetadata;
		System.Collections.Generic.List<DiskCardGame.CardAppearanceBehaviour.Appearance> appearanceBehaviour =
			APIPlugin.CardUtils.getRareAppearance;

		UnityEngine.Texture2D defaultTexture =
			APIPlugin.CardUtils.getAndloadImageAsTexture("scp_096_hide_small.png");

		UnityEngine.Texture2D altTexture =
			APIPlugin.CardUtils.getAndloadImageAsTexture("scp_096_attack_small.png");

		const string displayName = "Shy Guy";
		const string desc = "SCP-096 - Shy Guy. He may seem harmless, but no one has ever seen his face and survived.";

		var abilityIds = new System.Collections.Generic.List<APIPlugin.AbilityIdentifier>() { ability.id };

		APIPlugin.NewCard.Add(Name, displayName, 0, 6,
			metaCategories, DiskCardGame.CardComplexity.Simple, CardTemple.Nature,
			desc, bloodCost: 2, appearanceBehaviour: appearanceBehaviour,
			defaultTex: defaultTexture, altTex: altTexture, abilityIdsParam: abilityIds
		);
	}

	public static void InitCardStage1()
	{
		APIPlugin.NewAbility ability = SCP096_Ability2.InitAbility();
		System.Collections.Generic.List<DiskCardGame.CardMetaCategory> metaCategories =
			APIPlugin.CardUtils.getRareCardMetadata;
		System.Collections.Generic.List<DiskCardGame.CardAppearanceBehaviour.Appearance> appearanceBehaviour =
			APIPlugin.CardUtils.getRareAppearance;

		UnityEngine.Texture2D defaultTexture =
			APIPlugin.CardUtils.getAndloadImageAsTexture("SCP_096_STAGE1.png");

		const string name = "scp_096_stage1";
		const string displayName = "SCP-096 (Dormant)";
		const string desc = "SCP-096 - Shy Guy. He may seem harmless, but no one has ever seen his face and survived.";

		APIPlugin.NewCard.Add(name, displayName, 0, 10,
			metaCategories, DiskCardGame.CardComplexity.Simple, CardTemple.Nature,
			desc, bloodCost: 0, appearanceBehaviour: appearanceBehaviour,
			defaultTex: defaultTexture,
			abilityIdsParam: new System.Collections.Generic.List<APIPlugin.AbilityIdentifier>() { ability.id }
		);
	}

	public static void InitCardStage2()
	{
		System.Collections.Generic.List<DiskCardGame.CardAppearanceBehaviour.Appearance> appearanceBehaviour =
			APIPlugin.CardUtils.getRareAppearance;

		UnityEngine.Texture2D defaultTexture =
			APIPlugin.CardUtils.getAndloadImageAsTexture("SCP_096_STAGE2.png");

		const string name = "scp_096_stage2";
		const string displayName = "SCP-096 (Erupting)";
		const string desc = "SCP-096 - Shy Guy. He may seem harmless, but no one has ever seen his face and survived.";

		APIPlugin.NewCard.Add(name, displayName, 0, 10,
			null, DiskCardGame.CardComplexity.Simple, CardTemple.Nature,
			desc, bloodCost: 1, appearanceBehaviour: appearanceBehaviour,
			defaultTex: defaultTexture,
			abilities: new System.Collections.Generic.List<DiskCardGame.Ability>() { DiskCardGame.Ability.Evolve },
			evolveId: new APIPlugin.EvolveIdentifier("scp_096_stage3", 2)
		);
	}

	public static void InitCardStage3()
	{
		System.Collections.Generic.List<DiskCardGame.CardAppearanceBehaviour.Appearance> appearanceBehaviour =
			APIPlugin.CardUtils.getRareAppearance;

		UnityEngine.Texture2D defaultTexture =
			APIPlugin.CardUtils.getAndloadImageAsTexture("SCP_096_STAGE3.png");

		const string name = "scp_096_stage3";
		const string displayName = "SCP-096 (Active)";
		const string desc = "SCP-096 - Shy Guy. He may seem harmless, but no one has ever seen his face and survived.";

		APIPlugin.NewCard.Add(name, displayName, 10, 10,
			null, DiskCardGame.CardComplexity.Simple, CardTemple.Nature,
			desc, bloodCost: 1, appearanceBehaviour: appearanceBehaviour,
			defaultTex: defaultTexture,
			abilities: new System.Collections.Generic.List<DiskCardGame.Ability>()
			{
				DiskCardGame.Ability.Deathtouch, DiskCardGame.Ability.PreventAttack
			}
		);
	}
}
