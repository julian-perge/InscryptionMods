using System;
using System.Collections;
using System.Collections.Generic;
using APIPlugin;
using BepInEx;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace ExodiaAbility
{
	[BepInPlugin("com.julianperge.exodia", "Exodia", "1.0")]
	public class ExodiaPatch : BaseUnityPlugin
	{
		void Awake()
		{
			var harmony = new Harmony("com.julianperge.exodia");
			harmony.PatchAll();
			addExodia();
		}

		void addExodia()
		{
			AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
			info.ability = (Ability)100;
			info.powerLevel = 0;
			info.triggerText = "EXODIA, OBLITERATE";
			info.rulebookName = "Exodia";
			info.rulebookDescription = "You automatically win";
			info.metaCategories = new List<AbilityMetaCategory>()
			{
				AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part1Rulebook
			};
			var imgBytes = System.IO.File.ReadAllBytes("BepInEx/plugins/CardLoader/Artwork/exodia.png");
			Texture2D tex = new Texture2D(2, 2);
			tex.LoadImage(imgBytes);
			new NewAbility((Ability) 100, info, typeof(ExodiaAbility), tex);
		}
	}

	public class ExodiaAbility : DiskCardGame.AbilityBehaviour
	{
		public override Ability Ability
		{
			get
			{
				return (Ability)100;
			}
		}

		// public override IEnumerator OnPlayFromHand()
		// {
		// 	Console.WriteLine("Inside OnPlayFromhand");
		// 	int dmgToWin = Singleton<LifeManager>.Instance.DamageUntilPlayerWin;
		// 	yield return base.PreSuccessfulTriggerSequence();
		// 	yield return Singleton<LifeManager>.Instance.ShowDamageSequence(
		// 		dmgToWin,
		// 		1, false, 0.25f,
		// 		ResourceBank.Get<GameObject>("Prefabs/Environment/ScaleWeights/Weight_RealTooth"), 
		// 		3f);
		// }
	}
}
