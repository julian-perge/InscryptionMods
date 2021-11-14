using System;
using BepInEx;
using DiskCardGame;
using HarmonyLib;

namespace AddAllSCP
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
	public class HarmonyInitAll
	{
		private const string PluginGuid = "com.julianperge";
		private const string PluginName = "scp_universe";
		private const string PluginVersion = "1.0";

		void Awake()
		{
			var harmony = new Harmony(PluginGuid);
			harmony.PatchAll();


			SCP_035_Porcelain_Mask.MaskAbility.InitAbility();
			SCP_035_Porcelain_Mask.Card.InitCard();
			
			SCP_049_Plague_Doctor.Card.InitCard();
			SCP_049_Plague_Doctor.DoubleDeathTweaked.InitAbility();

			SCP_087_The_Stairwell.Card.InitCard();
			SCP_087_The_Stairwell.TheStairwellAbility.InitAbility();

			SCP_096_Shy_Guy.TheSightAbility.InitAbility();
			SCP_096_Shy_Guy.Card.InitCard();
			
			SCP_354_Blood_Pond.Card.InitCard();
			SCP_354_Blood_Pond.BloodPondAbility.InitAbility();
			
			SCP_999_Tickle_Monster.Card.InitCard();
		}

		// add this to your deck by scrolling upwards/pressing w key when at the map
		[HarmonyPatch(typeof(DeckReviewSequencer), nameof(DeckReviewSequencer.OnEnterDeckView))]
		public class AddPaleManToDeckPatch
		{
			[HarmonyPrefix]
			public static void AddShyGuy()
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
