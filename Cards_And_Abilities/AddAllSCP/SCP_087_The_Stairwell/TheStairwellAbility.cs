namespace AddAllSCP.SCP_087_The_Stairwell
{
	public class TheStairwellAbility : DiskCardGame.AbilityBehaviour
	{
		public static DiskCardGame.Ability ability;

		public override DiskCardGame.Ability Ability { get { return ability; } }

		private int turnsTaken = 1;

		public override bool RespondsToUpkeep(bool playerUpkeep)
		{
			return playerUpkeep && (base.Card is not null && !base.Card.Dead);
		}

		public override System.Collections.IEnumerator OnUpkeep(bool playerUpkeep)
		{
			var opposingSlotCard = base.Card.Slot.opposingSlot.Card;
			HarmonyInitAll.Log.LogDebug($"[StairwellAbility] Current turns {turnsTaken}, plus 1 [{turnsTaken + 1}]");

			// if it's not the 3rd turn, we don't care to check the opposing slot card
			if (turnsTaken++ != 3)
			{
				yield break;
			}

			// now reset regardless if opposing slot card is null
			turnsTaken = 0;

			if (opposingSlotCard is null)
			{
				yield break;
			}

			HarmonyInitAll.Log.LogDebug($"[StairwellAbility] Killing card {opposingSlotCard.name}");

			Singleton<DiskCardGame.ViewManager>.Instance.SwitchToView(DiskCardGame.View.Board, false, false);

			yield return new UnityEngine.WaitForSeconds(0.1f);
			yield return opposingSlotCard.Die(false, base.Card);
			yield return base.LearnAbility(0.25f);
			yield return new UnityEngine.WaitForSeconds(0.1f);
			yield break;
		}

		public static APIPlugin.NewAbility InitAbility()
		{
			// setup ability
			const string rulebookName = "The Stairwell";
			const string description = "At the start of every 3rd turn, automatically kill the card in the opposing slot.";
			DiskCardGame.AbilityInfo info =
				APIPlugin.AbilityInfoUtils.CreateInfoWithDefaultSettings(rulebookName, description);

			// get and load artwork
			UnityEngine.Texture2D sigilTex =
				APIPlugin.CardUtils.getAndloadImageAsTexture("scp_087_ability.png");

			// set ability to behavior class
			APIPlugin.NewAbility theStairwellAbility = new APIPlugin.NewAbility(info, typeof(TheStairwellAbility), sigilTex,
				APIPlugin.AbilityIdentifier.GetAbilityIdentifier(HarmonyInitAll.PluginGuid, info.rulebookName));
			ability = theStairwellAbility.ability;

			return theStairwellAbility;
		}
	}
}
