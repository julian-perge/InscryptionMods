using APIPlugin;
using DiskCardGame;
using GrimoraMod;
using UnityEngine;

namespace SCP_Universe;

public partial class SCP_Plugin
{
	public const string NameScp096ShyGuy = "scp_universe_SCP_096_ShyGuy";

	public const string NameScp096ShyGuyStages1 = "scp_universe_SCP_096_ShyGuy_Stage1";
	public const string NameScp096ShyGuyStages2 = "scp_universe_SCP_096_ShyGuy_Stage2";
	public const string NameScp096ShyGuyStages3 = "scp_universe_SCP_096_ShyGuy_Stage3";


	public static void Add_Scp096_And_Transforms()
	{
		// AddScp096ShyGuy();
		AddScp096ShyGuyStage1();
		AddScp096ShyGuyStage2();
		AddScp096ShyGuyStage3();
	}

	private static void AddScp096ShyGuy()
	{
		NewAbility ability = TheSight.InitAbility();
		var abIds = new List<AbilityIdentifier>() { ability.id };

		Texture2D defaultTexture =
			CardUtils.getAndloadImageAsTexture("scp_096_hide_small.png");

		Texture2D altTexture =
			CardUtils.getAndloadImageAsTexture("scp_096_attack_small.png");

		const string displayName = "Shy Guy";
		const string desc = "SCP-096 - Shy Guy. He may seem harmless, but no one has ever seen his face and survived.";

		NewCard.Add(CardBuilder.Builder
				.SetAsRareCard()
				.SetBaseAttackAndHealth(0, 6)
				.SetBloodCost(2)
				.SetNames(NameScp096ShyGuy, displayName)
				.SetDescription(desc)
				.Build(),
			abilityIdsParam: abIds
		);
	}

	private static void AddScp096ShyGuyStage1()
	{
		NewAbility ability = Withdrawn.Create();

		Texture2D defaultTexture = CardUtils.getAndloadImageAsTexture("SCP_096_Stage1.png");

		const string displayName = "SCP-096 (Dormant)";
		const string desc = "SCP-096 - Shy Guy. He may seem harmless, but no one has ever seen his face and survived.";

		NewCard.Add(CardBuilder.Builder
				.SetAsRareCard()
				.SetBaseAttackAndHealth(0, 10)
				.SetBloodCost(2)
				.SetNames(NameScp096ShyGuyStages1, displayName)
				.SetDescription(desc)
				.Build(),
			abilityIdsParam: new List<AbilityIdentifier>() { ability.id }
		);
	}

	private static void AddScp096ShyGuyStage2()
	{
		Texture2D defaultTexture = CardUtils.getAndloadImageAsTexture("SCP_096_STAGE2.png");

		const string displayName = "SCP-096 (Erupting)";
		const string desc = "SCP-096 - Shy Guy. He may seem harmless, but no one has ever seen his face and survived.";

		NewCard.Add(CardBuilder.Builder
				.SetAsRareCard()
				.SetAbilities(Ability.Evolve)
				.SetBaseAttackAndHealth(0, 10)
				.SetBloodCost(2)
				.SetNames(NameScp096ShyGuyStages2, displayName)
				.SetDescription(desc)
				.Build(),
			evolveId: new EvolveIdentifier(NameScp096ShyGuyStages3, 2)
		);
	}

	private static void AddScp096ShyGuyStage3()
	{
		Texture2D defaultTexture = CardUtils.getAndloadImageAsTexture("SCP_096_STAGE3.png");

		const string displayName = "SCP-096 (Active)";
		const string desc = "SCP-096 - Shy Guy. He may seem harmless, but no one has ever seen his face and survived.";

		NewCard.Add(CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Ability.Deathtouch, Ability.PreventAttack)
			.SetBaseAttackAndHealth(10, 10)
			.SetBloodCost(2)
			.SetNames(NameScp096ShyGuyStages3, displayName)
			.SetDescription(desc)
			.Build()
		);
	}
}
