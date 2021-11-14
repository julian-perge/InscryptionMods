using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AddAllSCP.SCP_096_Shy_Guy;
using APIPlugin;
using CardLoaderPlugin.lib;
using DiskCardGame;
using UnityEngine;

namespace AddAllSCP.SCP_049_Plague_Doctor
{
	public class DoubleDeathTweaked : AbilityBehaviour
	{
		public override Ability Ability { get { return Ability.DoubleDeath; } }

		public static Ability ability;

		public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat,
			PlayableCard killer)
		{
			// base.Card.OpponentCard is called in BoardManager.AssignCardToSlot
			bool isBaseCardValid = base.Card.OnBoard && deathSlot.Card != base.Card;
			bool isValidCardDeath = deathSlot.Card is not null && deathSlot.Card == card;
			return (isBaseCardValid && isValidCardDeath) || IsPlagueDoctorDeath(card);
		}

		private bool IsPlagueDoctorDeath(PlayableCard card)
		{
			return card is not null && card.name.Equals("SCP_049_PlagueDoctor");
		}

		private bool IsPlagueDoctorCuredCard(PlayableCard card)
		{
			return card.name.Contains("\"Cured\"");
		}

		public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat,
			PlayableCard killer)
		{
			yield return base.PreSuccessfulTriggerSequence();
			List<CardSlot> slots = Singleton<BoardManager>.Instance.GetSlots(true);
			if (IsPlagueDoctorDeath(card))
			{
				// loop through player slots and only check for cards with "Cured" in the name
				foreach (var slot in slots.Where(slot => slot is not null && IsPlagueDoctorCuredCard(slot.Card)))
				{
					yield return slot.Card.Die(false, null, true);
				}
			}
			else
			{
				// if not Plague Doctor death, set to 1/1 and spawn on an open slot on your side of the field
				foreach (var slot in slots.Where(slot => slot is not null && slot.Card is null))
				{
					CardInfo cardByName = CardLoader.GetCardByName(deathSlot.Card.name);
					CardModificationInfo cardModificationInfo = new CardModificationInfo();
					cardModificationInfo.attackAdjustment = -cardByName.Attack + 1;
					cardModificationInfo.healthAdjustment = -cardByName.Health + 1;
					cardModificationInfo.bloodCostAdjustment = -cardByName.BloodCost;
					cardModificationInfo.bonesCostAdjustment = -cardByName.BonesCost;
					cardModificationInfo.energyCostAdjustment = -cardByName.EnergyCost;
					cardModificationInfo.nullifyGemsCost = true;
					cardModificationInfo.nameReplacement = deathSlot.Card.name + " \"Cured\"";
					cardByName.Mods.Add(cardModificationInfo);
					yield return Singleton<BoardManager>.Instance.CreateCardInSlot(cardByName, slot, 0.1f, true);
					break;
				}
			}

			yield return new WaitForSeconds(0.1f);
			yield return base.LearnAbility(0.5f);
			yield break;
		}
		
		public static NewAbility InitAbility()
		{
			// setup ability
			AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
			info.powerLevel = 0;
			info.rulebookName = "The Cure";
			info.rulebookDescription =
				"Any card that dies, spawn a 1/1 \"Cured\" in an open slot on your side.";
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
