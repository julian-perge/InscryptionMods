namespace AddAllTarotCards.The_Emperor_And_Empress;

public static class Cards_Emperor_Empress
{
	public const string Name_Emperor = "Tarot_TheEmperor";
	public const string Name_Empress = "Tarot_TheEmpress";

	public static void InitCards()
	{
		APIPlugin.NewSpecialAbility ability = SpecialAbility_Emperor_Empress.InitAbility();

		UnityEngine.Texture2D defaultTextureEmperor = APIPlugin.CardUtils.getAndloadImageAsTexture("card_emperor.png");
		UnityEngine.Texture2D defaultTextureEmpress = APIPlugin.CardUtils.getAndloadImageAsTexture("card_empress.png");

		const string displayNameEmperor = "The Emperor";
		const string displayNameEmpress = "The Empress";

		const string desc = "The Empress and the Emperor. Relatively weak by themselves, but strong together.";

		var sId = new System.Collections.Generic.List<APIPlugin.SpecialAbilityIdentifier>() { ability.id };

		// emperor
		APIPlugin.NewCard.Add(Name_Emperor, displayNameEmperor, 2, 2,
			APIPlugin.CardUtils.getRareCardMetadata, DiskCardGame.CardComplexity.Simple, CardTemple.Nature,
			desc, bloodCost: 2, defaultTex: defaultTextureEmperor,
			appearanceBehaviour: APIPlugin.CardUtils.getRareAppearance, onePerDeck: true
		);

		// empress
		APIPlugin.NewCard.Add(Name_Empress, displayNameEmpress, 2, 2,
			APIPlugin.CardUtils.getRareCardMetadata, DiskCardGame.CardComplexity.Simple, CardTemple.Nature,
			desc, bloodCost: 2, defaultTex: defaultTextureEmpress,
			appearanceBehaviour: APIPlugin.CardUtils.getRareAppearance,
			specialAbilitiesIdsParam: sId, onePerDeck: true
		);
	}
}
