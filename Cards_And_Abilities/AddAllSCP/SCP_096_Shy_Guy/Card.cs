using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace AddAllSCP.SCP_096_Shy_Guy
{
	public static class Card
	{
		public const string Name = "SCP_096_ShyGuy";

		public static void InitCard()
		{
			NewAbility ability = TheSightAbility.InitAbility();
			List<CardMetaCategory> metaCategories = CardUtils.getRareCardMetadata;
			List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = CardUtils.getRareAppearance;

			Texture2D defaultTexture =
				CardUtils.getAndloadImageAsTexture("scp_096_hide_small.png");

			Texture2D altTexture =
				CardUtils.getAndloadImageAsTexture("scp_096_attack_small.png");

			var displayName = "Shy Guy";
			var desc = "SCP-096 - Shy Guy. He may seem harmless, but no one has ever seen his face and survived.";

			var abilityIds = new List<AbilityIdentifier>() { ability.id };

			NewCard.Add(Name, displayName, 0, 6,
				metaCategories, CardComplexity.Simple, CardTemple.Nature,
				desc, bloodCost: 2, appearanceBehaviour: appearanceBehaviour,
				defaultTex: defaultTexture, altTex: altTexture, abilityIdsParam: abilityIds
			);
		}

		public static void InitCardStage1()
		{
			NewAbility ability = SCP096_Ability2.InitAbility();
			List<CardMetaCategory> metaCategories = CardUtils.getRareCardMetadata;
			List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = CardUtils.getRareAppearance;

			Texture2D defaultTexture =
				CardUtils.getAndloadImageAsTexture("SCP_096_STAGE1.png");

			var name = "scp_096_stage1";
			var displayName = "SCP-096 (Dormant)";
			var desc = "SCP-096 - Shy Guy. He may seem harmless, but no one has ever seen his face and survived.";

			NewCard.Add(name, displayName, 0, 10,
				metaCategories, CardComplexity.Simple, CardTemple.Nature,
				desc, bloodCost: 0, appearanceBehaviour: appearanceBehaviour,
				defaultTex: defaultTexture, abilityIdsParam: new List<AbilityIdentifier>() { ability.id }
			);
		}

		public static void InitCardStage2()
		{
			List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = CardUtils.getRareAppearance;

			Texture2D defaultTexture =
				CardUtils.getAndloadImageAsTexture("SCP_096_STAGE2.png");

			var name = "scp_096_stage2";
			var displayName = "SCP-096 (Erupting)";
			var desc = "SCP-096 - Shy Guy. He may seem harmless, but no one has ever seen his face and survived.";

			NewCard.Add(name, displayName, 0, 10,
				null, CardComplexity.Simple, CardTemple.Nature,
				desc, bloodCost: 1, appearanceBehaviour: appearanceBehaviour,
				defaultTex: defaultTexture,
				abilities: new List<Ability>() { Ability.Evolve },
				evolveId: new EvolveIdentifier("scp_096_stage3", 2)
			);
		}

		public static void InitCardStage3()
		{
			List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = CardUtils.getRareAppearance;

			Texture2D defaultTexture =
				CardUtils.getAndloadImageAsTexture("SCP_096_STAGE3.png");

			var name = "scp_096_stage3";
			var displayName = "SCP-096 (Active)";
			var desc = "SCP-096 - Shy Guy. He may seem harmless, but no one has ever seen his face and survived.";

			NewCard.Add(name, displayName, 10, 10,
				null, CardComplexity.Simple, CardTemple.Nature,
				desc, bloodCost: 1, appearanceBehaviour: appearanceBehaviour,
				defaultTex: defaultTexture, abilities: new List<Ability>() { Ability.Deathtouch, Ability.PreventAttack }
			);
		}
	}
}
