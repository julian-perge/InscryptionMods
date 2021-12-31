namespace AddAllTarotCards.The_Hanged_Man
{
	public class Card_TheHangedMan
	{
		public const string Name = "Tarot_TheHangedMan";

		public static DiskCardGame.CardInfo InitCard()
		{
			APIPlugin.NewAbility ability = Ability_TheHangedMan.InitAbility();

			UnityEngine.Texture2D defaultTexture =
				APIPlugin.CardUtils.getAndloadImageAsTexture("card_the_hanged_man.png");

			var displayName = "The Hanged Man";
			var desc = "The Hanged Man. Its dead weight will provide cover for other creatures.";
			var abIds = new System.Collections.Generic.List<APIPlugin.AbilityIdentifier>() { ability.id };

			APIPlugin.NewCard.Add(Name, displayName, 0, 6,
				APIPlugin.CardUtils.getRareCardMetadata, DiskCardGame.CardComplexity.Simple, CardTemple.Nature,
				desc, bonesCost: 6, defaultTex: defaultTexture,
				appearanceBehaviour: APIPlugin.CardUtils.getRareAppearance,
				abilityIdsParam: abIds, onePerDeck: true
			);

			return APIPlugin.NewCard.cards.Find(i => i.name == Name);
		}
	}
}
