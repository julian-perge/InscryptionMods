using System.Collections;
using System.Collections.Generic;
using AddAllSCP.SCP_096_Shy_Guy;
using APIPlugin;
using CardLoaderPlugin.lib;
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
			return playerTurnEnd; // if it's our turn end
		}

		public override IEnumerator OnTurnEnd(bool playerTurnEnd)
		{
			yield return base.PreSuccessfulTriggerSequence();
			List<CardSlot> slots = Singleton<BoardManager>.Instance.GetSlots(true)
				.FindAll(slot => slot is not null & slot.Card is null);
			// if no available slots, list will be empty and won't loop
			foreach (var slot in slots)
			{
				var bloodCardToSpawn 
					= turnsTaken++ < 3 
					? CardLoader.GetCardByName("SCP_354_BloodCreature") 
					: CardLoader.GetCardByName("SCP_354_BloodEntity");

				yield return Singleton<BoardManager>.Instance.CreateCardInSlot(bloodCardToSpawn, slot, 0.1f, true);
				break; // only spawn one, then break out of loop and end resolve.
			}

			yield return new WaitForSeconds(0.1f);
			yield return base.LearnAbility(0.5f);
			yield break;
		}
		
		public static NewAbility InitAbility()
		{
			var description =
				"Spawn a Blood Creature (2/2 w/ Brittle) at the end of your turn in a random slot. After 3 turns, spawn a Blood Entity (3/3 w/ Brittle).";
			
			// setup ability
			AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
			info.powerLevel = 0;
			info.rulebookName = "Blood Pond";
			info.rulebookDescription = description;
			info.metaCategories = new List<AbilityMetaCategory>()
			{
				AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part1Rulebook
			};

			// get and load artwork
			Texture2D sigilTex =
				CardUtils.getAndloadImageAsTexture("BepInEx/plugins/CardLoader/Artwork/double_death_tweak.png");

			// set ability to behavior class
			NewAbility theSightAbility = new NewAbility(info, typeof(TheSightAbility), sigilTex);
			TheSightAbility.ability = theSightAbility.ability;

			return theSightAbility;
		}
		
	}
}
