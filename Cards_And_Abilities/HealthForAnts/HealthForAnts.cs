using static HealthForAnts.HarmonyInit;
using Enumerable = System.Linq.Enumerable;

namespace HealthForAnts
{
	public class HealthForAnts : DiskCardGame.VariableStatBehaviour
	{
		private static DiskCardGame.SpecialStatIcon specialStatIcon;

		public override DiskCardGame.SpecialStatIcon IconType => specialStatIcon;

		public override int[] GetStatValues()
		{
			System.Collections.Generic.List<DiskCardGame.CardSlot> list = base.PlayableCard.Slot.IsPlayerSlot
				? Singleton<DiskCardGame.BoardManager>.Instance.PlayerSlotsCopy
				: Singleton<DiskCardGame.BoardManager>.Instance.OpponentSlotsCopy;

			int numToAddToHealth = Enumerable.Count(Enumerable.Where(list, slot => slot.Card is not null),
				cardSlot => cardSlot.Card.Info.HasTrait(DiskCardGame.Trait.Ant));
			// Log.LogDebug($"[DomeAnt] GetStatValues called with DomeAnt. Adding [{num}] Health.");

			int[] array = new int[2];
			array[1] = numToAddToHealth;
			return array;
		}

		public static APIPlugin.NewSpecialAbility InitStatIconAndAbility()
		{
			DiskCardGame.StatIconInfo info = UnityEngine.ScriptableObject.CreateInstance<DiskCardGame.StatIconInfo>();
			info.appliesToAttack = false;
			info.appliesToHealth = true;
			info.rulebookName = "Ants (Health)";
			info.rulebookDescription =
				"The value represented with this sigil will be equal to the number of Ants that the owner has on their side of the table.";
			info.iconGraphic = DiskCardGame.StatIconInfo.GetIconInfo(DiskCardGame.SpecialStatIcon.Ants).iconGraphic;
			var sId = APIPlugin.SpecialAbilityIdentifier.GetID(PluginGuid, info.rulebookName);

			var healthForAntsAbility = new APIPlugin.NewSpecialAbility(typeof(HealthForAnts), sId, info);
			specialStatIcon = healthForAntsAbility.statIconInfo.iconType;

			return healthForAntsAbility;
		}
	}
}
