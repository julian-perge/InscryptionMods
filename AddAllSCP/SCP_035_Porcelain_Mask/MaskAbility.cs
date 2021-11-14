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
			return !base.Card.Dead && !otherCard.Dead && otherCard.Slot == base.Card.Slot.opposingSlot;;
		}

		public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
		{
			foeInOpposingSlot = otherCard;
			yield break;
		}

		public override bool RespondsToTurnEnd(bool playerTurnEnd)
		{
			return playerTurnEnd && foeInOpposingSlot is not null;
		}

		public override IEnumerator OnTurnEnd(bool playerTurnEnd)
		{
			// no null check as this only occurs if RespondsToTurnEnd returns true
			yield return foeInOpposingSlot.TakeDamage(1, base.Card);
			yield break;
		}

		public static NewAbility InitAbility()
		{
			// setup ability
			AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
			info.powerLevel = 0;
			info.rulebookName = "Porcelain Mask";
			info.rulebookDescription =
				"Once you put it on, you can't get it off. Deals 1 damage per turn to the foe in front of it.";
			info.metaCategories = new List<AbilityMetaCategory>()
			{
				AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part1Rulebook
			};

			// get and load artwork
			var imgBytes = System.IO.File.ReadAllBytes("BepInEx/plugins/CardLoader/Artwork/scp_035_sigil.png");
			Texture2D tex = new Texture2D(2, 2);
			tex.LoadImage(imgBytes);

			// set ability to behavior class
			NewAbility theSightAbility = new NewAbility(info, typeof(MaskAbility), tex);
			MaskAbility.ability = theSightAbility.ability;

			return theSightAbility;
		}
	}
}
