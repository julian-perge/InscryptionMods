using System.Collections;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using static AddAllTarotCards.HarmonyInit;

namespace AddAllTarotCards.The_World
{
	public class Ability_TheWorld : AbilityBehaviour
	{
		public static Ability ability;
		public override Ability Ability { get { return ability; } }

		public override bool RespondsToResolveOnBoard()
		{
			return true;
		}

		public override IEnumerator OnResolveOnBoard()
		{
			yield return new WaitForSeconds(0.25f);
			Singleton<TurnManager>.Instance.Opponent.SkipNextTurn = true;
			Singleton<TextDisplayer>.Instance.StartCoroutine(Singleton<TextDisplayer>.Instance
				.ShowUntilInput("I must yield to The World...", -0.65f, 0.4f));
			yield break;
		}

		public static NewAbility InitAbility()
		{
			// setup ability
			string name = "Yield";
			string desc = "A card bearing this sigil is the only card on the board that attacks, skipping the enemy's turn";
			AbilityInfo info = AbilityInfoUtils.CreateInfoWithDefaultSettings(name, desc);
			var abIds = AbilityIdentifier.GetAbilityIdentifier(PluginGuid, info.rulebookName);

			// get art
			Texture2D tex = CardUtils.getAndloadImageAsTexture("ability_yield.png");

			// set ability to behavior class
			NewAbility newAbility = new NewAbility(info, typeof(Ability_TheWorld), tex, abIds);
			ability = newAbility.ability;

			return newAbility;
		}
	}
}
