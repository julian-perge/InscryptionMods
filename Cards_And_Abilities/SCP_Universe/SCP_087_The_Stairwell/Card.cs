using APIPlugin;
using GrimoraMod;

namespace SCP_Universe;

public partial class SCP_Plugin
{
	public const string NameScp087TheStairwell = "scp_universe_SCP_087_TheStairwell";

	public static void Add_Scp087_TheStairwell()
	{
		NewAbility ability = Stairwell.Create();
		var abIds = new List<AbilityIdentifier>() { ability.id };

		const string displayName = "The Stairwell";
		const string desc =
			"Subjects report and audio recordings confirm the distressed vocalizations from what is presumed to be a child between the ages of █ and ██";

		NewCard.Add(CardBuilder.Builder
				.SetAsNormalCard()
				.SetBaseAttackAndHealth(0, 6)
				.SetBloodCost(2)
				.SetNames(NameScp087TheStairwell, displayName)
				.SetDescription(desc)
				.Build(),
			abilityIdsParam: abIds
		);
	}
}
