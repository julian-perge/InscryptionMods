using System.Collections;
using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace AddAllSCP.SCP_035_Porcelain_Mask
{
	public class MaskAbility : AbilityBehaviour
	{
		public static Ability ability;

		public override Ability Ability { get { return ability; } }

		private PlayableCard foeInOpposingSlot;

		public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard)
		{
			return !base.Card.Dead && !otherCard.Dead && otherCard.Slot == base.Card.Slot.opposingSlot;
		}

		public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
		{
			foeInOpposingSlot = otherCard;
			yield break;
		}

		public override bool RespondsToTurnEnd(bool playerTurnEnd)
		{
			HarmonyInitAll.Log.LogDebug($"Will respond to turn end? [{playerTurnEnd && foeInOpposingSlot is not null}]");
			return playerTurnEnd && foeInOpposingSlot is not null;
		}

		public override IEnumerator OnTurnEnd(bool playerTurnEnd)
		{
			HarmonyInitAll.Log.LogDebug($"Is player end of turn? [{playerTurnEnd}] Foe is [{foeInOpposingSlot.name}]");
			yield return foeInOpposingSlot.TakeDamage(1, base.Card);
			yield break;
		}

		protected internal static NewAbility InitAbility()
		{
			// setup ability
			var rulebookName = "Porcelain Mask";
			var description = "CANNOT BE SACRIFICED. Deals 1 damage per turn to the foe in front of it.";

			AbilityInfo info = AbilityInfoUtils.CreateAbilityInfo(rulebookName, description);

			// get and load artwork
			Texture2D tex = CardUtils.getAndloadImageAsTexture("BepInEx/plugins/CardLoader/Artwork/scp_035_sigil.png");

			// set ability to behavior class
			NewAbility maskAbility = new NewAbility(info, typeof(MaskAbility), tex,
				AbilityIdentifier.GetAbilityIdentifier(HarmonyInitAll.PluginGuid, info.rulebookName)
			);
			MaskAbility.ability = maskAbility.ability;

			return maskAbility;
		}
	}
}
