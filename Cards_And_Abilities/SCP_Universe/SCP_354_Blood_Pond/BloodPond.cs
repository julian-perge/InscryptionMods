using System.Collections;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using static SCP_Universe.SCP_Plugin;


namespace SCP_Universe;

public class BloodPond : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	private int turnsTaken = 1;

	public override bool RespondsToTurnEnd(bool playerTurnEnd)
	{
		Log.LogDebug($"[BloodPond.RespondsToTurnEnd] Current turns taken [{turnsTaken}]");
		return playerTurnEnd;
	}

	public override IEnumerator OnTurnEnd(bool playerTurnEnd)
	{
		yield return base.PreSuccessfulTriggerSequence();
		List<CardSlot> slots = BoardManager.Instance
			.GetSlots(true)
			.FindAll(slot => slot.Card is null);
		// if no available slots, list will be empty and won't loop
		Log.LogDebug($"[BloodPond] Number of slots available [{slots.Count}]");
		foreach (var slot in slots)
		{
			var bloodCardToSpawn
				= turnsTaken++ < 3
					? CardLoader.GetCardByName(NameScp354BloodCreature)
					: CardLoader.GetCardByName(NameScp354BloodEntity);

			Log.LogDebug($"-> Spawning a [{bloodCardToSpawn.name}]");
			yield return BoardManager.Instance.CreateCardInSlot(bloodCardToSpawn, slot, 0.1f, true);
			break; // only spawn one, then break out of loop and end resolve.
		}

		yield return new WaitForSeconds(0.1f);
		yield return base.LearnAbility(0.5f);
		yield break;
	}

	protected internal static NewAbility Create()
	{
		const string description =
			$"[creature] will spawn [define:{NameScp354BloodCreature}] at the end of your turn in a random slot. " +
			$"After 3 turns, spawn [define:{NameScp354BloodEntity}].";

		return ApiUtils.CreateAbility<BloodPond>(description);
	}
}
