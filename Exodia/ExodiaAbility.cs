using System.Collections;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace Exodia
{
	public class ExodiaAbility : AbilityBehaviour
	{
		private bool rightArmInCorrectSlot;
		private bool leftArmInCorrectSlot;

		public override Ability Ability
		{
			get { return ability; }
		}

		public static Ability ability;

		public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard)
		{
			return !otherCard.Dead && otherCard.Slot.IsPlayerSlot;
		}

		public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
		{
			yield return base.PreSuccessfulTriggerSequence();

			if (this.DoAdjacentSlotsHaveArms(otherCard))
			{
				Singleton<TextDisplayer>.Instance.StartCoroutine(
					Singleton<TextDisplayer>.Instance.ShowThenClear("EXODIA, OBLITERATE!", -0.65f, 0.4f,
						Emotion.Laughter, TextDisplayer.LetterAnimation.WavyJitter, DialogueEvent.Speaker.Bonelord)
				);
				yield return new WaitForSeconds(1f);

				int dmgToWin = Singleton<LifeManager>.Instance.DamageUntilPlayerWin;
				// show damage on scale, taken from Pliers and modified the dmg done
				yield return Singleton<LifeManager>.Instance.ShowDamageSequence(
					dmgToWin,
					1, false, 0.25f,
					ResourceBank.Get<GameObject>("Prefabs/Environment/ScaleWeights/Weight_RealTooth"),
					0.25f);
			}

			yield return base.LearnAbility(0.5f);
			yield break;
		}

		private bool DoAdjacentSlotsHaveArms(PlayableCard otherCard)
		{
			CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
			CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);
			if (otherCard.name.Contains("Right Arm") && otherCard.Slot == toRight)
			{
				rightArmInCorrectSlot = true;
			}
			else if (otherCard.name.Contains("Left Arm") && otherCard.Slot == toLeft)
			{
				leftArmInCorrectSlot = true;
			}

			return leftArmInCorrectSlot && rightArmInCorrectSlot;
		}

		protected internal static NewAbility InitAbility()
		{
			// setup ability
			string name = "The Forbidden One";
			string desc = "You automatically win the round.";
			AbilityInfo info = AbilityInfoUtils.CreateInfoWithDefaultSettings(name, desc);
			var abIds = AbilityIdentifier.GetAbilityIdentifier(HarmonyInit.PluginGuid, info.rulebookName);

			// get Exodia art
			Texture2D tex = CardUtils.getAndloadImageAsTexture("BepInEx/plugins/CardLoader/Artwork/exodia_sigil_small.png");

			// set ability to behavior class
			NewAbility exodiaAbility = new NewAbility(info, typeof(ExodiaAbility), tex, abIds);
			ability = exodiaAbility.ability;

			return exodiaAbility;
		}
	}
}
