using System.Collections;
using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace AddAllSCP.SCP_096_Shy_Guy
{
	public class TheSightAbility : AbilityBehaviour
	{
		private List<PlayableCard> cardsThatDealtDamage = new();
		private readonly CardModificationInfo mod = new() { attackAdjustment = 6 };
		private readonly List<int> indexesToRunOver = new List<int>();

		public override Ability Ability { get { return ability; } }

		public static Ability ability;

		public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard)
		{
			HarmonyInitAll.Log.LogInfo(
				$"Will respond to assigned slot? [{otherCard is not null && cardsThatDealtDamage.Contains(otherCard)}]");
			return otherCard is not null && cardsThatDealtDamage.Contains(otherCard);
		}

		public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
		{
			CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
			CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);
			Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
			yield return new WaitForSeconds(0.25f);

			var pSlots = Singleton<BoardManager>.Instance.GetSlots(true);

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
			PlayableCard card,
			CardSlot deathSlot,
			bool fromCombat,
			PlayableCard killer
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
		public override IEnumerator OnOtherCardDie(
			PlayableCard card,
			CardSlot deathSlot,
			bool fromCombat,
			PlayableCard killer
		)
		{
			HarmonyInitAll.Log.LogInfo($"-> Switching back to default portrait");
			yield return new WaitForSeconds(0.5f);
			yield return base.PreSuccessfulTriggerSequence();
			base.Card.SwitchToDefaultPortrait();
			base.Card.RemoveTemporaryMod(this.mod, true);
			yield return base.LearnAbility(0.5f);
			yield break;
		}

		public override bool RespondsToTakeDamage(PlayableCard source) { return true; }

		public override IEnumerator OnTakeDamage(PlayableCard source)
		{
			cardsThatDealtDamage.Add(source);
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
			var defaultTex =
				CardUtils.getAndloadImageAsTexture("BepInEx/plugins/CardLoader/Artwork/scp_096_ability_small.png");

			// set ability to behavior class
			NewAbility theSightAbility = new NewAbility(info, typeof(TheSightAbility), defaultTex,
				AbilityIdentifier.GetAbilityIdentifier(HarmonyInitAll.PluginGuid, info.rulebookName));
			ability = theSightAbility.ability;

			return theSightAbility;
		}
	}
}
