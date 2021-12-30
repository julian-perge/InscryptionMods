using System.Collections.Generic;
using System.Linq;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using static HealthForAnts.HarmonyInit;

namespace HealthForAnts
{
	public class HealthForAnts : VariableStatBehaviour
	{
		private static SpecialStatIcon specialStatIcon;

		protected override SpecialStatIcon IconType => specialStatIcon;

		protected override int[] GetStatValues()
		{
			List<CardSlot> list = base.PlayableCard.Slot.IsPlayerSlot
				? Singleton<BoardManager>.Instance.PlayerSlotsCopy
				: Singleton<BoardManager>.Instance.OpponentSlotsCopy;

			int numToAddToHealth = list
				.Where(slot => slot.Card is not null)
				.Count(cardSlot => cardSlot.Card.Info.HasTrait(Trait.Ant));
			// Log.LogDebug($"[DomeAnt] GetStatValues called with DomeAnt. Adding [{num}] Health.");

			int[] array = new int[2];
			array[1] = numToAddToHealth;
			return array;
		}

		public static NewSpecialAbility InitStatIconAndAbility()
		{
			StatIconInfo info = ScriptableObject.CreateInstance<StatIconInfo>();
			info.appliesToAttack = false;
			info.appliesToHealth = true;
			info.rulebookName = "Ants (Health)";
			info.rulebookDescription =
				"The value represented with this sigil will be equal to the number of Ants that the owner has on their side of the table.";
			info.iconGraphic = StatIconInfo.GetIconInfo(SpecialStatIcon.Ants).iconGraphic;
			var sId = SpecialAbilityIdentifier.GetID(PluginGuid, info.rulebookName);

			var healthForAntsAbility = new NewSpecialAbility(typeof(HealthForAnts), sId, info);
			specialStatIcon = healthForAntsAbility.statIconInfo.iconType;

			return healthForAntsAbility;
		}
	}
}
