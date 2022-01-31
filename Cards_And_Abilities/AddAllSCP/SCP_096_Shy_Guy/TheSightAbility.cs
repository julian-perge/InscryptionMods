namespace AddAllSCP.SCP_096_Shy_Guy;

public class TheSightAbility : DiskCardGame.AbilityBehaviour
{
	private readonly System.Collections.Generic.List<DiskCardGame.PlayableCard> cardsThatDealtDamage = new();
	private readonly DiskCardGame.CardModificationInfo mod = new() { attackAdjustment = 6 };
	private readonly System.Collections.Generic.List<int> indexesToRunOver = new System.Collections.Generic.List<int>();

	public override DiskCardGame.Ability Ability { get { return ability; } }

	public static DiskCardGame.Ability ability;

	public override bool RespondsToOtherCardAssignedToSlot(DiskCardGame.PlayableCard otherCard)
	{
		HarmonyInitAll.Log.LogInfo(
			$"Will respond to assigned slot? [{otherCard is not null && cardsThatDealtDamage.Contains(otherCard)}]");
		return otherCard is not null && cardsThatDealtDamage.Contains(otherCard);
	}

	public override System.Collections.IEnumerator OnOtherCardAssignedToSlot(DiskCardGame.PlayableCard otherCard)
	{
		DiskCardGame.CardSlot toLeft = DiskCardGame.BoardManager.Instance.GetAdjacent(base.Card.Slot, true);
		DiskCardGame.CardSlot toRight = DiskCardGame.BoardManager.Instance.GetAdjacent(base.Card.Slot, false);
		Singleton<DiskCardGame.ViewManager>.Instance.SwitchToView(DiskCardGame.View.Board, false, false);
		yield return new UnityEngine.WaitForSeconds(0.25f);

		var pSlots = DiskCardGame.BoardManager.Instance.GetSlots(true);

		for (var i = 0; i < pSlots.Count; i++)
		{
			// turn 2
			// O O X O O
			// s 1 2 3 4, turns

			// turn 3

			indexesToRunOver.Add(i);
			if (otherCard.Slot.opposingSlot == pSlots[i])
			{
				foreach (var index in indexesToRunOver)
				{
				}

				// yield return base.StartCoroutine(this.DoStrafe(toLeft, toRight));
			}
		}


		yield return base.OnOtherCardAssignedToSlot(otherCard);
	}

	public override bool RespondsToOtherCardDie(
		DiskCardGame.PlayableCard card,
		DiskCardGame.CardSlot deathSlot,
		bool fromCombat,
		DiskCardGame.PlayableCard killer
	)
	{
		if (cardsThatDealtDamage.Contains(card))
		{
			cardsThatDealtDamage.Remove(card);
		}

		if (cardsThatDealtDamage.Count > 0 || deathSlot.IsPlayerSlot) { return false; }

		bool isDeathOfCardValid = deathSlot.Card == null || deathSlot.Card.Dead;
		return isDeathOfCardValid;
	}

	/// <summary>
	/// OnOtherCardDie does not get triggered unless RespondsToOtherCardDie returns true
	/// </summary>
	/// <param name="card">The card that died.</param>
	/// <param name="deathSlot">DeathSlot is the slot where the card died.</param>
	/// <param name="fromCombat">Did it die from combat or sacrifice/bomb/etc</param>
	/// <param name="killer">Who, if applicable, killed the {card}.</param>
	/// <returns></returns>
	public override System.Collections.IEnumerator OnOtherCardDie(
		DiskCardGame.PlayableCard card,
		DiskCardGame.CardSlot deathSlot,
		bool fromCombat,
		DiskCardGame.PlayableCard killer
	)
	{
		HarmonyInitAll.Log.LogInfo($"-> Switching back to default portrait");
		yield return new UnityEngine.WaitForSeconds(0.5f);
		yield return base.PreSuccessfulTriggerSequence();
		base.Card.SwitchToDefaultPortrait();
		base.Card.RemoveTemporaryMod(this.mod, true);
		yield return base.LearnAbility(0.5f);
		yield break;
	}

	public override bool RespondsToTakeDamage(DiskCardGame.PlayableCard source) { return true; }

	public override System.Collections.IEnumerator OnTakeDamage(DiskCardGame.PlayableCard source)
	{
		cardsThatDealtDamage.Add(source);
		yield return base.PreSuccessfulTriggerSequence();
		base.Card.Anim.StrongNegationEffect();
		base.Card.SwitchToAlternatePortrait();
		base.Card.AddTemporaryMod(this.mod);
		yield return new UnityEngine.WaitForSeconds(0.55f);
		yield return base.LearnAbility(0.4f);
		yield break;
	}

	public static APIPlugin.NewAbility InitAbility()
	{
		// setup ability
		const string rulebookName = "The Sight";
		const string rulebookDescription = "It will not stop until all that have seen its face perish. " +
		                                   "And that includes anything that gets in the way.";
		DiskCardGame.AbilityInfo info =
			SigilADay_julianperge.SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription);

		// get and load artwork
		var defaultTex = APIPlugin.CardUtils.getAndloadImageAsTexture("scp_096_ability_small.png");

		// set ability to behavior class
		APIPlugin.NewAbility theSightAbility = new APIPlugin.NewAbility(info, typeof(TheSightAbility), defaultTex,
			APIPlugin.AbilityIdentifier.GetID(HarmonyInitAll.PluginGuid, info.rulebookName));
		ability = theSightAbility.ability;

		return theSightAbility;
	}
}
