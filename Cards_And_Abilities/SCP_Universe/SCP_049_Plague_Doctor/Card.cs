using APIPlugin;
using GrimoraMod;

namespace SCP_Universe;

public partial class SCP_Plugin
{
	public const string NameScp049PlagueDoctor = "scp_universe_SCP_049_PlagueDoctor";

	public static void Add_Scp049_PlagueDoctor()
	{
		const string displayName = "Plague Doctor";

		NewAbility ability = TheCure.InitAbility();
		var abIds = new List<AbilityIdentifier>() { ability.id };

		NewCard.Add(CardBuilder.Builder
				.SetAsNormalCard()
				.SetBaseAttackAndHealth(0, 2)
				.SetBoneCost(6)
				.SetNames(NameScp049PlagueDoctor, displayName)
				.Build(),
			abilityIdsParam: abIds
		);
	}
}
