using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace Exodia
{
	public static class Card
	{
		public const string Name = "Exodia";
		public const string NameLeftArm = "Left_Arm_Of_Exodia";
		public const string NameRightArm = "Right_Arm_Of_Exodia";

		public static void InitCards()
		{
			InitCardExodia();
			InitCardArms();
		}

		private static void InitCardExodia()
		{
			NewAbility ability = ExodiaAbility.InitAbility();
			List<CardMetaCategory> metaCategories = CardUtils.getNormalCardMetadata;

			Texture2D defaultTexture =
				CardUtils.getAndloadImageAsTexture("BepInEx/plugins/CardLoader/Artwork/exodia_card_small.png");

			var displayName = "The Forbidden One";
			var desc = "";
			var abIds = new List<AbilityIdentifier>() { ability.id };

			NewCard.Add(Name, displayName, 1, 1,
				metaCategories, CardComplexity.Simple, CardTemple.Nature,
				desc, bloodCost: 1, defaultTex: defaultTexture, abilityIdsParam: abIds, onePerDeck: true
			);
		}

		private static void InitCardArms()
		{
			List<CardMetaCategory> metaCategories = CardUtils.getNormalCardMetadata;

			Texture2D defaultTexLeftArm =
				CardUtils.getAndloadImageAsTexture("BepInEx/plugins/CardLoader/Artwork/exodia_left_arm_small.png");

			Texture2D defaultTexRightArm =
				CardUtils.getAndloadImageAsTexture("BepInEx/plugins/CardLoader/Artwork/exodia_right_arm_small.png");

			var displayNameLeft = "Left Arm Of Exodia";
			var displayNameRight = "Right Arm Of Exodia";

			var desc = "One of the arms of The Forbidden One";

			// Left Arm
			NewCard.Add(NameLeftArm, displayNameLeft, 0, 1,
				metaCategories, CardComplexity.Simple, CardTemple.Nature,
				desc, bloodCost: 1, defaultTex: defaultTexLeftArm, onePerDeck: true
			);

			// Right Arm
			NewCard.Add(NameRightArm, displayNameRight, 0, 1,
				metaCategories, CardComplexity.Simple, CardTemple.Nature,
				desc, bloodCost: 1, defaultTex: defaultTexRightArm, onePerDeck: true
			);
		}
	}
}
