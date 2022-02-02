using System.Collections;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using static SCP_Universe.SCP_Plugin;

namespace SCP_Universe;

public class TheCure : AbilityBehaviour
{
	public static Ability ability;
	public override Ability Ability => ability;


	public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot,
		bool fromCombat,
		PlayableCard killer)
	{
		// can't rez non-living things. Moon causes bugs.
		if (card.Info.traits.Exists(trait =>
			    trait is Trait.Terrain or Trait.Pelt or Trait.Giant))
		{
			return false;
		}

		// base.Card.OpponentCard is called in BoardManager.AssignCardToSlot
		bool isBaseCardValid = base.Card.OnBoard && deathSlot.Card != base.Card;
		bool isValidCardDeath = deathSlot.Card is not null && deathSlot.Card == card;
		return (fromCombat && isBaseCardValid && isValidCardDeath) || IsPlagueDoctorDeath(card);
	}

	private bool IsPlagueDoctorDeath(PlayableCard card)
	{
		return card is not null && card.name.Contains("SCP_049_PlagueDoctor");
	}

	private bool IsPlagueDoctorCuredCard(PlayableCard card)
	{
		return card.name.Contains("\"Cured\"");
	}

	public override IEnumerator OnOtherCardDie(PlayableCard card,
		CardSlot deathSlot,
		bool fromCombat,
		PlayableCard killer)
	{
		yield return base.PreSuccessfulTriggerSequence();
		List<CardSlot> slots = BoardManager.Instance.GetSlots(true);
		Log.LogDebug(
			$"[TheCure] Card is null {card is null} deathslot card is null {deathSlot is null} Killer {killer.name}");
		if (IsPlagueDoctorDeath(card))
		{
			Log.LogDebug("-> Is Plague Doctor death card is true");
			// loop through player slots and only check for cards with "Cured" in the name
			foreach (var slot in slots.Where(slot => slot is not null && IsPlagueDoctorCuredCard(slot.Card)))
			{
				yield return slot.Card.Die(false, null, true);
			}
		}
		else
		{
			string nameOfCard = card.Info.name;
			Log.LogDebug($"-> Is Plague Doctor death card was false, checking card name is {nameOfCard}");
			// if not Plague Doctor death, set to 1/1 and spawn on an open slot on your side of the field
			var filteredSlots = slots.Where(slot => slot is not null && slot.Card is null);
			Log.LogDebug($"-> Number of filtered slots [{filteredSlots.Count()}]");
			foreach (var slot in slots.Where(slot => slot is not null && slot.Card is null))
			{
				CardInfo cardByName = CardLoader.GetCardByName(nameOfCard);
				CardModificationInfo cardModificationInfo = new CardModificationInfo
				{
					attackAdjustment = -cardByName.Attack + 1,
					healthAdjustment = -cardByName.Health + 1,
					bloodCostAdjustment = -cardByName.BloodCost,
					bonesCostAdjustment = -cardByName.BonesCost,
					energyCostAdjustment = -cardByName.EnergyCost,
					nullifyGemsCost = true,
					nameReplacement = nameOfCard + " \"Cured\""
				};
				cardByName.Mods.Add(cardModificationInfo);
				yield return BoardManager.Instance.CreateCardInSlot(cardByName, slot, 0.1f, true);
				break;
			}
		}

		yield return new WaitForSeconds(0.1f);
		yield return base.LearnAbility(0.5f);
		yield break;
	}

	public static NewAbility InitAbility()
	{
		const string rulebookDescription = "Any card that dies from combat, [creature] will" +
		                                   " spawn a 1/1 \"Cured\" version of the card in an open slot on your side.";

		return ApiUtils.CreateAbility<TheCure>(rulebookDescription);
	}
}
