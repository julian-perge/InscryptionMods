using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace AddAllTarotCards.The_Hanged_Man
{
	public class Card_TheHangedMan
	{
		public const string Name = "Tarot_TheHangedMan";

		public static CardInfo InitCard()
		{
			NewAbility ability = SpecialAbility_TheHangedMan.InitAbility();

			Texture2D defaultTexture =
				CardUtils.getAndloadImageAsTexture("card_the_hanged_man.png");

			var displayName = "The Hanged Man";
			var desc = "The Hanged Man. Its dead weight will provide cover for other creatures.";
			var abIds = new List<AbilityIdentifier>() { ability.id };

			NewCard.Add(Name, displayName, 0, 6,
				CardUtils.getRareCardMetadata, CardComplexity.Simple, CardTemple.Nature,
				desc, bonesCost: 6, defaultTex: defaultTexture,
				appearanceBehaviour: CardUtils.getRareAppearance,
				abilityIdsParam: abIds, onePerDeck: true
			);

			return NewCard.cards.Find(i => i.name == Name);
		}
	}
}
