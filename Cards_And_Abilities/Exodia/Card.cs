namespace Exodia;

public partial class Plugin
{
	public const string Name = "Exodia";

	public static void AddExodiaCards()
	{
		AddCardExodia();
	}

	private static void AddCardExodia()
	{
		// find the ability since it will be created in SigilADay
		APIPlugin.NewAbility ability =
			APIPlugin.NewAbility.abilities.Find(ab => ab.ability == SigilADay_julianperge.Exodia.ability);
		System.Collections.Generic.List<DiskCardGame.CardMetaCategory> metaCategories =
			APIPlugin.CardUtils.getNormalCardMetadata;

		UnityEngine.Texture2D defaultTexture =
			SigilADay_julianperge.SigilUtils.LoadTextureFromResource(Properties.Resources.card_exodia);

		var displayName = "The Forbidden One";
		var desc = "WHAT, THAT'S NOT POSSIBLE?!";
		var abIds = new System.Collections.Generic.List<APIPlugin.AbilityIdentifier>() { ability.id };

		APIPlugin.NewCard.Add(Name, displayName, 1, 1,
			metaCategories, DiskCardGame.CardComplexity.Simple, CardTemple.Nature,
			desc, bloodCost: 1, defaultTex: defaultTexture, abilityIdsParam: abIds, onePerDeck: true
		);
	}
}
