using static AddAllTarotCards.HarmonyInit;

namespace AddAllTarotCards.The_World
{
	public class Ability_TheWorld : DiskCardGame.AbilityBehaviour
	{
		public static DiskCardGame.Ability ability;
		public override DiskCardGame.Ability Ability { get { return ability; } }

		public override bool RespondsToResolveOnBoard()
		{
			return true;
		}

		public override System.Collections.IEnumerator OnResolveOnBoard()
		{
			yield return new UnityEngine.WaitForSeconds(0.25f);
			Singleton<DiskCardGame.TurnManager>.Instance.Opponent.SkipNextTurn = true;
			Singleton<DiskCardGame.TextDisplayer>.Instance.StartCoroutine(Singleton<DiskCardGame.TextDisplayer>.Instance
				.ShowUntilInput("I must yield to The World...", -0.65f, 0.4f));
			yield break;
		}

		public static APIPlugin.NewAbility InitAbility()
		{
			// setup ability
			const string name = "Yield";
			const string desc =
				"A card bearing this sigil is the only card on the board that attacks, skipping the enemy's turn";
			DiskCardGame.AbilityInfo info = APIPlugin.AbilityInfoUtils.CreateInfoWithDefaultSettings(name, desc);
			var abIds = APIPlugin.AbilityIdentifier.GetID(PluginGuid, info.rulebookName);

			// get art
			UnityEngine.Texture2D tex = APIPlugin.CardUtils.LoadImageAndGetTexture("ability_yield.png");

			// set ability to behavior class
			APIPlugin.NewAbility newAbility = new APIPlugin.NewAbility(info, typeof(Ability_TheWorld), tex, abIds);
			ability = newAbility.ability;

			return newAbility;
		}
	}
}
