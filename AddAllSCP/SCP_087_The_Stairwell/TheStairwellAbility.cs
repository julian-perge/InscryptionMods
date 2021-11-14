using System.Collections;
using System.Collections.Generic;
using AddAllSCP.SCP_096_Shy_Guy;
using APIPlugin;
using CardLoaderPlugin.lib;
using DiskCardGame;
using UnityEngine;

namespace AddAllSCP.SCP_087_The_Stairwell
{
	public class TheStairwellAbility : AbilityBehaviour
	{

		public static Ability ability;
		
		public override Ability Ability { get { return ability;  } }
		
		private PlayableCard foeInOpposingSlot;
		private int turnsTaken = 1;
		
		private bool RespondsToOpposingCard(PlayableCard otherCard)
		{
			return !base.Card.Dead && !otherCard.Dead && otherCard.Slot == base.Card.Slot.opposingSlot;
		}

		public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
		{
			if (card.Slot == base.Card.Slot.opposingSlot)
			{
				foeInOpposingSlot = null;
			}

			yield break;
		}

		public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard)
		{
			return this.RespondsToOpposingCard(otherCard);
		}

		public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
		{
			foeInOpposingSlot = otherCard;
			yield break;
		}

		public override bool RespondsToTurnEnd(bool playerTurnEnd)
		{
			return playerTurnEnd;
		}
		
		public override IEnumerator OnTurnEnd(bool playerTurnEnd)
		{
			if (turnsTaken++ == 3 && foeInOpposingSlot is not null)
			{
				turnsTaken = 0;
				yield return foeInOpposingSlot.Die(false, base.Card);
			}
			yield break;
		}
		
		public static NewAbility InitAbility()
		{
			// setup ability
			AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
			info.powerLevel = 0;
			info.rulebookName = "The Stairwell";
			info.rulebookDescription =
				"Any card that dies, spawn a 1/1 \"Cured\" in an open slot on your side.";
			info.metaCategories = new List<AbilityMetaCategory>()
			{
				AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part1Rulebook
			};

			// get and load artwork
			Texture2D sigilTex =
				CardUtils.getAndloadImageAsTexture("BepInEx/plugins/CardLoader/Artwork/double_death_tweak.png");

			// set ability to behavior class
			NewAbility theSightAbility = new NewAbility(info, typeof(TheSightAbility), sigilTex);
			TheSightAbility.ability = theSightAbility.ability;

			return theSightAbility;
		}
		
	}
}
