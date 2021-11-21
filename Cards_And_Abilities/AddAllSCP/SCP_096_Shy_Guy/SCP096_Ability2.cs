using System.Collections;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace AddAllSCP.SCP_096_Shy_Guy
{
	public class SCP096_Ability2 : AbilityBehaviour
	{
		public override Ability Ability { get { return ability; } }

		public static Ability ability;

		public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard)
		{
			HarmonyInitAll.Log.LogDebug(
				$"Will respond to assigned slot? [{otherCard is not null && otherCard.Slot.opposingSlot == base.Card.Slot}]");
			return otherCard is not null && otherCard.Slot.opposingSlot == base.Card.Slot;
		}

		public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
		{
			var test = otherCard.Info.name;
			yield return new WaitForSeconds(0.25f);
			CardInfo cardByName = CardLoader.GetCardByName("scp_096_stage2");
			yield return base.Card.TransformIntoCard(cardByName);
			yield return new WaitForSeconds(0.5f);

			yield break;
		}

		public static NewAbility InitAbility()
		{
			// setup ability
			const string rulebookName = "Withdrawn";
			const string rulebookDescription =
				"A creature bearing this sigil does not want anyone around especially if they see it's face. Once triggered, it evolves.";

			AbilityInfo info = AbilityInfoUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription);

			// get and load artwork
			var defaultTex = CardUtils.getAndloadImageAsTexture("scp_096_ability_small.png");

			// set ability to behavior class
			NewAbility newAbility = new NewAbility(info, typeof(SCP096_Ability2), defaultTex,
				AbilityIdentifier.GetAbilityIdentifier(HarmonyInitAll.PluginGuid, info.rulebookName));
			ability = newAbility.ability;

			return newAbility;
		}
	}
}
