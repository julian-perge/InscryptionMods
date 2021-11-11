using System;
using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace MoreAnts
{
	[HarmonyPatch(typeof(CardTriggerHandler), "AddAbility", new Type[] { typeof(SpecialTriggeredAbility) })]
	public class CardTriggerHandler_AddAbility
	{
		public static bool Prefix(SpecialTriggeredAbility ability, CardTriggerHandler __instance)
		{
			if ((int)ability < 99)
			{
				return true;
			}

			FileLog.Log("Patching special abilities");
			bool SpecialAbilityDoesExist(Tuple<SpecialTriggeredAbility, SpecialCardBehaviour> tuple) 
				=> tuple.Item1 == ability;

			// return true if the ability does not exist
			if (!__instance.specialAbilities.Exists(SpecialAbilityDoesExist))
			{
				NewSpecialAbility newAbility = NewSpecialAbility.specialAbilities.Find((NewSpecialAbility x) 
					=> x.specialTriggeredAbility == ability);
				Type type = newAbility.specialCardBehaviour;
				Component baseC = (Component)__instance;
				SpecialCardBehaviour item = baseC.gameObject.GetComponent(type) as SpecialCardBehaviour;
				if (item == null)
				{
					item = baseC.gameObject.AddComponent(type) as SpecialCardBehaviour;
				}

				__instance.specialAbilities.Add(new Tuple<SpecialTriggeredAbility, SpecialCardBehaviour>(ability, item));
			}

			return false;
		}

		public static bool AbilityCanStackAndIsNotPassive(Ability ability)
		{
			return AbilitiesUtil.GetInfo(ability).canStack && !AbilitiesUtil.GetInfo(ability).passive;
		}
	}
}
