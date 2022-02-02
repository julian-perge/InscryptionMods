using APIPlugin;
using DiskCardGame;
using GrimoraMod;

namespace SCP_Universe;

public partial class SCP_Plugin
{
	public const string NameScp035PorcelainMask = "scp_universe_SCP_035_PorcelainMask";

	public static void Add_Scp035_PorcelainMask()
	{
		NewAbility ability = PorcelainMask.InitAbility();
		var abIds = new List<AbilityIdentifier>() { ability.id };

		const string displayName = "Porcelain Mask";
		const string desc =
			"A highly corrosive and degenerative viscous liquid constantly seeps from the eye and mouth holes.";

		NewCard.Add(CardBuilder.Builder
				.SetAsNormalCard()
				.SetAbilities(Ability.PreventAttack)
				.SetBaseAttackAndHealth(0, 1)
				.SetBloodCost(1)
				.SetNames(NameScp035PorcelainMask, displayName)
				.SetDescription(desc)
				.SetTraits(Trait.Terrain) // this makes it not sacrificable
				.Build(),
			abilityIdsParam: abIds
		);
	}
}
