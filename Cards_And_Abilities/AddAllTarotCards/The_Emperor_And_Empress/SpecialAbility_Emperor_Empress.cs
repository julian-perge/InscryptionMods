using System.Collections;
using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using static AddAllTarotCards.HarmonyInit;

namespace TheEmperorAndEmpress
{
	public class SpecialAbility_Emperor_Empress : SpecialCardBehaviour
	{
		public static NewSpecialAbility _SpecialAbility;

		private bool modsHaveBeenApplied;
		private CardSlot slotWithEmperor;

		private CardModificationInfo ReignOfPowerMods
		{
			get
			{
				return new()
				{
					abilities = new List<Ability>() { Ability.BuffNeighbours, Ability.DebuffEnemy },
					attackAdjustment = 1,
					healthAdjustment = 2
				};
			}
		}

		private bool IsCardEmperor(PlayableCard playableCard)
		{
			return playableCard.Info.name.Equals("TheEmperor");
		}

		private CardSlot GetEmperorCardSlotIfExists()
		{
			return Singleton<BoardManager>
				.Instance
				.GetSlots(true)
				.Find(slot => slot && slot.Card && slot != base.PlayableCard.Slot && IsCardEmperor(slot.Card));
		}

		public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
		{
			return true;
		}

		public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
		{
			if (slotWithEmperor)
			{
				Log.LogDebug($"[OnDie] Removing [Reign of Power] mods from [{slotWithEmperor.Card.name}]");
				slotWithEmperor.Card.RemoveTemporaryMod(ReignOfPowerMods, true);
			}

			yield break;
		}

		public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat,
			PlayableCard killer)
		{
			// only respond to another card dying if mods have been applied because that means the duo is on the board
			return modsHaveBeenApplied && deathSlot == slotWithEmperor;
		}

		public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat,
			PlayableCard killer)
		{
			Log.LogDebug($"[OnOtherCardDie] Removing [Reign of Power] mods from [{base.PlayableCard.name}]");
			base.PlayableCard.RemoveTemporaryMod(ReignOfPowerMods, true);
			yield break;
		}

		public override bool RespondsToPlayFromHand()
		{
			return true;
		}

		public override IEnumerator OnPlayFromHand()
		{
			var _slotWithEmperor = GetEmperorCardSlotIfExists();
			if (_slotWithEmperor)
			{
				Log.LogDebug("-> [OnPlayFromHand] Both cards exist on field, adding [Reign of Power] mods to both cards");
				MakeReignOfPowerActive(_slotWithEmperor.Card);
			}

			yield break;
		}

		public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard)
		{
			// if the modifications have been applied already, we stop checking
			return !modsHaveBeenApplied && otherCard.Slot.IsPlayerSlot;
		}

		public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
		{
			if (IsCardEmperor(otherCard))
			{
				Log.LogDebug(
					"-> [OnOtherCardAssignedToSlot] Both cards exist on field, adding [Reign of Power] mods to both cards");
				MakeReignOfPowerActive(otherCard);
			}

			yield break;
		}

		private void MakeReignOfPowerActive(PlayableCard otherCard)
		{
			slotWithEmperor = otherCard.Slot;
			base.PlayableCard.AddTemporaryMod(ReignOfPowerMods);
			otherCard.AddTemporaryMod(ReignOfPowerMods);
			modsHaveBeenApplied = true;
		}

		public static NewSpecialAbility InitAbility()
		{
			string name = "ReignOfPower";
			string desc =
				"When both the Empress and the Emperor exist on the board, they become 3/4 cards with Unkillable and Leader";

			// setup ability
			StatIconInfo info = ScriptableObject.CreateInstance<StatIconInfo>();
			info.appliesToAttack = false; // icon will replace both attack and health numbers until played
			info.appliesToHealth = false;
			info.rulebookName = name;
			info.rulebookDescription = desc;

			var sId = SpecialAbilityIdentifier.GetID(PluginGuid, info.rulebookName);

			// set ability to behavior class
			var newAbility = new NewSpecialAbility(typeof(SpecialAbility_Emperor_Empress), sId, info);
			_SpecialAbility = newAbility; // this is so we can use it in the HarmonyInit class

			return newAbility;
		}
	}
}
