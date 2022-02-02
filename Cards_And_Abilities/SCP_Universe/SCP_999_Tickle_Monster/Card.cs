using APIPlugin;
using DiskCardGame;
using GrimoraMod;

namespace SCP_Universe;

public partial class SCP_Plugin
{
	public const string NameScp999TickleMonster = "scp_universe_SCP_999_TickleMonster";

	public static void Add_Scp999_TickleMonster()
	{
		const string displayName = "Tickle Monster";
		const string desc = "Simply touching SCP-999’s surface causes an immediate euphoria, " +
		                    "which intensifies the longer one is exposed to SCP-999, " +
		                    "and lasts long after separation from the creature";

		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.BuffNeighbours, Ability.DebuffEnemy)
			.SetBaseAttackAndHealth(0, 2)
			.SetBloodCost(1)
			.SetNames(NameScp999TickleMonster, displayName)
			.SetDescription(desc)
			.Build()
		);
	}
}
