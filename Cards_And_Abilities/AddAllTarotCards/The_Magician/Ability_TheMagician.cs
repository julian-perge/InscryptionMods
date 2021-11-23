using System.Collections;
using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using static AddAllTarotCards.HarmonyInit;

namespace AddAllTarotCards.The_Magician
{
	public class Ability_TheMagician : AbilityBehaviour
	{
		public static Ability ability;

		public override Ability Ability { get { return ability; } }

		private PlayableCard originalOpposingSlotPlayableCard;

		private List<string> listMagicianTerms = new List<string>()
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
			base.StartCoroutine(Singleton<TextDisplayer>
				.Instance
				.ShowThenClear(listMagicianTerms[UnityEngine.Random.RandomRangeInt(1, listMagicianTerms.Count)], 3f)
			);
		}

		private bool WillTriggerAbility(PlayableCard card)
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
				return copy.Info.abilities.Count > 0;
			}

			return false;
		}

		public override bool RespondsToPlayFromHand()
		{
			return WillTriggerAbility(null);
		}

		public override IEnumerator OnPlayFromHand()
		{
			yield return ActivateAbility(originalOpposingSlotPlayableCard);
			yield break;
		}

		public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat,
			PlayableCard killer)
		{
			return originalOpposingSlotPlayableCard
			       && deathSlot == originalOpposingSlotPlayableCard.Slot
			       && card.Slot == originalOpposingSlotPlayableCard.Slot;
		}

		public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat,
			PlayableCard killer)
		{
			originalOpposingSlotPlayableCard = null;
			yield break;
		}

		public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
		{
			return true;
		}

		public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
		{
			if (originalOpposingSlotPlayableCard && !originalOpposingSlotPlayableCard.Dead)
			{
				// Log.LogDebug($"Adding back abilities for [{originalOpposingSlotPlayableCard.Info.name}]");
				yield return originalOpposingSlotPlayableCard.TransformIntoCard(originalOpposingSlotPlayableCard.Info);
			}

			yield break;
		}

		public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard)
		{
			// we only card to responds to the opposing slot AND if the card has sigils
			return otherCard.Slot != base.Card.Slot && WillTriggerAbility(otherCard);
		}

		public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
		{
			yield return base.PreSuccessfulTriggerSequence();

			// Log.LogDebug($"Copied [{copyInfo.name}] with [{copyInfo.abilities.Count}] abilities to remove");
			yield return ActivateAbility(otherCard);

			yield return base.LearnAbility(0.25f);
			yield break;
		}

		private IEnumerator ActivateAbility(PlayableCard otherCard)
		{
			CardInfo copyInfo = CardLoader.GetCardByName(otherCard.Info.name);
			copyInfo.abilities = new List<Ability>();

			Singleton<ViewManager>.Instance.SwitchToView(View.Board);
			base.Card.Anim.StrongNegationEffect();
			displayRandomMagicianTerm();
			yield return new WaitForSeconds(2.5f);
			yield return otherCard.TransformIntoCard(copyInfo);

			originalOpposingSlotPlayableCard = otherCard;
		}


		public static NewAbility InitAbility()
		{
			// setup ability
			string name = "Silence";
			string desc = "A card bearing this sigil removes the sigils of the card in front of it";
			AbilityInfo info = AbilityInfoUtils.CreateInfoWithDefaultSettings(name, desc);
			var abIds = AbilityIdentifier.GetAbilityIdentifier(PluginGuid, info.rulebookName);

			// get art
			Texture2D tex = CardUtils.getAndloadImageAsTexture("ability_silence.png");

			// set ability to behavior class
			NewAbility newAbility = new NewAbility(info, typeof(Ability_TheMagician), tex, abIds);
			ability = newAbility.ability;

			return newAbility;
		}
	}
}
