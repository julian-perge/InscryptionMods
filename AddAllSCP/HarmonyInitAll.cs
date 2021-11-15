using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using APIPlugin;
using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using PeterHan.PLib.Utils;

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

			SCP_348_Thinking_Of_You.Card.InitCard();

			// WORKS
			// SCP_354_Blood_Pond.Card.InitCardsAndAbilities();

			// SCP_999_Tickle_Monster.Card.InitCard();

			var harmony = new Harmony(PluginGuid);
			harmony.PatchAll();
		}

		// add this to your deck by scrolling upwards/pressing w key when at the map
		[HarmonyPatch(typeof(DeckReviewSequencer), nameof(DeckReviewSequencer.OnEnterDeckView))]
		public class AddSCPCardToDeckPatch
		{
			[HarmonyPrefix]
			public static void AddSCP()
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

		[HarmonyPatch]
		public class ChangeBoonToGiveTwentyBones
		{
			[HarmonyTargetMethods]
			static IEnumerable<MethodBase> ReturnMoveNextMethodFromNestedEnumerator(Harmony _)
			{
				// StatBoostSequence is the IEnumerator method, but there's a hidden compiler class, <StatBoostSequence>d__12,
				//	that actually has all the byte code to look for.
				Type getEnumeratorType = AccessTools.TypeByName("DiskCardGame.BoonsHandler+<ActivatePreCombatBoons>d__4");
				return AccessTools.GetDeclaredMethods(getEnumeratorType).Where(m => m.Name.Equals("MoveNext"));
			}

			[HarmonyTranspiler]
			internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
			{
				return increaseBonesBoon
					? PPatchTools.ReplaceConstant(instructions, 8, 20, false)
					: instructions;
			}
		}

		[HarmonyPatch(typeof(DeckInfo), "Boons", MethodType.Getter)]
		public class StartWith
		{
			[HarmonyPostfix]
			public static List<BoonData> AddBoons(List<BoonData> __result)
			{
				if (increaseBonesBoon)
				{
					__result.Add(BoonsUtil.GetData(BoonData.Type.StartingBones));
				}

				__result.Add(BoonsUtil.GetData(BoonData.Type.StartingGoat));
				// __result.Add(BoonsUtil.GetData(BoonData.Type.DoubleDraw));
				return __result;
			}
		}
	}
}
