namespace AddAllSCP;

[BepInEx.BepInPlugin(PluginGuid, PluginName, PluginVersion)]
[BepInEx.BepInDependency(CyantistInscryptionAPI, BepInEx.BepInDependency.DependencyFlags.HardDependency)]
[BepInEx.BepInDependency(SigilADay_julianPerge, BepInEx.BepInDependency.DependencyFlags.HardDependency)]
public class HarmonyInitAll : BepInEx.BaseUnityPlugin
{
	public const string CyantistInscryptionAPI = "cyantist.inscryption.api";
	public const string SigilADay_julianPerge = "julianperge.inscryption.sigiladay";

	public const string PluginGuid = "julianperge.inscryption.scpUniverse";
	public const string PluginName = "SCP_Universe";
	public const string PluginVersion = "0.1.0";

	internal static BepInEx.Logging.ManualLogSource Log;

	void Awake()
	{
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
		SCP_096_Shy_Guy.Card.InitCardStage1();
		SCP_096_Shy_Guy.Card.InitCardStage2();
		SCP_096_Shy_Guy.Card.InitCardStage3();

		// SCP_348_Thinking_Of_You.Card.InitCard();

		// WORKS
		// SCP_354_Blood_Pond.Card.InitCardsAndAbilities();

		// SCP_999_Tickle_Monster.Card.InitCard();

		var harmony = new HarmonyLib.Harmony(PluginGuid);
		harmony.PatchAll();
	}

	// add this to your deck by scrolling upwards/pressing w key when at the map
	[HarmonyLib.HarmonyPatch(typeof(DiskCardGame.DeckReviewSequencer), "OnEnterDeckView")]
	public class AddCardsToDeckPatch
	{
		private static bool allowSettingDeck = true;

		[HarmonyLib.HarmonyPrefix]
		public static void AddCards()
		{
			if (allowSettingDeck)
			{
				SaveManager.SaveFile.CurrentDeck.Cards.Clear();

				SaveManager.SaveFile.CurrentDeck.Cards.Add(DiskCardGame.CardLoader.GetCardByName("scp_096_stage1"));
				SaveManager.SaveFile.CurrentDeck.Cards.Add(DiskCardGame.CardLoader.GetCardByName("Stinkbug_Talking"));
				SaveManager.SaveFile.CurrentDeck.Cards.Add(DiskCardGame.CardLoader.GetCardByName("Stinkbug_Talking"));
				SaveManager.SaveFile.CurrentDeck.Cards.Add(DiskCardGame.CardLoader.GetCardByName("Stinkbug_Talking"));
				// SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName(SCP_034_Obsidian_Ritual_Knife.Card.Name));
				// SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName(SCP_035_Porcelain_Mask.Card.Name));
				// SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName(SCP_049_Plague_Doctor.Card.Name));
				// SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName(SCP_087_The_Stairwell.Card.Name));
				// SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName(SCP_348_Thinking_Of_You.Card.Name));
				// SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName(SCP_354_Blood_Pond.Card.Name));
				// SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName(SCP_999_Tickle_Monster.Card.Name));
			}
		}
	}
}
