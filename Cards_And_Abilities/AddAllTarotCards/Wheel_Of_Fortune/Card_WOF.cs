using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace AddAllTarotCards.Wheel_Of_Fortune
{
	public static class Card_WOF
	{
		public const string Name = "Tarot_WheelOfFortune";

		public static CardInfo InitCard()
		{
			NewSpecialAbility ability = SpecialAbility_WheelOfFortune.InitAbility();

			Texture2D defaultTexture =
				CardUtils.getAndloadImageAsTexture("card_wof.png");

			var displayName = "Wheel of Fortune";
			var desc = "The Wheel of Fortune. Its power and health are up to chance.";
			var sId = new List<SpecialAbilityIdentifier>() { ability.id };

			NewCard.Add(Name, displayName, 0, 0,
				CardUtils.getRareCardMetadata, CardComplexity.Simple, CardTemple.Nature,
				desc, bloodCost: 2, defaultTex: defaultTexture,
				appearanceBehaviour: CardUtils.getRareAppearance,
				specialStatIcon: SpecialAbility_WheelOfFortune._iconType,
				specialAbilitiesIdsParam: sId, onePerDeck: true
			);

			return NewCard.cards.Find(i => i.name == Name);
		}
	}
}
