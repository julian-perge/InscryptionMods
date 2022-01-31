namespace AddAllSCP.SCP_034_Obsidian_Ritual_Knife;

public class RitualKnifeAbility : DiskCardGame.AbilityBehaviour
{
	public static DiskCardGame.Ability ability;

	public override DiskCardGame.Ability Ability { get { return ability; } }

	private DiskCardGame.PlayableCard foeInOpposingSlot;
	private bool isTransformed;
	private int turnsTaken = 0;
	private int turnsToStayTransformed = 1;

	public override bool RespondsToDealDamage(int amount, DiskCardGame.PlayableCard target)
	{
		bool doesTargetHaveValidTraits = !target.Info.traits.Exists(t =>
			t is DiskCardGame.Trait.Terrain or DiskCardGame.Trait.Pelt or DiskCardGame.Trait.Giant);
		return !isTransformed && doesTargetHaveValidTraits && !target.Dead;
	}

	public override System.Collections.IEnumerator OnDealDamage(int amount, DiskCardGame.PlayableCard target)
	{
		foeInOpposingSlot = target;
		turnsToStayTransformed = amount;
		isTransformed = true;
		yield return TransformToCard(target.name);
		var modInfo = new DiskCardGame.CardModificationInfo
		{
			abilities = new System.Collections.Generic.List<DiskCardGame.Ability>() { DiskCardGame.Ability.Evolve }
		};
		base.Card.AddTemporaryMod(modInfo);
		yield break;
	}

	public override bool RespondsToTurnEnd(bool playerTurnEnd)
	{
		return playerTurnEnd && foeInOpposingSlot is not null;
	}

	public override System.Collections.IEnumerator OnTurnEnd(bool playerTurnEnd)
	{
		if (++turnsTaken == turnsToStayTransformed)
		{
			turnsTaken = 0;
			yield return TransformToCard(SCP_034_Obsidian_Ritual_Knife.Card.Name);
		}

		yield break;
	}

	public override bool RespondsToOtherCardDie(
		DiskCardGame.PlayableCard card,
		DiskCardGame.CardSlot deathSlot,
		bool fromCombat,
		DiskCardGame.PlayableCard killer
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
	public override System.Collections.IEnumerator OnOtherCardDie(
		DiskCardGame.PlayableCard card,
		DiskCardGame.CardSlot deathSlot,
		bool fromCombat,
		DiskCardGame.PlayableCard killer
	)
	{
		HarmonyInitAll.Log.LogInfo($"-> Switching back to default knife, card {card.name} is dead");
		yield return new UnityEngine.WaitForSeconds(0.5f);
		yield return base.PreSuccessfulTriggerSequence();
		yield return TransformToCard(SCP_034_Obsidian_Ritual_Knife.Card.Name);
		yield return base.LearnAbility(0.5f);
		yield break;
	}

	private System.Collections.IEnumerator TransformToCard(string cardToTransformTo)
	{
		HarmonyInitAll.Log.LogInfo($"Transforming into [{cardToTransformTo}]");
		DiskCardGame.CardInfo cardByName = DiskCardGame.CardLoader.GetCardByName(cardToTransformTo);

		DiskCardGame.CardModificationInfo statsMod = GetTransformStatInfo(cardByName);
		statsMod.nameReplacement = base.Card.Info.DisplayedNameEnglish;
		cardByName.Mods.Add(statsMod);

		DiskCardGame.CardModificationInfo cardModificationInfo2 =
			new DiskCardGame.CardModificationInfo(DiskCardGame.Ability.Transformer) { nonCopyable = true };
		cardByName.Mods.Add(cardModificationInfo2);
		cardByName.evolveParams = new DiskCardGame.EvolveParams { evolution = base.Card.Info, turnsToEvolve = 1 };

		yield return base.Card.TransformIntoCard(cardByName, null);
	}

	protected DiskCardGame.CardModificationInfo GetTransformStatInfo(DiskCardGame.CardInfo card)
	{
		return new DiskCardGame.CardModificationInfo
		{
			attackAdjustment = card.Attack, healthAdjustment = card.Health, nonCopyable = true
		};
	}

	protected internal static APIPlugin.NewAbility InitAbility()
	{
		// setup ability
		const string rulebookName = "Physical Copy";
		const string description =
			"For the amount of damage done to a card, transform into the card attacked for that amount of turns. " +
			"Revert if original card dies.";
		DiskCardGame.AbilityInfo info =
			APIPlugin.AbilityInfoUtils.CreateInfoWithDefaultSettings(rulebookName, description);

		// get and load artwork
		UnityEngine.Texture2D tex = APIPlugin.CardUtils.getAndloadImageAsTexture("scp_034_sigil_small.png");

		// set ability to behavior class
		APIPlugin.NewAbility knifeAbility = new APIPlugin.NewAbility(info, typeof(RitualKnifeAbility), tex,
			APIPlugin.AbilityIdentifier.GetID(HarmonyInitAll.PluginGuid, info.rulebookName)
		);
		ability = knifeAbility.ability;

		return knifeAbility;
	}
}
