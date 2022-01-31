using System.Linq;

namespace AddAllSCP.SCP_049_Plague_Doctor
{
	public class TheCureAbility : DiskCardGame.AbilityBehaviour
	{
		public override DiskCardGame.Ability Ability { get { return ability; } }

		public static DiskCardGame.Ability ability;

		public override bool RespondsToOtherCardDie(DiskCardGame.PlayableCard card, DiskCardGame.CardSlot deathSlot,
			bool fromCombat,
			DiskCardGame.PlayableCard killer)
		{
			// can't rez non-living things. Moon causes bugs.
			if (card.Info.traits.Exists(trait =>
				trait is DiskCardGame.Trait.Terrain or DiskCardGame.Trait.Pelt or DiskCardGame.Trait.Giant))
			{
				return false;
			}

			// base.Card.OpponentCard is called in BoardManager.AssignCardToSlot
			bool isBaseCardValid = base.Card.OnBoard && deathSlot.Card != base.Card;
			bool isValidCardDeath = deathSlot.Card is not null && deathSlot.Card == card;
			return (fromCombat && isBaseCardValid && isValidCardDeath) || IsPlagueDoctorDeath(card);
		}

		private bool IsPlagueDoctorDeath(DiskCardGame.PlayableCard card)
		{
			return card is not null && card.name.Contains("SCP_049_PlagueDoctor");
		}

		private bool IsPlagueDoctorCuredCard(DiskCardGame.PlayableCard card)
		{
			return card.name.Contains("\"Cured\"");
		}

		public override System.Collections.IEnumerator OnOtherCardDie(DiskCardGame.PlayableCard card,
			DiskCardGame.CardSlot deathSlot,
			bool fromCombat,
			DiskCardGame.PlayableCard killer)
		{
			yield return base.PreSuccessfulTriggerSequence();
			System.Collections.Generic.List<DiskCardGame.CardSlot> slots =
				DiskCardGame.BoardManager.Instance.GetSlots(true);
			HarmonyInitAll.Log.LogDebug(
				$"[TheCure] Card is null {card is null} deathslot card is null {deathSlot is null} Killer {killer.name}");
			if (IsPlagueDoctorDeath(card))
			{
				HarmonyInitAll.Log.LogDebug("-> Is Plague Doctor death card is true");
				// loop through player slots and only check for cards with "Cured" in the name
				foreach (var slot in Enumerable.Where(slots, slot => slot is not null && IsPlagueDoctorCuredCard(slot.Card)))
				{
					yield return slot.Card.Die(false, null, true);
				}
			}
			else
			{
				string nameOfCard = card.Info.name;
				HarmonyInitAll.Log.LogDebug($"-> Is Plague Doctor death card was false, checking card name is {nameOfCard}");
				// if not Plague Doctor death, set to 1/1 and spawn on an open slot on your side of the field
				var filteredSlots = Enumerable.Where(slots, slot => slot is not null && slot.Card is null);
				HarmonyInitAll.Log.LogDebug($"-> Number of filtered slots [{Enumerable.Count(filteredSlots)}]");
				foreach (var slot in Enumerable.Where(slots, slot => slot is not null && slot.Card is null))
				{
					DiskCardGame.CardInfo cardByName = DiskCardGame.CardLoader.GetCardByName(nameOfCard);
					DiskCardGame.CardModificationInfo cardModificationInfo = new DiskCardGame.CardModificationInfo
					{
						attackAdjustment = -cardByName.Attack + 1,
						healthAdjustment = -cardByName.Health + 1,
						bloodCostAdjustment = -cardByName.BloodCost,
						bonesCostAdjustment = -cardByName.BonesCost,
						energyCostAdjustment = -cardByName.EnergyCost,
						nullifyGemsCost = true,
						nameReplacement = nameOfCard + " \"Cured\""
					};
					cardByName.Mods.Add(cardModificationInfo);
					yield return DiskCardGame.BoardManager.Instance.CreateCardInSlot(cardByName, slot, 0.1f, true);
					break;
				}
			}

			yield return new UnityEngine.WaitForSeconds(0.1f);
			yield return base.LearnAbility(0.5f);
			yield break;
		}

		public static APIPlugin.NewAbility InitAbility()
		{
			// setup ability
			const string rulebookName = "The Cure";
			const string description =
				"Any card that dies from combat, spawn a 1/1 \"Cured\" version of the card in an open slot on your side.";
			DiskCardGame.AbilityInfo info =
				APIPlugin.AbilityInfoUtils.CreateInfoWithDefaultSettings(rulebookName, description);

			// get and load artwork
			UnityEngine.Texture2D sigilTex =
				APIPlugin.CardUtils.LoadImageAndGetTexture("double_death_tweak.png");

			// set ability to behavior class
			APIPlugin.NewAbility theCureAbility = new APIPlugin.NewAbility(info, typeof(TheCureAbility), sigilTex,
				APIPlugin.AbilityIdentifier.GetID(HarmonyInitAll.PluginGuid, info.rulebookName));
			ability = theCureAbility.ability;

			return theCureAbility;
		}
	}
}
