namespace AddAllSCP.SCP_035_Porcelain_Mask
{
	public class MaskAbility : DiskCardGame.AbilityBehaviour
	{
		public static DiskCardGame.Ability ability;

		public override DiskCardGame.Ability Ability { get { return ability; } }

		private DiskCardGame.PlayableCard foeInOpposingSlot;

		public override bool RespondsToOtherCardAssignedToSlot(DiskCardGame.PlayableCard otherCard)
		{
			return !base.Card.Dead && !otherCard.Dead && otherCard.Slot == base.Card.Slot.opposingSlot;
		}

		public override System.Collections.IEnumerator OnOtherCardAssignedToSlot(DiskCardGame.PlayableCard otherCard)
		{
			foeInOpposingSlot = otherCard;
			yield break;
		}

		public override bool RespondsToTurnEnd(bool playerTurnEnd)
		{
			HarmonyInitAll.Log.LogDebug($"Will respond to turn end? [{playerTurnEnd && foeInOpposingSlot is not null}]");
			return playerTurnEnd && foeInOpposingSlot is not null;
		}

		public override System.Collections.IEnumerator OnTurnEnd(bool playerTurnEnd)
		{
			HarmonyInitAll.Log.LogDebug($"Is player end of turn? [{playerTurnEnd}] Foe is [{foeInOpposingSlot.name}]");
			yield return foeInOpposingSlot.TakeDamage(1, base.Card);
			yield break;
		}

		protected internal static APIPlugin.NewAbility InitAbility()
		{
			// setup ability
			var rulebookName = "Porcelain Mask";
			var description = "CANNOT BE SACRIFICED. Deals 1 damage per turn to the foe in front of it.";

			DiskCardGame.AbilityInfo info =
				APIPlugin.AbilityInfoUtils.CreateInfoWithDefaultSettings(rulebookName, description);

			// get and load artwork
			UnityEngine.Texture2D tex = APIPlugin.CardUtils.getAndloadImageAsTexture("scp_035_sigil.png");

			// set ability to behavior class
			APIPlugin.NewAbility maskAbility = new APIPlugin.NewAbility(info, typeof(MaskAbility), tex,
				APIPlugin.AbilityIdentifier.GetAbilityIdentifier(HarmonyInitAll.PluginGuid, info.rulebookName)
			);
			MaskAbility.ability = maskAbility.ability;

			return maskAbility;
		}
	}
}
