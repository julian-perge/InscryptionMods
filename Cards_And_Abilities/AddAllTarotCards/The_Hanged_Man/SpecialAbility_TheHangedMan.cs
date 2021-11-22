using System.Collections;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using static AddAllTarotCards.HarmonyInit;

namespace AddAllTarotCards.The_Hanged_Man
{
	public class SpecialAbility_TheHangedMan : AbilityBehaviour
	{
		public static Ability ability;
		public override Ability Ability { get { return ability; } }

		private int overkillDmg = 0;
		private bool willHangedManDie;
		private bool willFriendlyCardDie;
		private CardModificationInfo modsFromAttackedCard = new();

		public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
		{
			return attacker
			       && base.Card.Slot.IsPlayerSlot == slot.IsPlayerSlot
			       && base.Card.Slot != slot;
		}

		public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
		{
			// base.Card.Health == 2
			// attacker.lastFrameAttack == 3
			// slot.Card.Health == 2
			if (attacker.lastFrameAttack > base.Card.Health)
			{
				// overkillDmg == 1;
				overkillDmg = attacker.lastFrameAttack - base.Card.Health;
				willHangedManDie = true;
				base.StartCoroutine(Singleton<TextDisplayer>
					.Instance
					.ShowThenClear("The Hanged Man has taken their last breath", 3f)
				);
				yield return new WaitForSeconds(0.75f);
				// using example above, friendly card won't die
				willFriendlyCardDie = slot.Card.Health - overkillDmg <= 0;
				Log.LogDebug($"Friendly card [{slot.Card.Info.name}] will die? [{willFriendlyCardDie}]");
			}
			else
			{
				Log.LogDebug($"HangedMan won't die, add [{attacker.lastFrameAttack}] temp health to [{slot.Card.Info.name}]");
				modsFromAttackedCard.healthAdjustment = attacker.lastFrameAttack;
				slot.Card.AddTemporaryMod(modsFromAttackedCard);
			}

			yield break;
		}

		public override bool RespondsToOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
		{
			// Log.LogDebug($"Card [{target.Info.name}] is being attacked by [{attacker.Info.name}]");
			return attacker
			       && base.Card.Slot.IsPlayerSlot == target.Slot.IsPlayerSlot
			       && base.Card.Slot != target.Slot;
		}

		public override IEnumerator OnOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
		{
			Singleton<ViewManager>.Instance.SwitchToView(View.Board);
			yield return new WaitForSeconds(0.1f);
			yield return base.PreSuccessfulTriggerSequence();

			int dmgToTake = amount - overkillDmg;
			Log.LogDebug($"-> The Hanged Man card should take [{dmgToTake}] damage");
			yield return new WaitForSeconds(5.0f);
			base.Card.Anim.StrongNegationEffect();
			yield return base.Card.TakeDamage(dmgToTake, attacker);

			if (!willFriendlyCardDie)
			{
				Log.LogDebug($"-> [{target.Info.name}] being healed for [{dmgToTake}]");
				yield return new WaitForSeconds(5.0f);
				target.HealDamage(dmgToTake);

				Log.LogDebug($"-> [{target.Info.name}] removing temporary mods");
				yield return new WaitForSeconds(5.0f);
				target.RemoveTemporaryMod(modsFromAttackedCard);
			}

			yield return base.LearnAbility();
			yield break;
		}

		public static NewAbility InitAbility()
		{
			// setup ability
			string name = "Martyr";
			string desc = "When another friendly card would take damage, a card with this sigil receives it instead";
			AbilityInfo info = AbilityInfoUtils.CreateInfoWithDefaultSettings(name, desc);
			var abIds = AbilityIdentifier.GetAbilityIdentifier(PluginGuid, info.rulebookName);

			// get Exodia art
			Texture2D tex = CardUtils.getAndloadImageAsTexture("ability_martyr.png");

			// set ability to behavior class
			NewAbility newAbility = new NewAbility(info, typeof(SpecialAbility_TheHangedMan), tex, abIds);
			ability = newAbility.ability;

			return newAbility;
		}
	}
}
