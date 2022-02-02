using APIPlugin;
using DiskCardGame;
using UnityEngine;
using static SCP_Universe.SCP_Plugin;


namespace SCP_Universe;

public class ThinkingOfYou : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	private readonly Dictionary<PlayableCard, int> cardsWithTimesHealed = new();

	private static bool CardCanBeHealed(PlayableCard card)
	{
		return !card.Dead && card.Health < card.MaxHealth;
	}

	private static bool CardCannotBeHealed(PlayableCard card)
	{
		return !CardCanBeHealed(card);
	}

	public override bool RespondsToTurnEnd(bool playerTurnEnd)
	{
		return playerTurnEnd;
	}

	public void doLogicOnCardSlot(CardSlot slot)
	{
		if (slot == null || CardCannotBeHealed(slot.Card))
		{
			return;
		}

		PlayableCard card = slot.Card;

		if (cardsWithTimesHealed.TryGetValue(card, out int total) && total < 3)
		{
			Log.LogDebug($"Healing [{card.name}. Has been healed [{total}] so far.");
			card.HealDamage(1);
			cardsWithTimesHealed[card] = ++total;
		}
		else
		{
			Log.LogDebug($"Adding [{card.name} to dictionary.");
			cardsWithTimesHealed.Add(card, 0);
		}
	}

	public override System.Collections.IEnumerator OnTurnEnd(bool playerTurnEnd)
	{
		yield return base.PreSuccessfulTriggerSequence();

		CardSlot toLeft = BoardManager.Instance.GetAdjacent(base.Card.Slot, true);
		CardSlot toRight = BoardManager.Instance.GetAdjacent(base.Card.Slot, false);

		doLogicOnCardSlot(toLeft);
		doLogicOnCardSlot(toRight);

		yield return new WaitForSeconds(0.1f);
		yield return base.LearnAbility(0.5f);
	}

	protected internal static NewAbility InitAbility()
	{
		const string rulebookDescription =
			"[creature] will heal neighbor cards by 1 each turn up to 3 times while neighbor card is not at max health. " +
			"Does not increase health more than the total of the card.";

		return ApiUtils.CreateAbility<ThinkingOfYou>(rulebookDescription);
	}
}
