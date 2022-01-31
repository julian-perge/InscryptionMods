namespace TheWorld;

public class Card_TheWorld
{
	public const string Name = "Tarot_TheWorld";

	public static DiskCardGame.CardInfo InitCard()
	{
		APIPlugin.NewAbility ability = AddAllTarotCards.The_Hanged_Man.Ability_TheHangedMan.InitAbility();

		UnityEngine.Texture2D defaultTexture = APIPlugin.CardUtils.getAndloadImageAsTexture("card_the_world.png");

		const string displayName = "The World";
		const string desc = "The World. Other beings yield before it.";
		var abIds = new System.Collections.Generic.List<APIPlugin.AbilityIdentifier>() { ability.id };

		APIPlugin.NewCard.Add(Name, displayName, 7, 5,
			APIPlugin.CardUtils.getRareCardMetadata, DiskCardGame.CardComplexity.Simple, CardTemple.Nature,
			desc, bloodCost: 4, defaultTex: defaultTexture,
			appearanceBehaviour: APIPlugin.CardUtils.getRareAppearance,
			abilityIdsParam: abIds, onePerDeck: true
		);

		return APIPlugin.NewCard.cards.Find(i => i.name == Name);
	}
}
