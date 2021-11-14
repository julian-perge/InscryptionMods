using System.Collections;
using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace AddAllSCP.SCP_096_Shy_Guy
{
	public class TheSightAbility : AbilityBehaviour
	{
		private PlayableCard foeCardThatDealtDamage;
		private readonly CardModificationInfo mod = new CardModificationInfo() { attackAdjustment = 6 };

		public override Ability Ability { get { return ability; } }

		public static Ability ability;

		public override bool RespondsToOtherCardDie(
			PlayableCard card,
			CardSlot deathSlot,
			bool fromCombat,
			PlayableCard killer
		)
		{
			bool bothCardsAreNotNull = foeCardThatDealtDamage is not null && card is not null;
			bool isEnemySlot = !deathSlot.IsPlayerSlot;
			
			if (isEnemySlot && bothCardsAreNotNull)
			{
				bool isDeathOfCardValid = deathSlot.Card == null || deathSlot.Card.Dead;
				bool isCardThatDiedSameAsFoe = foeCardThatDealtDamage.name.Equals(card.name); 
				return isDeathOfCardValid && isCardThatDiedSameAsFoe;
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
			yield return new WaitForSeconds(0.5f);
			yield return base.PreSuccessfulTriggerSequence();
			base.Card.SwitchToDefaultPortrait();
			base.Card.RemoveTemporaryMod(this.mod, true);
			yield return base.LearnAbility(0.5f);
			foeCardThatDealtDamage = null;
			yield break;
		}

		public override bool RespondsToTakeDamage(PlayableCard source) { return true; }

		public override IEnumerator OnTakeDamage(PlayableCard source)
		{
			// if foeCardThatDealtDamage is null, assign
			foeCardThatDealtDamage ??= source;
			yield return base.PreSuccessfulTriggerSequence();
			base.Card.Anim.StrongNegationEffect();
			base.Card.SwitchToAlternatePortrait();
			base.Card.AddTemporaryMod(this.mod);
			yield return new WaitForSeconds(0.55f);
			yield return base.LearnAbility(0.4f);
			yield break;
		}

		public static NewAbility InitAbility()
		{
			// setup ability
			AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
			info.powerLevel = 0;
			info.rulebookName = "The Sight";
			info.rulebookDescription =
				"It will not stop until all that have seen its face perish. And that includes anything that gets in the way.";
			info.metaCategories = new List<AbilityMetaCategory>()
			{
				AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part1Rulebook
			};

			// get and load artwork
			var imgBytes = System.IO.File.ReadAllBytes("BepInEx/plugins/CardLoader/Artwork/scp_096_ability_small.png");
			Texture2D tex = new Texture2D(2, 2);
			tex.LoadImage(imgBytes);

			// set ability to behavior class
			NewAbility theSightAbility = new NewAbility(info, typeof(TheSightAbility), tex);
			TheSightAbility.ability = theSightAbility.ability;

			return theSightAbility;
		}
	}
}
