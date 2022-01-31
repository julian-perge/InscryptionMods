namespace HealthForAnts;

public class CardExample
{
	public static void InitCard()
	{
		var newAbility = HarmonyInit.antHealthSpecialAbility;

		var defaultTex = APIPlugin.CardUtils.getAndloadImageAsTexture("dome_ant.png");

		System.Collections.Generic.List<DiskCardGame.CardMetaCategory> metaCategories =
			APIPlugin.CardUtils.getNormalCardMetadata;

		const string name = "DomeAnt";
		const string displayedName = "Dome Ant";
		const string descryption = "Loves to guard his friends";

		DiskCardGame.EvolveParams evolveParams =
			new() { turnsToEvolve = 1, evolution = DiskCardGame.CardLoader.GetCardByName("AntQueen") };
		System.Collections.Generic.List<DiskCardGame.Tribe> tribes = new() { DiskCardGame.Tribe.Insect };
		System.Collections.Generic.List<DiskCardGame.Trait> traits = new() { DiskCardGame.Trait.Ant };

		var sAbIds = new System.Collections.Generic.List<APIPlugin.SpecialAbilityIdentifier>() { newAbility.id };

		APIPlugin.NewCard.Add(
			name, displayedName,
			0, 1,
			APIPlugin.CardUtils.getNormalCardMetadata, DiskCardGame.CardComplexity.Advanced, CardTemple.Nature,
			descryption,
			evolveParams: evolveParams,
			bloodCost: 1,
			defaultTex: defaultTex,
			specialStatIcon: newAbility.statIconInfo.iconType, specialAbilitiesIdsParam: sAbIds,
			tribes: tribes,
			traits: traits
		);
	}
}
