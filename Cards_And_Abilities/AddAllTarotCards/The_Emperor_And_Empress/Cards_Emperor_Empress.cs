using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace TheEmperorAndEmpress
{
	public static class Cards_Emperor_Empress
	{
		public const string Name_Emperor = "Tarot_TheEmperor";
		public const string Name_Empress = "Tarot_TheEmpress";

		public static void InitCards()
		{
			NewSpecialAbility ability = SpecialAbility_Emperor_Empress.InitAbility();

			Texture2D defaultTextureEmperor = CardUtils.getAndloadImageAsTexture("card_emperor.png");
			Texture2D defaultTextureEmpress = CardUtils.getAndloadImageAsTexture("card_empress.png");

			var displayNameEmperor = "The Emperor";
			var displayNameEmpress = "The Empress";

			var desc = "The Empress and the Emperor. Relatively weak by themselves, but strong together.";

			var sId = new List<SpecialAbilityIdentifier>() { ability.id };

			// emperor
			NewCard.Add(Name_Emperor, displayNameEmperor, 2, 2,
				CardUtils.getRareCardMetadata, CardComplexity.Simple, CardTemple.Nature,
				desc, bloodCost: 2, defaultTex: defaultTextureEmperor,
				appearanceBehaviour: CardUtils.getRareAppearance, onePerDeck: true
			);

			// empress
			NewCard.Add(Name_Empress, displayNameEmpress, 2, 2,
				CardUtils.getRareCardMetadata, CardComplexity.Simple, CardTemple.Nature,
				desc, bloodCost: 2, defaultTex: defaultTextureEmpress,
				appearanceBehaviour: CardUtils.getRareAppearance,
				specialAbilitiesIdsParam: sId, onePerDeck: true
			);
		}
	}
}
