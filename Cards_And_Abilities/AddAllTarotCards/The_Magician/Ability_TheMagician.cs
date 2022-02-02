using static AddAllTarotCards.HarmonyInit;

namespace AddAllTarotCards.The_Magician;

public class Ability_TheMagician : DiskCardGame.AbilityBehaviour
{
	public static DiskCardGame.Ability ability;

	public override DiskCardGame.Ability Ability => ability;

	private DiskCardGame.PlayableCard originalOpposingSlotPlayableCard;

	private System.Collections.Generic.List<string> listMagicianTerms = new System.Collections.Generic.List<string>()
	{
		"Abracadabra!",
		"Acetabularii!",
		"Hocus Pocus!",
		"Time to juggle!",
		"Watch closely!",
		"Prestidigitation!",
		"Aaaand, Presto!",
		"Sim Sala Bim!",
		"Voila!"
	};

	private void displayRandomMagicianTerm()
	{
		base.StartCoroutine(Singleton<DiskCardGame.TextDisplayer>
			.Instance
			.ShowThenClear(listMagicianTerms[UnityEngine.Random.Range(1, listMagicianTerms.Count)], 3f)
		);
	}

	private bool WillTriggerAbility(DiskCardGame.PlayableCard card)
	{
		Log.LogDebug($"[Ability_TheMagician::WillTriggerAbility] card is null? [{card is null}]");
		if (base.Card.Slot)
		{
			Log.LogDebug($"base.Card.Slot [{base.Card.Slot}] is not null");
			if (base.Card.Slot.opposingSlot)
			{
				Log.LogDebug($"base.Card.Slot.opposingSlot [{base.Card.Slot.opposingSlot}] is not null");
				if (base.Card.Slot.opposingSlot.Card)
				{
					Log.LogDebug(
						$"base.Card.Slot.opposingSlot.Card [{base.Card.Slot.opposingSlot.Card}] is not null with card [{base.Card.Slot.opposingSlot.Card.Info.name}]");
				}
			}
		}

		var copy = card;
		if (base.Card.Slot && base.Card.Slot.opposingSlot && base.Card.Slot.opposingSlot.Card)
		{
			Log.LogDebug($"-> card is null? [{card is null}]");
			copy ??= base.Card.Slot.opposingSlot.Card;
		}

		if (copy)
		{
			Log.LogDebug($"Assigning [{copy.Info.name}] to originalOpposingSlotPlayableCard");
			originalOpposingSlotPlayableCard = copy;
			return copy.Info.Abilities.Count > 0;
		}

		return false;
	}

	public override bool RespondsToPlayFromHand()
	{
		return WillTriggerAbility(null);
	}

	public override System.Collections.IEnumerator OnPlayFromHand()
	{
		yield return ActivateAbility(originalOpposingSlotPlayableCard);
		yield break;
	}

	public override bool RespondsToOtherCardDie(DiskCardGame.PlayableCard card, DiskCardGame.CardSlot deathSlot,
		bool fromCombat,
		DiskCardGame.PlayableCard killer)
	{
		return originalOpposingSlotPlayableCard
		       && deathSlot == originalOpposingSlotPlayableCard.Slot
		       && card.Slot == originalOpposingSlotPlayableCard.Slot;
	}

	public override System.Collections.IEnumerator OnOtherCardDie(DiskCardGame.PlayableCard card,
		DiskCardGame.CardSlot deathSlot, bool fromCombat,
		DiskCardGame.PlayableCard killer)
	{
		originalOpposingSlotPlayableCard = null;
		yield break;
	}

	public override bool RespondsToDie(bool wasSacrifice, DiskCardGame.PlayableCard killer)
	{
		return true;
	}

	public override System.Collections.IEnumerator OnDie(bool wasSacrifice, DiskCardGame.PlayableCard killer)
	{
		if (originalOpposingSlotPlayableCard && !originalOpposingSlotPlayableCard.Dead)
		{
			// Log.LogDebug($"Adding back abilities for [{originalOpposingSlotPlayableCard.Info.name}]");
			yield return originalOpposingSlotPlayableCard.TransformIntoCard(originalOpposingSlotPlayableCard.Info);
		}

		yield break;
	}

	public override bool RespondsToOtherCardAssignedToSlot(DiskCardGame.PlayableCard otherCard)
	{
		// we only card to responds to the opposing slot AND if the card has sigils
		return otherCard.Slot != base.Card.Slot && WillTriggerAbility(otherCard);
	}

	public override System.Collections.IEnumerator OnOtherCardAssignedToSlot(DiskCardGame.PlayableCard otherCard)
	{
		yield return base.PreSuccessfulTriggerSequence();

		// Log.LogDebug($"Copied [{copyInfo.name}] with [{copyInfo.abilities.Count}] abilities to remove");
		yield return ActivateAbility(otherCard);

		yield return base.LearnAbility(0.25f);
		yield break;
	}

	private System.Collections.IEnumerator ActivateAbility(DiskCardGame.PlayableCard otherCard)
	{
		DiskCardGame.CardInfo copyInfo = DiskCardGame.CardLoader.GetCardByName(otherCard.Info.name);
		// copyInfo.Abilities = new List<Ability>();

		Singleton<DiskCardGame.ViewManager>.Instance.SwitchToView(DiskCardGame.View.Board);
		base.Card.Anim.StrongNegationEffect();
		displayRandomMagicianTerm();
		yield return new UnityEngine.WaitForSeconds(2.5f);
		yield return otherCard.TransformIntoCard(copyInfo);

		originalOpposingSlotPlayableCard = otherCard;
	}


	public static APIPlugin.NewAbility InitAbility()
	{
		// setup ability
		const string name = "Silence";
		const string desc = "A card bearing this sigil removes the sigils of the card in front of it";
		DiskCardGame.AbilityInfo info = APIPlugin.AbilityInfoUtils.CreateInfoWithDefaultSettings(name, desc);
		var abIds = APIPlugin.AbilityIdentifier.GetID(PluginGuid, info.rulebookName);

		// get art
		UnityEngine.Texture2D tex = APIPlugin.CardUtils.getAndloadImageAsTexture("ability_silence.png");

		// set ability to behavior class
		APIPlugin.NewAbility newAbility = new APIPlugin.NewAbility(info, typeof(Ability_TheMagician), tex, abIds);
		ability = newAbility.ability;

		return newAbility;
	}
}
