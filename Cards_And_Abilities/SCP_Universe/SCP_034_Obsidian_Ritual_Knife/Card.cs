using APIPlugin;
using GrimoraMod;

namespace SCP_Universe;

public partial class SCP_Plugin
{
	public const string NameScp034RitualKnife = "scp_universe_SCP_034_RitualKnife";

	public static void Add_Scp034_RitualKnife()
	{
		const string displayName = "Ritual Knife";
		const string desc =
			"SCP-034 is a primitive knife constructed out of pure obsidian. " +
			"Tests reveal that SCP-034 is approximately 1000 years old.";

		NewAbility ability = RitualKnife.Create();
		var abIds = new List<AbilityIdentifier>() { ability.id };

		NewCard.Add(CardBuilder.Builder
				.SetAsNormalCard()
				.SetBaseAttackAndHealth(1, 1)
				.SetBloodCost(1)
				.SetNames(NameScp034RitualKnife, displayName)
				.SetDescription(desc)
				.Build(),
			abilityIdsParam: abIds
		);
	}
}
