using System;
using System.Collections.Generic;
using System.IO;
using APIPlugin;
using BepInEx;
using CardLoaderPlugin.lib;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using Logger = HarmonyLib.Tools.Logger;

namespace ThePaleManCard
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
	public class HarmonyInit : BaseUnityPlugin
	{
		
		private const string PluginGuid = "com.julianperge.scp096";
		private const string PluginName = "scp096";
		private const string PluginVersion = "1.0";
		
		public void Awake()
		{
			var harmony = new Harmony(PluginGuid);
			harmony.PatchAll();

			TheSightAbility.InitAbility();
			Card.InitCard();
		}

		
		// add this to your deck by scrolling upwards/pressing w key when at the map
		[HarmonyPatch(typeof(DeckReviewSequencer), nameof(DeckReviewSequencer.OnEnterDeckView))]
		public class AddPaleManToDeckPatch
		{
			[HarmonyPrefix]
			public static void AddPaleMan()
			{
				CardInfo info = CardLoader.GetCardByName("SCP096_ShyGuy");
				var currDeck = SaveManager.SaveFile.CurrentDeck;
				if (!currDeck.Cards.Exists(card => card.displayedName == info.displayedName))
				{
					SaveManager.SaveFile.CurrentDeck.Cards.RemoveRange(0, 1);
					SaveManager.SaveFile.CurrentDeck.Cards.Add(info);
				}
			}
		}
		



	}
}
