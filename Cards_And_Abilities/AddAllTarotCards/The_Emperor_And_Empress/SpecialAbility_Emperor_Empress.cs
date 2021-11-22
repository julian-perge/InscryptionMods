using System.Collections;
using System.Collections.Generic;
using System.Linq;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using static AddAllTarotCards.HarmonyInit;

namespace TheEmperorAndEmpress
{
	public class SpecialAbility_Emperor_Empress : SpecialCardBehaviour
	{
		public static NewSpecialAbility _SpecialAbility;

		private bool isBaseCardEmperor;
		private bool modsHaveBeenApplied;
		private CardSlot slotWithPartner;

		private void Start()
		{
			isBaseCardEmperor = base.Card.Info.name.Equals("ThEmperor");
		}

		private CardModificationInfo ReignOfPowerMods
		{
			get
			{
				return new()
				{
					abilities = new List<Ability>() { Ability.BuffNeighbours, Ability.PreventAttack },
					attackAdjustment = 1,
					healthAdjustment = 2
				};
			}
		}

		private bool IsCardEmpress(PlayableCard playableCard)
		{
			return playableCard.Info.name.Equals("TheEmpress");
		}

		private bool IsCardEmperor(PlayableCard playableCard)
		{
			return playableCard.Info.name.Equals("TheEmperor");
		}

		private bool IsBaseEmperorAndOtherEmpress(PlayableCard otherCard)
		{
			return isBaseCardEmperor && IsCardEmpress(otherCard);
		}

		private bool IsBaseEmpressAndOtherEmperor(PlayableCard otherCard)
		{
			return !isBaseCardEmperor && IsCardEmperor(otherCard);
		}

		private bool isBothCardsOnField(PlayableCard otherCard)
		{
			// one or the other but not both
			return IsBaseEmperorAndOtherEmpress(otherCard) ^ IsBaseEmpressAndOtherEmperor(otherCard);
		}

		public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
		{
			return true;
		}

		public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
		{
			if (slotWithPartner)
			{
				Log.LogDebug($"[OnDie] Removing [Reign of Power] mods from [{slotWithPartner.Card.name}]");
				slotWithPartner.Card.RemoveTemporaryMod(ReignOfPowerMods, true);
			}

			yield break;
		}

		public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat,
			PlayableCard killer)
		{
			// only respond to another card dying if mods have been applied because that means the duo is on the board
			return modsHaveBeenApplied && deathSlot == slotWithPartner;
		}

		public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat,
			PlayableCard killer)
		{
			Log.LogDebug($"[OnOtherCardDie] Removing [Reign of Power] mods from [{base.PlayableCard.name}]");
			base.PlayableCard.RemoveTemporaryMod(ReignOfPowerMods, true);
			yield break;
		}

		public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard)
		{
			// if the modifications have been applied already, we stop checking
			return !modsHaveBeenApplied && otherCard.Slot.IsPlayerSlot;
		}

		public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
		{
			var playerSlots = Singleton<BoardManager>.Instance
				.GetSlots(true)
				.Where(slot => slot && slot.Card);

			Log.LogDebug($"Found [{playerSlots.Count()}] slots that are valid");

			foreach (var playerSlot in playerSlots)
			{
				if (isBothCardsOnField(playerSlot.Card))
				{
					Log.LogDebug("-> Both cards exist on field, adding [Reign of Power] mods to both cards");
					slotWithPartner = playerSlot;
					base.PlayableCard.AddTemporaryMod(ReignOfPowerMods);
					playerSlot.Card.AddTemporaryMod(ReignOfPowerMods);
					modsHaveBeenApplied = true;
				}
			}

			yield break;
		}

		public static NewSpecialAbility InitAbility()
		{
			string name = "ReignOfPower";
			string desc =
				"When both the Empress and the Emperor are on the board, they become 3/4 cards with Unkillable and Leader";

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
