using APIPlugin;
using BepInEx;
using DiskCardGame;
using HarmonyLib;

namespace Exodia
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
	public class HarmonyInit : BaseUnityPlugin
	{
		public const string PluginGuid = "com.julianperge";
		private const string PluginName = "exodia";
		private const string PluginVersion = "1.2";

		private void Awake()
		{
			Exodia.Card.InitCards();
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
				CardInfo card = CardLoader.GetCardByName(Exodia.Card.Name);
				CardInfo cardLeftArm = CardLoader.GetCardByName(Exodia.Card.Name);
				CardInfo cardRightArm = CardLoader.GetCardByName(Exodia.Card.Name);

				CardUtils.PrintCardInfo(card);

				SaveManager.SaveFile.CurrentDeck.Cards.Clear();

				SaveManager.SaveFile.CurrentDeck.Cards.Add(card);
				SaveManager.SaveFile.CurrentDeck.Cards.Add(cardLeftArm);
				SaveManager.SaveFile.CurrentDeck.Cards.Add(cardRightArm);
			}
		}
	}
}
