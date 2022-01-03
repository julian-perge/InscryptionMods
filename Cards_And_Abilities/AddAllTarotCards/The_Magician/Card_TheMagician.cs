namespace AddAllTarotCards.The_Magician
{
	public static class Card_TheMagician
	{
		public const string Name = "Tarot_TheMagician";

		public static DiskCardGame.CardInfo InitCard()
		{
			APIPlugin.NewAbility ability = Ability_TheMagician.InitAbility();

			UnityEngine.Texture2D defaultTexture = APIPlugin.CardUtils.LoadImageAndGetTexture("card_the_magician.png");

			const string displayName = "The Magician";
			const string desc = "The Magician. It causes its opponent's abilities to... disappear!";
			var abIds = new System.Collections.Generic.List<APIPlugin.AbilityIdentifier>() { ability.id };

			APIPlugin.NewCard.Add(Name, displayName, 2, 2,
				APIPlugin.CardUtils.getRareCardMetadata, DiskCardGame.CardComplexity.Simple, CardTemple.Nature,
				desc, bloodCost: 2, defaultTex: defaultTexture,
				appearanceBehaviour: APIPlugin.CardUtils.getRareAppearance,
				abilityIdsParam: abIds, onePerDeck: true
			);

			return APIPlugin.NewCard.cards.Find(i => i.name == Name);
		}
	}
}
