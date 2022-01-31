namespace AddAllSCP.SCP_348_Thinking_Of_You
{
	public class ThinkingOfYouAbility : DiskCardGame.AbilityBehaviour
	{
		public static DiskCardGame.Ability ability;

		public override DiskCardGame.Ability Ability { get { return ability; } }

		private readonly System.Collections.Generic.Dictionary<DiskCardGame.PlayableCard, int> cardsWithTimesHealed = new();

		private static bool CardCanBeHealed(DiskCardGame.PlayableCard card)
		{
			return !card.Dead && card.Health < card.MaxHealth;
		}

		private static bool CardCannotBeHealed(DiskCardGame.PlayableCard card)
		{
			return !CardCanBeHealed(card);
		}

		public override bool RespondsToTurnEnd(bool playerTurnEnd)
		{
			return playerTurnEnd;
		}

		public void doLogicOnCardSlot(DiskCardGame.CardSlot slot)
		{
			if (slot == null || CardCannotBeHealed(slot.Card))
			{
				return;
			}

			DiskCardGame.PlayableCard card = slot.Card;

			if (cardsWithTimesHealed.TryGetValue(card, out int total) && total < 3)
			{
				HarmonyInitAll.Log.LogInfo($"Healing [{card.name}. Has been healed [{total}] so far.");
				card.HealDamage(1);
				cardsWithTimesHealed[card] = ++total;
			}
			else
			{
				HarmonyInitAll.Log.LogInfo($"Adding [{card.name} to dictionary.");
				cardsWithTimesHealed.Add(card, 0);
			}
		}

		public override System.Collections.IEnumerator OnTurnEnd(bool playerTurnEnd)
		{
			yield return base.PreSuccessfulTriggerSequence();

			DiskCardGame.CardSlot toLeft = DiskCardGame.BoardManager.Instance.GetAdjacent(base.Card.Slot, true);
			DiskCardGame.CardSlot toRight = DiskCardGame.BoardManager.Instance.GetAdjacent(base.Card.Slot, false);

			doLogicOnCardSlot(toLeft);
			doLogicOnCardSlot(toRight);

			yield return new UnityEngine.WaitForSeconds(0.1f);
			yield return base.LearnAbility(0.5f);
			yield break;
		}

		protected internal static APIPlugin.NewAbility InitAbility()
		{
			const string rulebookName = "Home Cooking";
			const string description =
				"Heal neighbor cards by 1 each turn up to 3 times while neighbor card is not at max health. " +
				"Does not increase health more than the total of the card.";

			// setup ability
			DiskCardGame.AbilityInfo info =
				APIPlugin.AbilityInfoUtils.CreateInfoWithDefaultSettings(rulebookName, description);

			// get and load artwork
			UnityEngine.Texture2D sigilTex =
				APIPlugin.CardUtils.LoadImageAndGetTexture("scp_348_sigil_small.png");

			// set ability to behavior class
			APIPlugin.NewAbility newAbility = new APIPlugin.NewAbility(
				info, typeof(ThinkingOfYouAbility), sigilTex,
				APIPlugin.AbilityIdentifier.GetID(HarmonyInitAll.PluginGuid, info.rulebookName)
			);
			ability = newAbility.ability;

			return newAbility;
		}
	}
}
