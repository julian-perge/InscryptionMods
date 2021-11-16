using System.Collections;
using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using static NestCard.HarmonyInit;

namespace NestCard
{
	public class NestAbility : BeesOnHit
	{
		private List<CardInfo> originalDeckCards;

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
			originalDeckCards ??= Singleton<CardDrawPiles>.Instance.Deck.cards;
			return true;
		}

		public override IEnumerator OnTakeDamage(PlayableCard source)
		{
			yield return base.PreSuccessfulTriggerSequence();
			base.Card.Anim.StrongNegationEffect();
			yield return new WaitForSeconds(0.4f);

			// Log.LogInfo($"instance count before tutor cards [{Singleton<CardDrawPiles>.Instance.Deck.cards.Count}]");
			Singleton<CardDrawPiles>.Instance.Deck.cards = new List<CardInfo>(tutorCards);

			// Log.LogInfo("yield return instance.Tutor");
			yield return Singleton<CardDrawPiles>.Instance.Deck.Tutor();

			// Log.LogInfo("Destroying main card pile since it contains the tutor cards");
			yield return Singleton<CardDrawPiles3D>.Instance.pile.DestroyCards();

			// Log.LogInfo($"Setting instance cards to originalDeckCards. {originalDeckCards.Count} to add back");
			Singleton<CardDrawPiles>.Instance.Deck.cards = originalDeckCards;

			// Log.LogInfo($"Calling 3D pile spawn cards with og deck count [{originalDeckCards.Count}]");
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
			Texture2D sigilTex =
				CardUtils.getAndloadImageAsTexture("BepInEx/plugins/CardLoader/Artwork/scp_354_blood_pond_ability_small.png");

			// set ability to behavior class
			NewAbility newAbility = new NewAbility(info, typeof(NestAbility), sigilTex,
				AbilityIdentifier.GetAbilityIdentifier(PluginGuid, info.rulebookName));
			ability = newAbility.ability;

			return newAbility;
		}
	}
}
