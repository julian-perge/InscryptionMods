using System.Collections;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using static SCP_Universe.SCP_Plugin;

namespace SCP_Universe;

public class Withdrawn : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard)
	{
		Log.LogDebug(
			$"Respond to assigned slot? [{otherCard is not null && otherCard.Slot.opposingSlot == base.Card.Slot}]");
		return otherCard is not null && otherCard.Slot.opposingSlot == base.Card.Slot;
	}

	public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
	{
		yield return new WaitForSeconds(0.25f);
		CardInfo cardByName = CardLoader.GetCardByName(NameScp096ShyGuyStages2);
		yield return base.Card.TransformIntoCard(cardByName);
		yield return new WaitForSeconds(0.5f);
	}

	public static NewAbility Create()
	{
		// setup ability
		const string rulebookDescription = "[creature] does not want anyone around, especially if they see it's face.";

		return ApiUtils.CreateAbility<Withdrawn>(rulebookDescription);
	}
}
