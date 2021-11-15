using APIPlugin;
using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using Card = AddAllSCP.SCP_348_Thinking_Of_You.Card;

namespace AddAllSCP
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
	public class HarmonyInitAll : BaseUnityPlugin
	{
		public const string PluginGuid = "com.julianperge";
		public const string PluginName = "scp_universe";
		public const string PluginVersion = "1.0";

		private static bool allowSettingDeck = false;
		private static bool increaseBonesBoon = false;

		internal static ManualLogSource Log;

		void Awake()
		{
			Logger.LogInfo($"Loaded {PluginName}!");
			Log = base.Logger;

			// TODO: Need to mimic DiskCardGame.Transformer/DiskCardGame.Evolve-like transformations;
			// SCP_034_Obsidian_Ritual_Knife.Card.InitCard();

			// WORKS
			// SCP_035_Porcelain_Mask.Card.InitCard();

			// WORKS
			// SCP_049_Plague_Doctor.Card.InitCard();

			// WORKS
			// SCP_087_The_Stairwell.Card.InitCard();

			// TODO: Still needs to implement following card if card has strafe
			// SCP_096_Shy_Guy.Card.InitCard();

			Card.InitCard();

			// WORKS
			// SCP_354_Blood_Pond.Card.InitCardsAndAbilities();

			// SCP_999_Tickle_Monster.Card.InitCard();

			var harmony = new Harmony(PluginGuid);
			harmony.PatchAll();
		}

		// add this to your deck by scrolling upwards/pressing w key when at the map
		[HarmonyPatch(typeof(DeckReviewSequencer), nameof(DeckReviewSequencer.OnEnterDeckView))]
		public class AddCardsToDeckPatch
		{
			[HarmonyPrefix]
			public static void AddCards()
			{
				if (allowSettingDeck)
				{
					CardInfo scp = CardLoader.GetCardByName(SCP_034_Obsidian_Ritual_Knife.Card.Name);
					CardUtils.PrintCardInfo(scp);
					// CardInfo scp087 = CardLoader.GetCardByName("SCP_087_TheStairwell");
					SaveManager.SaveFile.CurrentDeck.Cards.Clear();

					SaveManager.SaveFile.CurrentDeck.Cards.Add(scp);
					SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName("Goat"));
					SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName("Snapper"));
					SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName("Snapper"));
				}
			}
		}
	}
}
