using System.Collections.Generic;
using AddAllTarotCards.The_Hanged_Man;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace TheWorld
{
	public class Card_TheWorld
	{
		public const string Name = "Tarot_TheWorld";

		public static CardInfo InitCard()
		{
			NewAbility ability = Ability_TheHangedMan.InitAbility();

			Texture2D defaultTexture =
				CardUtils.getAndloadImageAsTexture("card_the_world.png");

			var displayName = "The World";
			var desc = "The World. Other beings yield before it.";
			var abIds = new List<AbilityIdentifier>() { ability.id };

			NewCard.Add(Name, displayName, 7, 5,
				CardUtils.getRareCardMetadata, CardComplexity.Simple, CardTemple.Nature,
				desc, bloodCost: 4, defaultTex: defaultTexture,
				appearanceBehaviour: CardUtils.getRareAppearance,
				abilityIdsParam: abIds, onePerDeck: true
			);

			return NewCard.cards.Find(i => i.name == Name);
		}
	}
}
