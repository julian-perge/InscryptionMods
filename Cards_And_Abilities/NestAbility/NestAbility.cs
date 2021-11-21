using System.Collections;
using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using static NestCard.HarmonyInit;

namespace NestCard
{
	public class NestAbility : AbilityBehaviour
	{
		private List<CardInfo> originalDeckCards;

		// todo: list of cards that are mediocre/decent
		private static readonly List<CardInfo> tutorCards = new()
		{
			CardLoader.GetCardByName("Tail_Insect"),
			CardLoader.GetCardByName("RingWorm"),
			CardLoader.GetCardByName("Mantis"),
			CardLoader.GetCardByName("RingWorm")
		};

		public static Ability ability;

		public override Ability Ability { get { return ability; } }

		public override bool RespondsToTakeDamage(PlayableCard source)
		{
			// on first hit, assign original deck cards
			originalDeckCards ??= Singleton<CardDrawPiles>.Instance.Deck.cards;
			return true;
		}

		public override IEnumerator OnTakeDamage(PlayableCard source)
		{
			yield return base.PreSuccessfulTriggerSequence();
			base.Card.Anim.StrongNegationEffect();
			yield return new WaitForSeconds(0.4f);

			// set the current deck to a copy of the tutored cards
			Singleton<CardDrawPiles>.Instance.Deck.cards = new List<CardInfo>(tutorCards);

			// now tutor
			yield return Singleton<CardDrawPiles>.Instance.Deck.Tutor();

			// after tutor, call the 3D pile (not regular CardDrawPiles) to destroy the cards
			// this is needed due to the fact that after tutoring, the game will set the deck to the list of tutored cards
			yield return Singleton<CardDrawPiles3D>.Instance.pile.DestroyCards();

			// now set the deck cards back to the original cards
			Singleton<CardDrawPiles>.Instance.Deck.cards = originalDeckCards;

			// call SpawnCards to correctly show how many cards are left.
			yield return Singleton<CardDrawPiles3D>.Instance.pile.SpawnCards(originalDeckCards.Count, 0.5f);

			yield return base.LearnAbility(0.5f);
			Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
			yield break;
		}

		public static NewAbility InitAbility()
		{
			var rulebookName = "Nest";
			var description = "On taking damage, tutor for a card from a specific draw pile.";

			// setup ability
			AbilityInfo info = AbilityInfoUtils.CreateInfoWithDefaultSettings(rulebookName, description);

			// get and load artwork
			Texture2D sigilTex = CardUtils.getAndloadImageAsTexture("nest_ability_small.png");

			// set ability to behavior class
			NewAbility newAbility = new NewAbility(info, typeof(NestAbility), sigilTex,
				AbilityIdentifier.GetAbilityIdentifier(PluginGuid, info.rulebookName));
			ability = newAbility.ability;

			return newAbility;
		}
	}
}
