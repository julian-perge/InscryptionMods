namespace AddAllSCP.SCP_096_Shy_Guy
{
	public class SCP096_Ability2 : DiskCardGame.AbilityBehaviour
	{
		public override DiskCardGame.Ability Ability { get { return ability; } }

		public static DiskCardGame.Ability ability;

		public override bool RespondsToOtherCardAssignedToSlot(DiskCardGame.PlayableCard otherCard)
		{
			HarmonyInitAll.Log.LogDebug(
				$"Will respond to assigned slot? [{otherCard is not null && otherCard.Slot.opposingSlot == base.Card.Slot}]");
			return otherCard is not null && otherCard.Slot.opposingSlot == base.Card.Slot;
		}

		public override System.Collections.IEnumerator OnOtherCardAssignedToSlot(DiskCardGame.PlayableCard otherCard)
		{
			yield return new UnityEngine.WaitForSeconds(0.25f);
			DiskCardGame.CardInfo cardByName = DiskCardGame.CardLoader.GetCardByName("scp_096_stage2");
			yield return base.Card.TransformIntoCard(cardByName);
			yield return new UnityEngine.WaitForSeconds(0.5f);

			yield break;
		}

		public static APIPlugin.NewAbility InitAbility()
		{
			// setup ability
			const string rulebookName = "Withdrawn";
			const string rulebookDescription =
				"A creature bearing this sigil does not want anyone around especially if they see it's face. Once triggered, it evolves.";

			DiskCardGame.AbilityInfo info =
				APIPlugin.AbilityInfoUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription);

			// get and load artwork
			var defaultTex = APIPlugin.CardUtils.getAndloadImageAsTexture("scp_096_ability_small.png");

			// set ability to behavior class
			APIPlugin.NewAbility newAbility = new APIPlugin.NewAbility(info, typeof(SCP096_Ability2), defaultTex,
				APIPlugin.AbilityIdentifier.GetAbilityIdentifier(HarmonyInitAll.PluginGuid, info.rulebookName));
			ability = newAbility.ability;

			return newAbility;
		}
	}
}
