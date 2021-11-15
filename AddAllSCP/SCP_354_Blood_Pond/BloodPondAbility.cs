using System.Collections;
using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace AddAllSCP.SCP_354_Blood_Pond
{
	public class BloodPondAbility : AbilityBehaviour
	{
		public static Ability ability;

		public override Ability Ability { get { return ability; } }

		private int turnsTaken = 1;

		public override bool RespondsToTurnEnd(bool playerTurnEnd)
		{
			HarmonyInitAll.Log.LogDebug($"[RespondsToTurnEnd] Current turns taken [{turnsTaken}]");
			return playerTurnEnd;
		}

		public override IEnumerator OnTurnEnd(bool playerTurnEnd)
		{
			yield return base.PreSuccessfulTriggerSequence();
			List<CardSlot> slots = Singleton<BoardManager>.Instance.GetSlots(true)
				.FindAll(slot => slot is not null && slot.Card is null);
			// if no available slots, list will be empty and won't loop
			HarmonyInitAll.Log.LogDebug($"[BloodPond] Number of slots available [{slots.Count}]");
			foreach (var slot in slots)
			{
				var bloodCardToSpawn
					= turnsTaken++ < 3
						? CardLoader.GetCardByName("SCP_354_BloodCreature")
						: CardLoader.GetCardByName("SCP_354_BloodEntity");

				HarmonyInitAll.Log.LogDebug($"-> Spawning a [{bloodCardToSpawn.displayedName}]");
				yield return Singleton<BoardManager>.Instance.CreateCardInSlot(bloodCardToSpawn, slot, 0.1f, true);
				break; // only spawn one, then break out of loop and end resolve.
			}

			yield return new WaitForSeconds(0.1f);
			yield return base.LearnAbility(0.5f);
			yield break;
		}

		protected internal static NewAbility InitAbility()
		{
			var rulebookName = "Blood Pond";
			var description =
				"Spawn a Blood Creature (1/1 w/ Brittle) at the end of your turn in a random slot. After 3 turns, spawn a Blood Entity (1/1 w/ Brittle).";

			// setup ability
			AbilityInfo info = AbilityInfoUtils.CreateAbilityInfo(rulebookName, description);

			// get and load artwork
			Texture2D sigilTex =
				CardUtils.getAndloadImageAsTexture("BepInEx/plugins/CardLoader/Artwork/scp_354_blood_pond_ability_small.png");

			// set ability to behavior class
			NewAbility bloodPondAbility = new NewAbility(info, typeof(BloodPondAbility), sigilTex,
				AbilityIdentifier.GetAbilityIdentifier(HarmonyInitAll.PluginGuid, info.rulebookName));
			ability = bloodPondAbility.ability;

			return bloodPondAbility;
		}
	}
}
