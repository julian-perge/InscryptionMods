using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;
using static HealthForAnts.HealthForAntsPlugin;

namespace HealthForAnts;

public class HealthForAnts : VariableStatBehaviour
{
	public static SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility FullSpecial;
	public static StatIconManager.FullStatIcon FullStatIcon;

	public override SpecialStatIcon IconType => FullStatIcon.Id;

	public override int[] GetStatValues()
	{
		int numToAddToHealth = BoardManager.Instance.GetSlots(!base.PlayableCard.OpponentCard)
			.Where(slot => slot.Card)
			.Count(cardSlot => cardSlot.Card.Info.HasTrait(Trait.Ant));

		int[] array = new int[2];
		array[1] = numToAddToHealth;
		return array;
	}

	public static StatIconManager.FullStatIcon InitStatIconAndAbility()
	{
		StatIconInfo info = ScriptableObject.CreateInstance<StatIconInfo>();
		info.appliesToAttack = false;
		info.appliesToHealth = true;
		info.rulebookName = "Ants (Health)";
		info.rulebookDescription =
			"The value represented with this sigil will be equal to the number of Ants that the owner has on their side of the table.";
		info.iconGraphic = StatIconInfo.GetIconInfo(SpecialStatIcon.Ants).iconGraphic;

		FullStatIcon = StatIconManager.Add(PluginGuid, info, typeof(HealthForAnts));
		FullSpecial = SpecialTriggeredAbilityManager.Add(PluginGuid, info.rulebookName, typeof(HealthForAnts));

		return FullStatIcon;
	}
}
