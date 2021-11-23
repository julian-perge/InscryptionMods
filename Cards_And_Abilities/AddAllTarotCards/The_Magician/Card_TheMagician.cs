using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace AddAllTarotCards.The_Magician
{
	public static class Card_TheMagician
	{
		public const string Name = "Tarot_TheMagician";

		public static CardInfo InitCard()
		{
			NewAbility ability = Ability_TheMagician.InitAbility();

			Texture2D defaultTexture = CardUtils.getAndloadImageAsTexture("card_the_magician.png");

			var displayName = "The Magician";
			var desc = "The Magician. It causes its opponent's abilities to... disappear!";
			var abIds = new List<AbilityIdentifier>() { ability.id };

			NewCard.Add(Name, displayName, 2, 2,
				CardUtils.getRareCardMetadata, CardComplexity.Simple, CardTemple.Nature,
				desc, bloodCost: 2, defaultTex: defaultTexture,
				appearanceBehaviour: CardUtils.getRareAppearance,
				abilityIdsParam: abIds, onePerDeck: true
			);

			return NewCard.cards.Find(i => i.name == Name);
		}
	}
}
