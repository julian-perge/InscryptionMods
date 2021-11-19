using System.Collections;
using System.Linq;
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
			// now only check those filtered cards that have terrain traits
			foreach (var slot in slotsWithCards.Where(slot => slot.Card.Info.HasTrait(Trait.Terrain)))
			{
				yield return new WaitForSeconds(0.1f);
				yield return slot.Card.Die(false, base.Card);
				// this is copied from BeesOnHit Ability
				if (Singleton<ViewManager>.Instance.CurrentView != View.Default)
				{
					yield return new WaitForSeconds(0.2f);
					Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
					yield return new WaitForSeconds(0.2f);
				}

				// Spawn squirrel in your hand
				yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(CardLoader.GetCardByName("Squirrel"), null);
			}

			yield return base.LearnAbility(0.25f);
			yield break;
		}
	}
}
