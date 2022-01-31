namespace AddAllTarotCards.Wheel_Of_Fortune;

public static class Card_WOF
{
	public const string Name = "Tarot_WheelOfFortune";

	public static DiskCardGame.CardInfo InitCard()
	{
		APIPlugin.NewSpecialAbility ability = SpecialAbility_WheelOfFortune.InitAbility();

		UnityEngine.Texture2D defaultTexture =
			APIPlugin.CardUtils.getAndloadImageAsTexture("card_wof.png");

		var displayName = "Wheel of Fortune";
		var desc = "The Wheel of Fortune. Its power and health are up to chance.";
		var sId = new System.Collections.Generic.List<APIPlugin.SpecialAbilityIdentifier>() { ability.id };

		APIPlugin.NewCard.Add(Name, displayName, 0, 0,
			APIPlugin.CardUtils.getRareCardMetadata, DiskCardGame.CardComplexity.Simple, CardTemple.Nature,
			desc, bloodCost: 2, defaultTex: defaultTexture,
			appearanceBehaviour: APIPlugin.CardUtils.getRareAppearance,
			specialStatIcon: SpecialAbility_WheelOfFortune._iconType,
			specialAbilitiesIdsParam: sId, onePerDeck: true
		);

		return APIPlugin.NewCard.cards.Find(i => i.name == Name);
	}
}
