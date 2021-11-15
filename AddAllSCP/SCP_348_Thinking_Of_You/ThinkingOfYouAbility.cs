using System.Collections;
using System.Collections.Generic;
using System.Linq;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace AddAllSCP.SCP_348_Thinking_Of_You
{
	public class ThinkingOfYouAbility : AbilityBehaviour
	{
		public static Ability ability;

		public override Ability Ability { get { return ability; } }

		private readonly Dictionary<PlayableCard, int> cardsWithTimesHealed = new();

		private static bool CardCanBeHealed(PlayableCard card)
		{
			return !card.Dead && card.Health < card.MaxHealth;
		}
		
		private static bool CardCannotBeHealed(PlayableCard card)
		{
			return !CardCanBeHealed(card);
		}
		
		public override bool RespondsToTurnEnd(bool playerTurnEnd)
		{
			return playerTurnEnd;
		}

		public void doLogicOnCardSlot(CardSlot slot)
		{
			if (slot == null || CardCannotBeHealed(slot.Card))
			{
				return;
			}

			PlayableCard card = slot.Card;

			if (cardsWithTimesHealed.TryGetValue(card, out int total) && total < 3)
			{
				HarmonyInitAll.Log.LogInfo($"Healing [{card.name}. Has been healed [{total}] so far.");
				card.HealDamage(1);
				cardsWithTimesHealed[card] = ++total;
			}
			else
			{
				HarmonyInitAll.Log.LogInfo($"Adding [{card.name} to dictionary.");
				cardsWithTimesHealed.Add(card, 0);
			}
		}

		public override IEnumerator OnTurnEnd(bool playerTurnEnd)
		{
			yield return base.PreSuccessfulTriggerSequence();
			
			CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
			CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);
			
			doLogicOnCardSlot(toLeft);
			doLogicOnCardSlot(toRight);
			
			yield return new WaitForSeconds(0.1f);
			yield return base.LearnAbility(0.5f);
			yield break;
		}

		protected internal static NewAbility InitAbility()
		{
			var rulebookName = "Home Cooking";
			var description =
				"Heal neighbor cards by 1 each turn up to 3 times while neighbor card is not at max health. Does not increase health more than the total of the card.";

			// setup ability
			AbilityInfo info = AbilityInfoUtils.CreateAbilityInfo(rulebookName, description);

			// get and load artwork
			Texture2D sigilTex =
				CardUtils.getAndloadImageAsTexture("BepInEx/plugins/CardLoader/Artwork/scp_348_sigil_small.png");

			// set ability to behavior class
			NewAbility newAbility = new NewAbility(
				info, typeof(ThinkingOfYouAbility), sigilTex,
				AbilityIdentifier.GetAbilityIdentifier(HarmonyInitAll.PluginGuid, info.rulebookName)
			);
			ability = newAbility.ability;

			return newAbility;
		}
	}
}
