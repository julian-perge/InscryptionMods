using APIPlugin;
using GrimoraMod;

namespace SCP_Universe;

public partial class SCP_Plugin
{
	public const string NameScp348ThinkingOfYou = "scp_universe_SCP_348_ThinkingOfYou";

	public static void Add_Scp348_ThinkingOfYou()
	{
		NewAbility ability = ThinkingOfYou.InitAbility();
		var abIds = new List<AbilityIdentifier>() { ability.id };

		const string displayName = "Thinking Of You";
		const string desc = "Those who eat from SCP-348 several times often express a feeling of contentment, " +
		                    "stating that though they are eating by themselves, they do not feel lonely.";

		NewCard.Add(CardBuilder.Builder
				.SetAsNormalCard()
				.SetBaseAttackAndHealth(0, 1)
				.SetBloodCost(1)
				.SetNames(NameScp348ThinkingOfYou, displayName)
				.SetDescription(desc)
				.Build(),
			abilityIdsParam: abIds
		);
	}
}
