using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;

namespace MoreAnts
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency("cyantist.inscryption.api")]
	public class MoreAnts : BaseUnityPlugin
	{
		public const string PluginGuid = "julianperge.inscryption.cards.moreAnts";
		private const string PluginName = "MoreAnts";
		private const string PluginVersion = "1.0";
		internal static ManualLogSource Log;

		void Awake()
		{
			Log = base.Logger;

			DomeAnt.InitCard();

			Harmony harmony = new Harmony(PluginGuid);
			harmony.PatchAll();
		}
	}

	// add this to your deck by scrolling upwards/pressing w key when at the map
	[HarmonyPatch(typeof(DeckReviewSequencer), nameof(DeckReviewSequencer.OnEnterDeckView))]
	public class AddCardsToDeckPatch
	{
		private static bool allowSettingDeck = true;

		[HarmonyPrefix]
		public static void AddCardsToDeck()
		{
			if (allowSettingDeck)
			{
				CardInfo card = CardLoader.GetCardByName("DomeAnt");
				// foreach (var specialTriggeredAbility in card.SpecialAbilities)
				// {
				// 	MoreAnts.Log.LogInfo($"-> special ability for [{card.name}] => [{specialTriggeredAbility}]");
				// }

				SaveManager.SaveFile.CurrentDeck.Cards.Clear();

				// SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName("Geck"));
				// SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName("Ant"));
				SaveManager.SaveFile.CurrentDeck.Cards.Add(card);
				SaveManager.SaveFile.CurrentDeck.Cards.Add(card);
				SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName("Ant"));
				SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName("Ant"));
			}
		}
	}
}
