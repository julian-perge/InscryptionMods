using System.Collections;
using APIPlugin;
using DiskCardGame;
using static SCP_Universe.SCP_Plugin;

namespace SCP_Universe;

public class PorcelainMask : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	private PlayableCard foeInOpposingSlot;

	public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard)
	{
		return !base.Card.Dead && !otherCard.Dead && otherCard.Slot == base.Card.Slot.opposingSlot;
	}

	public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
	{
		foeInOpposingSlot = otherCard;
		yield break;
	}

	public override bool RespondsToTurnEnd(bool playerTurnEnd)
	{
		Log.LogDebug($"Will respond to turn end? [{playerTurnEnd && foeInOpposingSlot is not null}]");
		return playerTurnEnd && foeInOpposingSlot is not null;
	}

	public override IEnumerator OnTurnEnd(bool playerTurnEnd)
	{
		Log.LogDebug($"Is player end of turn? [{playerTurnEnd}] Foe is [{foeInOpposingSlot.name}]");
		yield return foeInOpposingSlot.TakeDamage(1, base.Card);
	}

	protected internal static NewAbility InitAbility()
	{
		// setup ability
		const string rulebookDescription = "CANNOT BE SACRIFICED. " +
		                                   "[creature] deals 1 damage per turn to the foe in front of it.";

		return ApiUtils.CreateAbility<PorcelainMask>(rulebookDescription);
	}
}
