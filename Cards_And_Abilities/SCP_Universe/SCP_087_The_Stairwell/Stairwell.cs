using System.Collections;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using static SCP_Universe.SCP_Plugin;

namespace SCP_Universe;

public class Stairwell : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	private int turnsTaken = 1;

	public override bool RespondsToUpkeep(bool playerUpkeep)
	{
		return playerUpkeep && (base.Card is not null && !base.Card.Dead);
	}

	public override IEnumerator OnUpkeep(bool playerUpkeep)
	{
		var opposingSlotCard = base.Card.Slot.opposingSlot.Card;
		Log.LogDebug($"[StairwellAbility] Current turns {turnsTaken}, plus 1 [{turnsTaken + 1}]");

		// if it's not the 3rd turn, we don't care to check the opposing slot card
		if (turnsTaken++ != 3)
		{
			yield break;
		}

		// now reset regardless if opposing slot card is null
		turnsTaken = 0;

		if (opposingSlotCard is null)
		{
			yield break;
		}

		Log.LogDebug($"[StairwellAbility] Killing card {opposingSlotCard.name}");

		Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);

		yield return new WaitForSeconds(0.1f);
		yield return opposingSlotCard.Die(false, base.Card);
		yield return base.LearnAbility(0.25f);
		yield return new WaitForSeconds(0.1f);
		yield break;
	}

	public static NewAbility Create()
	{
		// setup ability
		const string rulebookDescription = "At the start of every 3rd turn, " +
		                                   "[creature] automatically kills the card in the opposing slot.";
		return ApiUtils.CreateAbility<Stairwell>(rulebookDescription);
	}
}
