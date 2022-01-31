namespace AddAllSCP.SCP_354_Blood_Pond
{
	public class BloodPondAbility : DiskCardGame.AbilityBehaviour
	{
		public static DiskCardGame.Ability ability;

		public override DiskCardGame.Ability Ability { get { return ability; } }

		private int turnsTaken = 1;

		public override bool RespondsToTurnEnd(bool playerTurnEnd)
		{
			HarmonyInitAll.Log.LogDebug($"[RespondsToTurnEnd] Current turns taken [{turnsTaken}]");
			return playerTurnEnd;
		}

		public override System.Collections.IEnumerator OnTurnEnd(bool playerTurnEnd)
		{
			yield return base.PreSuccessfulTriggerSequence();
			System.Collections.Generic.List<DiskCardGame.CardSlot> slots = DiskCardGame.BoardManager.Instance
				.GetSlots(true)
				.FindAll(slot => slot is not null && slot.Card is null);
			// if no available slots, list will be empty and won't loop
			HarmonyInitAll.Log.LogDebug($"[BloodPond] Number of slots available [{slots.Count}]");
			foreach (var slot in slots)
			{
				var bloodCardToSpawn
					= turnsTaken++ < 3
						? DiskCardGame.CardLoader.GetCardByName("SCP_354_BloodCreature")
						: DiskCardGame.CardLoader.GetCardByName("SCP_354_BloodEntity");

				HarmonyInitAll.Log.LogDebug($"-> Spawning a [{bloodCardToSpawn.name}]");
				yield return DiskCardGame.BoardManager.Instance.CreateCardInSlot(bloodCardToSpawn, slot, 0.1f, true);
				break; // only spawn one, then break out of loop and end resolve.
			}

			yield return new UnityEngine.WaitForSeconds(0.1f);
			yield return base.LearnAbility(0.5f);
			yield break;
		}

		protected internal static APIPlugin.NewAbility InitAbility()
		{
			const string rulebookName = "Blood Pond";
			const string description =
				"Spawn a Blood Creature (1/1 w/ Brittle) at the end of your turn in a random slot. " +
				"After 3 turns, spawn a Blood Entity (1/1 w/ Brittle).";

			// setup ability
			DiskCardGame.AbilityInfo info =
				APIPlugin.AbilityInfoUtils.CreateInfoWithDefaultSettings(rulebookName, description);

			// get and load artwork
			UnityEngine.Texture2D sigilTex =
				APIPlugin.CardUtils.LoadImageAndGetTexture("scp_354_blood_pond_ability_small.png");

			// set ability to behavior class
			APIPlugin.NewAbility bloodPondAbility = new APIPlugin.NewAbility(info, typeof(BloodPondAbility), sigilTex,
				APIPlugin.AbilityIdentifier.GetID(HarmonyInitAll.PluginGuid, info.rulebookName));
			ability = bloodPondAbility.ability;

			return bloodPondAbility;
		}
	}
}
