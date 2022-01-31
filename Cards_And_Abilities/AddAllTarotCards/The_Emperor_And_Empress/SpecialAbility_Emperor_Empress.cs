using static AddAllTarotCards.HarmonyInit;

namespace AddAllTarotCards.The_Emperor_And_Empress
{
	public class SpecialAbility_Emperor_Empress : DiskCardGame.SpecialCardBehaviour
	{
		public static APIPlugin.NewSpecialAbility _SpecialAbility;

		private bool modsHaveBeenApplied;
		private DiskCardGame.CardSlot slotWithEmperor;

		private DiskCardGame.CardModificationInfo ReignOfPowerMods
		{
			get
			{
				return new()
				{
					abilities = new System.Collections.Generic.List<DiskCardGame.Ability>()
					{
						DiskCardGame.Ability.BuffNeighbours, DiskCardGame.Ability.DebuffEnemy
					},
					attackAdjustment = 1,
					healthAdjustment = 2
				};
			}
		}

		private bool IsCardEmperor(DiskCardGame.PlayableCard playableCard)
		{
			return playableCard.Info.name.Equals("TheEmperor");
		}

		private DiskCardGame.CardSlot GetEmperorCardSlotIfExists()
		{
			return DiskCardGame.BoardManager
				.Instance
				.GetSlots(true)
				.Find(slot => slot && slot.Card && slot != base.PlayableCard.Slot && IsCardEmperor(slot.Card));
		}

		public override bool RespondsToDie(bool wasSacrifice, DiskCardGame.PlayableCard killer)
		{
			return true;
		}

		public override System.Collections.IEnumerator OnDie(bool wasSacrifice, DiskCardGame.PlayableCard killer)
		{
			if (slotWithEmperor)
			{
				Log.LogDebug($"[OnDie] Removing [Reign of Power] mods from [{slotWithEmperor.Card.name}]");
				slotWithEmperor.Card.RemoveTemporaryMod(ReignOfPowerMods, true);
			}

			yield break;
		}

		public override bool RespondsToOtherCardDie(DiskCardGame.PlayableCard card, DiskCardGame.CardSlot deathSlot,
			bool fromCombat,
			DiskCardGame.PlayableCard killer)
		{
			// only respond to another card dying if mods have been applied because that means the duo is on the board
			return modsHaveBeenApplied && deathSlot == slotWithEmperor;
		}

		public override System.Collections.IEnumerator OnOtherCardDie(DiskCardGame.PlayableCard card,
			DiskCardGame.CardSlot deathSlot, bool fromCombat,
			DiskCardGame.PlayableCard killer)
		{
			Log.LogDebug($"[OnOtherCardDie] Removing [Reign of Power] mods from [{base.PlayableCard.name}]");
			base.PlayableCard.RemoveTemporaryMod(ReignOfPowerMods, true);
			yield break;
		}

		public override bool RespondsToPlayFromHand()
		{
			return true;
		}

		public override System.Collections.IEnumerator OnPlayFromHand()
		{
			var _slotWithEmperor = GetEmperorCardSlotIfExists();
			if (_slotWithEmperor)
			{
				Log.LogDebug("-> [OnPlayFromHand] Both cards exist on field, adding [Reign of Power] mods to both cards");
				MakeReignOfPowerActive(_slotWithEmperor.Card);
			}

			yield break;
		}

		public override bool RespondsToOtherCardAssignedToSlot(DiskCardGame.PlayableCard otherCard)
		{
			// if the modifications have been applied already, we stop checking
			return !modsHaveBeenApplied && otherCard.Slot.IsPlayerSlot;
		}

		public override System.Collections.IEnumerator OnOtherCardAssignedToSlot(DiskCardGame.PlayableCard otherCard)
		{
			if (IsCardEmperor(otherCard))
			{
				Log.LogDebug(
					"-> [OnOtherCardAssignedToSlot] Both cards exist on field, adding [Reign of Power] mods to both cards");
				MakeReignOfPowerActive(otherCard);
			}

			yield break;
		}

		private void MakeReignOfPowerActive(DiskCardGame.PlayableCard otherCard)
		{
			slotWithEmperor = otherCard.Slot;
			base.PlayableCard.AddTemporaryMod(ReignOfPowerMods);
			otherCard.AddTemporaryMod(ReignOfPowerMods);
			modsHaveBeenApplied = true;
		}

		public static APIPlugin.NewSpecialAbility InitAbility()
		{
			const string name = "ReignOfPower";
			const string desc =
				"When both the Empress and the Emperor exist on the board, they become 3/4 cards with Unkillable and Leader";

			// setup ability
			DiskCardGame.StatIconInfo info = UnityEngine.ScriptableObject.CreateInstance<DiskCardGame.StatIconInfo>();
			info.appliesToAttack = false; // icon will replace both attack and health numbers until played
			info.appliesToHealth = false;
			info.rulebookName = name;
			info.rulebookDescription = desc;

			var sId = APIPlugin.SpecialAbilityIdentifier.GetID(PluginGuid, info.rulebookName);

			// set ability to behavior class
			var newAbility = new APIPlugin.NewSpecialAbility(typeof(SpecialAbility_Emperor_Empress), sId, info);
			_SpecialAbility = newAbility; // this is so we can use it in the HarmonyInit class

			return newAbility;
		}
	}
}
