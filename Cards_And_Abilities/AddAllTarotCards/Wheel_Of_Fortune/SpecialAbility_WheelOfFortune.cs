﻿using System.Collections;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using static AddAllTarotCards.HarmonyInit;

namespace AddAllTarotCards.Wheel_Of_Fortune
{
	public class SpecialAbility_WheelOfFortune : VariableStatBehaviour
	{
		public static NewSpecialAbility _SpecialAbility;
		public static SpecialStatIcon _iconType;

		protected override SpecialStatIcon IconType { get => _iconType; }

		private const int MAX_TOTAL_STATS = 7;
		private int attack = -1;
		private int health = -1;

		public override bool RespondsToDrawn()
		{
			return true;
		}

		public override IEnumerator OnDrawn()
		{
			Log.LogDebug("Called OnDrawn for WheelOfFortune");
			// this will generate a number that is >=1 and <= 6
			attack = UnityEngine.Random.Range(1, 7);
			health = MAX_TOTAL_STATS - attack; // 7 minus whatever attack is. Lowest value possible is 1.
			Log.LogDebug($"-> Wheel of Fortune - Attack [{attack}] Health [{health}]");
			return base.OnDrawn();
		}

		protected override int[] GetStatValues()
		{
			int[] array = new int[2];
			if (attack == -1 || health == -1)
			{
				Log.LogDebug("Attack and Health are still negative 1");
				return array;
			}

			array[0] = attack;
			array[1] = health;

			return array;
		}

		public static NewSpecialAbility InitAbility()
		{
			string name = "Wheel of Fortune";
			string desc =
				"When this card enters your hand, its power and health become two random non-zero numbers that add up to 7.";

			// setup ability
			StatIconInfo info = ScriptableObject.CreateInstance<StatIconInfo>();
			// icon will replace both attack and health numbers until played?
			info.appliesToAttack = true; // icon will replace both attack and health numbers until played
			info.appliesToHealth = true;
			info.rulebookName = name;
			info.rulebookDescription = desc;

			info.iconGraphic = CardUtils.getAndloadImageAsTexture("ability_wof_atk.png");
			var sId = SpecialAbilityIdentifier.GetID(PluginGuid, info.rulebookName);

			// set ability to behavior class
			var newAbility = new NewSpecialAbility(typeof(SpecialAbility_WheelOfFortune), sId, info);
			_iconType = newAbility.statIconInfo.iconType;
			_SpecialAbility = newAbility; // this is so we can use it in the HarmonyInit class

			return newAbility;
		}
	}
}
