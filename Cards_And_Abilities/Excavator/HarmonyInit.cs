using System.Collections.Generic;
using APIPlugin;
using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;

namespace Excavator
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
	public class HarmonyInit : BaseUnityPlugin
	{
		public const string PluginGuid = "julian.inscryption.abilities.excavator";
		private const string PluginName = "Custom_Ability_Excavator";
		private const string PluginVersion = "1.1";

		internal static ManualLogSource Log;

		void Awake()
		{
			Log = base.Logger;

			NewAbility ability = ExcavatorAbility.InitAbility();
			var abilities = new List<Ability>() { ability.ability };
			new CustomCard("Stinkbug_Talking") { abilities = abilities };

			var harmony = new Harmony(PluginGuid);
			harmony.PatchAll();
		}
	}

	// add this to your deck by scrolling upwards/pressing w key when at the map
	[HarmonyPatch(typeof(DeckReviewSequencer), nameof(DeckReviewSequencer.OnEnterDeckView))]
	public class AddCardsToDeckPatch
	{
		private static bool allowSettingDeck = false;

		[HarmonyPrefix]
		public static void AddCardsToDeck()
		{
			if (allowSettingDeck)
			{
				CardInfo boulder = CardLoader.GetCardByName("Boulder");
				CardInfo stinkbug = CardLoader.GetCardByName("Stinkbug_Talking");

				// CardUtils.PrintCardInfo(card);

				SaveManager.SaveFile.CurrentDeck.Cards.Clear();

				SaveManager.SaveFile.CurrentDeck.Cards.Add(boulder);
				SaveManager.SaveFile.CurrentDeck.Cards.Add(boulder);
				SaveManager.SaveFile.CurrentDeck.Cards.Add(boulder);
				SaveManager.SaveFile.CurrentDeck.Cards.Add(stinkbug);
			}
		}
	}
}
