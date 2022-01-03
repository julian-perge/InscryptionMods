using static AddAllTarotCards.HarmonyInit;

namespace AddAllTarotCards.The_Hanged_Man
{
	public class Ability_TheHangedMan : DiskCardGame.AbilityBehaviour
	{
		public static DiskCardGame.Ability ability;
		public override DiskCardGame.Ability Ability { get => ability; }

		private int overkillDmg = 0;
		private bool willHangedManDie;
		private bool willFriendlyCardDie;
		private DiskCardGame.CardModificationInfo modsFromAttackedCard = new();

		public override bool RespondsToSlotTargetedForAttack(DiskCardGame.CardSlot slot, DiskCardGame.PlayableCard attacker)
		{
			return attacker
			       && base.Card.Slot.IsPlayerSlot == slot.IsPlayerSlot
			       && base.Card.Slot != slot;
		}

		public override System.Collections.IEnumerator OnSlotTargetedForAttack(DiskCardGame.CardSlot slot,
			DiskCardGame.PlayableCard attacker)
		{
			// base.Card.Health == 2
			// attacker.lastFrameAttack == 3
			// slot.Card.Health == 2
			if (attacker.Attack > base.Card.Health)
			{
				// overkillDmg == 1;
				overkillDmg = attacker.Attack - base.Card.Health;
				willHangedManDie = true;
				base.StartCoroutine(Singleton<DiskCardGame.TextDisplayer>
					.Instance
					.ShowThenClear("The Hanged Man has taken their last breath", 3f)
				);
				yield return new UnityEngine.WaitForSeconds(0.75f);
				// using example above, friendly card won't die
				// 2 minus 1 == 1
				willFriendlyCardDie = slot.Card.Health - overkillDmg <= 0;
				Log.LogDebug($"Friendly card [{slot.Card.Info.name}] will die? [{willFriendlyCardDie}]");
			}
			else
			{
				Log.LogDebug($"HangedMan won't die, add [{attacker.Attack}] temp health to [{slot.Card.Info.name}]");
				modsFromAttackedCard.healthAdjustment = attacker.Attack;
				slot.Card.AddTemporaryMod(modsFromAttackedCard);
			}

			yield break;
		}

		public override bool RespondsToOtherCardDealtDamage(DiskCardGame.PlayableCard attacker, int amount,
			DiskCardGame.PlayableCard target)
		{
			// Log.LogDebug($"Card [{target.Info.name}] is being attacked by [{attacker.Info.name}]");
			return attacker
			       && base.Card.Slot.IsPlayerSlot == target.Slot.IsPlayerSlot
			       && base.Card.Slot != target.Slot;
		}

		public override System.Collections.IEnumerator OnOtherCardDealtDamage(DiskCardGame.PlayableCard attacker,
			int amount, DiskCardGame.PlayableCard target)
		{
			Singleton<DiskCardGame.ViewManager>.Instance.SwitchToView(DiskCardGame.View.Board);
			yield return new UnityEngine.WaitForSeconds(0.1f);
			yield return base.PreSuccessfulTriggerSequence();

			// 3 minus 1 == 2
			int dmgToTake = amount - overkillDmg;
			Log.LogDebug($"-> The Hanged Man card should take [{dmgToTake}] damage");
			yield return new UnityEngine.WaitForSeconds(0.2f);
			base.Card.Anim.StrongNegationEffect();
			// does from taking 2 damage
			yield return base.Card.TakeDamage(dmgToTake, attacker);

			// friendly card won't die in example above
			if (!willFriendlyCardDie)
			{
				Log.LogDebug($"-> [{target.Info.name}] being healed for [{dmgToTake}]");
				target.HealDamage(dmgToTake);

				Log.LogDebug($"-> [{target.Info.name}] removing temporary mods");
				target.RemoveTemporaryMod(modsFromAttackedCard);
			}

			yield return base.LearnAbility();
			yield break;
		}

		public static APIPlugin.NewAbility InitAbility()
		{
			// setup ability
			const string name = "Martyr";
			const string desc = "When another friendly card would take damage, a card with this sigil receives it instead";
			DiskCardGame.AbilityInfo info = APIPlugin.AbilityInfoUtils.CreateInfoWithDefaultSettings(name, desc);
			var abIds = APIPlugin.AbilityIdentifier.GetID(PluginGuid, info.rulebookName);

			// get art
			UnityEngine.Texture2D tex = APIPlugin.CardUtils.LoadImageAndGetTexture("ability_martyr.png");

			// set ability to behavior class
			APIPlugin.NewAbility newAbility = new APIPlugin.NewAbility(info, typeof(Ability_TheHangedMan), tex, abIds);
			ability = newAbility.ability;

			return newAbility;
		}
	}
}
