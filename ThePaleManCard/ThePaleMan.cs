using System;
using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using UnityEngine;

namespace ThePaleManCard
{
	public class ThePaleMan : AbilityBehaviour
	{
		private PlayableCard foeCardThatDealtDamage;
		private CardModificationInfo mod = new CardModificationInfo() { attackAdjustment = 6 };

		public override Ability Ability { get { return ability; } }

		public static Ability ability;

		public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat,
			PlayableCard killer)
		{
			if (!deathSlot.IsPlayerSlot && foeCardThatDealtDamage is not null && card is not null)
			{
				bool isDeathOfFoeCard = deathSlot.Card == null || deathSlot.Card.Dead;
				return isDeathOfFoeCard && foeCardThatDealtDamage.name.Equals(card.name);
			}

			return false;
		}

		public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat,
			PlayableCard killer)
		{
			yield return new WaitForSeconds(0.5f);
			yield return base.PreSuccessfulTriggerSequence();
			base.Card.SwitchToDefaultPortrait();
			base.Card.RemoveTemporaryMod(this.mod, true);
			yield return base.LearnAbility(0.5f);
			yield break;
		}

		public override bool RespondsToTakeDamage(PlayableCard source) { return true; }

		// Token: 0x0600140A RID: 5130 RVA: 0x000117D8 File Offset: 0x0000F9D8
		public override IEnumerator OnTakeDamage(PlayableCard source)
		{
			foeCardThatDealtDamage ??= source;
			yield return base.PreSuccessfulTriggerSequence();
			base.Card.Anim.StrongNegationEffect();
			base.Card.SwitchToAlternatePortrait();
			base.Card.AddTemporaryMod(this.mod);
			yield return new WaitForSeconds(0.55f);
			yield return base.LearnAbility(0.4f);
			yield break;
		}
	}
}
