using System.Collections;
using System.Collections.Generic;
using System.Linq;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace AddAllSCP.SCP_049_Plague_Doctor
{
	public class TheCureAbility : AbilityBehaviour
	{
		public override Ability Ability { get { return ability; } }

		public static Ability ability;

		public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat,
			PlayableCard killer)
		{
			// can't rez non-living things. Moon causes bugs.
			if (card.Info.traits.Exists(t => t is Trait.Terrain or Trait.Pelt or Trait.Giant))
			{
				return false;
			}

			// base.Card.OpponentCard is called in BoardManager.AssignCardToSlot
			bool isBaseCardValid = base.Card.OnBoard && deathSlot.Card != base.Card;
			bool isValidCardDeath = deathSlot.Card is not null && deathSlot.Card == card;
			return (fromCombat && isBaseCardValid && isValidCardDeath) || IsPlagueDoctorDeath(card);
		}

		private bool IsPlagueDoctorDeath(PlayableCard card)
		{
			return card is not null && card.name.Contains("SCP_049_PlagueDoctor");
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
			HarmonyInitAll.Log.LogDebug(
				$"[TheCure] Card is null {card is null} deathslot card is null {deathSlot is null} Killer {killer.name}");
			if (IsPlagueDoctorDeath(card))
			{
				HarmonyInitAll.Log.LogDebug("-> Is Plague Doctor death card is true");
				// loop through player slots and only check for cards with "Cured" in the name
				foreach (var slot in slots.Where(slot => slot is not null && IsPlagueDoctorCuredCard(slot.Card)))
				{
					yield return slot.Card.Die(false, null, true);
				}
			}
			else
			{
				string nameOfCard = CardUtils.cleanCardName(card.name);
				HarmonyInitAll.Log.LogDebug($"-> Is Plague Doctor death card was false, checking card name is {nameOfCard}");
				// if not Plague Doctor death, set to 1/1 and spawn on an open slot on your side of the field
				var filteredSlots = slots.Where(slot => slot is not null && slot.Card is null);
				HarmonyInitAll.Log.LogDebug($"-> Number of filtered slots [{filteredSlots.Count()}]");
				foreach (var slot in slots.Where(slot => slot is not null && slot.Card is null))
				{
					CardInfo cardByName = CardLoader.GetCardByName(nameOfCard);
					CardModificationInfo cardModificationInfo = new CardModificationInfo();
					cardModificationInfo.attackAdjustment = -cardByName.Attack + 1;
					cardModificationInfo.healthAdjustment = -cardByName.Health + 1;
					cardModificationInfo.bloodCostAdjustment = -cardByName.BloodCost;
					cardModificationInfo.bonesCostAdjustment = -cardByName.BonesCost;
					cardModificationInfo.energyCostAdjustment = -cardByName.EnergyCost;
					cardModificationInfo.nullifyGemsCost = true;
					cardModificationInfo.nameReplacement = nameOfCard + " \"Cured\"";
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
			var rulebookName = "The Cure";
			var description =
				"Any card that dies from combat, spawn a 1/1 \"Cured\" version of the card in an open slot on your side.";
			AbilityInfo info = AbilityInfoUtils.CreateInfoWithDefaultSettings(rulebookName, description);

			// get and load artwork
			Texture2D sigilTex =
				CardUtils.getAndloadImageAsTexture("BepInEx/plugins/CardLoader/Artwork/double_death_tweak.png");

			// set ability to behavior class
			NewAbility theCureAbility = new NewAbility(info, typeof(TheCureAbility), sigilTex,
				AbilityIdentifier.GetAbilityIdentifier(HarmonyInitAll.PluginGuid, info.rulebookName));
			ability = theCureAbility.ability;

			return theCureAbility;
		}
	}
}
