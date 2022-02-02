using APIPlugin;
using DiskCardGame;
using GrimoraMod;

namespace SCP_Universe;

public partial class SCP_Plugin
{
	public const string NameScp354BloodPond = "scp_universe_SCP_354_BloodPond";
	public const string NameScp354BloodCreature = "scp_universe_SCP_354_BloodCreature";
	public const string NameScp354BloodEntity = "scp_universe_SCP_354_BloodEntity";

	public static void Add_Scp354_BloodPond_And_Creatures()
	{
		Add_Scp354_BloodPond();
		Add_Scp354_Creatures();
	}

	private static void Add_Scp354_BloodPond()
	{
		NewAbility ability = BloodPond.Create();
		var abIds = new List<AbilityIdentifier>() { ability.id };

		const string displayName = "Blood Pond";

		NewCard.Add(CardBuilder.Builder
				.SetAsNormalCard()
				.SetBaseAttackAndHealth(0, 4)
				.SetBloodCost(2)
				.SetNames(NameScp354BloodPond, displayName)
				.Build(),
			abilityIdsParam: abIds
		);
	}

	private static void Add_Scp354_Creatures()
	{
		const string displayNameCreature = "Blood Creature";
		const string desc = "Spawned from the Blood Pond.";

		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Brittle)
			.SetBaseAttackAndHealth(1, 1)
			.SetNames(NameScp354BloodCreature, displayNameCreature)
			.SetDescription(desc)
			.Build()
		);

		const string displayNameEntity = "Blood Entity";

		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Brittle)
			.SetBaseAttackAndHealth(2, 2)
			.SetNames(NameScp354BloodEntity, displayNameEntity)
			.SetDescription(desc)
			.Build()
		);
	}
}
