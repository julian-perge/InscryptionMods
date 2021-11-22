using System.Collections;
using System.Linq;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace Excavator
{
	public class ExcavatorAbility : AbilityBehaviour
	{
		public static Ability ability;
		public override Ability Ability { get { return ability; } }

		public override bool RespondsToResolveOnBoard()
		{
			return true;
		}

		public override IEnumerator OnResolveOnBoard()
		{
			yield return base.PreSuccessfulTriggerSequence();

			// only get a list of slots that are not null and have cards in them
			var slotsWithCards = Singleton<BoardManager>
				.Instance
				.GetSlots(true)
				.Where(slot => slot && slot.Card);

			Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
			// all the boulders need to die before we switch to our hand view to spawn the Squirrels
			int numberOfCardsToSpawn = 0;

			// now only check those filtered cards that have terrain traits
			foreach (var slot in slotsWithCards.Where(slot => slot.Card.Info.HasTrait(Trait.Terrain)))
			{
				numberOfCardsToSpawn++;
				yield return new WaitForSeconds(0.1f);
				yield return slot.Card.Die(false, base.Card);
			}

			// this is copied from BeesOnHit Ability
			// switch to hand view
			if (Singleton<ViewManager>.Instance.CurrentView != View.Default)
			{
				yield return new WaitForSeconds(0.2f);
				Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
				yield return new WaitForSeconds(0.2f);
			}

			for (int i = 0; i < numberOfCardsToSpawn; i++)
			{
				// Spawn squirrel in your hand
				yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(CardLoader.GetCardByName("Squirrel"), null);
			}

			yield return base.LearnAbility(0.25f);
			yield break;
		}

		public static NewAbility InitAbility()
		{
			// setup ability
			string name = "Excavator";
			string desc =
				"When placed on the board, remove all Terrain cards and for each card removed, place a Squirrel in your hand. ";
			AbilityInfo info = AbilityInfoUtils.CreateInfoWithDefaultSettings(name, desc);
			var abIds = AbilityIdentifier.GetAbilityIdentifier(HarmonyInit.PluginGuid, info.rulebookName);

			Texture2D tex = CardUtils.getAndloadImageAsTexture("ability_excavator.png");

			// set ability to behavior class
			NewAbility newAbility = new NewAbility(info, typeof(ExcavatorAbility), tex, abIds);
			ability = newAbility.ability;

			return newAbility;
		}
	}
}
