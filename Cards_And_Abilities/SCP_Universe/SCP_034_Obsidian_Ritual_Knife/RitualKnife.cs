using System.Collections;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using static SCP_Universe.SCP_Plugin;

namespace SCP_Universe;

public class RitualKnife : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	private PlayableCard foeInOpposingSlot;
	private bool isTransformed;
	private int turnsTaken = 0;
	private int turnsToStayTransformed = 1;

	public override bool RespondsToDealDamage(int amount, PlayableCard target)
	{
		bool doesTargetHaveValidTraits = !target.Info.traits.Exists(t =>
			t is Trait.Terrain or Trait.Pelt or Trait.Giant);
		return !isTransformed && doesTargetHaveValidTraits && !target.Dead;
	}

	public override IEnumerator OnDealDamage(int amount, PlayableCard target)
	{
		foeInOpposingSlot = target;
		turnsToStayTransformed = amount;
		isTransformed = true;
		yield return TransformToCard(target.name);
		var modInfo = new CardModificationInfo { abilities = new List<Ability>() { Ability.Evolve } };
		base.Card.AddTemporaryMod(modInfo);
		yield break;
	}

	public override bool RespondsToTurnEnd(bool playerTurnEnd)
	{
		return playerTurnEnd && foeInOpposingSlot is not null;
	}

	public override IEnumerator OnTurnEnd(bool playerTurnEnd)
	{
		if (++turnsTaken == turnsToStayTransformed)
		{
			turnsTaken = 0;
			yield return TransformToCard(NameScp034RitualKnife);
		}

		yield break;
	}

	public override bool RespondsToOtherCardDie(
		PlayableCard card,
		CardSlot deathSlot,
		bool fromCombat,
		PlayableCard killer
	)
	{
		if (foeInOpposingSlot is not null)
		{
			return foeInOpposingSlot.Slot == deathSlot;
		}

		return false;
	}

	/// <summary>
	/// OnOtherCardDie does not get triggered unless RespondsToOtherCardDie returns true
	/// </summary>
	/// <param name="card">The card that died.</param>
	/// <param name="deathSlot">DeathSlot is the slot where the card died.</param>
	/// <param name="fromCombat">Did it die from combat or sacrifice/bomb/etc</param>
	/// <param name="killer">Who, if applicable, killed the {card}.</param>
	/// <returns></returns>
	public override IEnumerator OnOtherCardDie(
		PlayableCard card,
		CardSlot deathSlot,
		bool fromCombat,
		PlayableCard killer
	)
	{
		Log.LogDebug($"-> Switching back to default knife, card {card.name} is dead");
		yield return new WaitForSeconds(0.5f);
		yield return base.PreSuccessfulTriggerSequence();
		yield return TransformToCard(NameScp034RitualKnife);
		yield return base.LearnAbility(0.5f);
	}

	private IEnumerator TransformToCard(string cardToTransformTo)
	{
		Log.LogDebug($"Transforming into [{cardToTransformTo}]");
		CardInfo cardByName = CardLoader.GetCardByName(cardToTransformTo);

		CardModificationInfo statsMod = GetTransformStatInfo(cardByName);
		statsMod.nameReplacement = base.Card.Info.DisplayedNameEnglish;
		cardByName.Mods.Add(statsMod);

		CardModificationInfo cardModificationInfo2 =
			new CardModificationInfo(Ability.Transformer) { nonCopyable = true };
		cardByName.Mods.Add(cardModificationInfo2);
		cardByName.evolveParams = new EvolveParams { evolution = base.Card.Info, turnsToEvolve = 1 };

		yield return base.Card.TransformIntoCard(cardByName, null);
	}

	private CardModificationInfo GetTransformStatInfo(CardInfo card)
	{
		return new CardModificationInfo
		{
			attackAdjustment = card.Attack, healthAdjustment = card.Health, nonCopyable = true
		};
	}

	protected internal static NewAbility Create()
	{
		const string rulebookDescription =
			"For the amount of damage done to a card, [creature] will transform into the card attacked for that amount of turns. " +
			"Revert if original card dies.";

		return ApiUtils.CreateAbility<RitualKnife>(rulebookDescription);
	}
}
