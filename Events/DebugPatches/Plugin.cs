using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace DebugPatches
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	public class Plugin : BaseUnityPlugin
	{
		public const string PluginGuid = "julianperge.inscryption.enableDebugTools";
		public const string PluginName = "julianperge_EnableDebuggingTools";
		private const string PluginVersion = "0.2.0";

		internal static ManualLogSource Log;

		private List<DiskCardGame.EncounterBlueprintData> resourceList;

		private Dictionary<String, DiskCardGame.Opponent.Type> BossTypesByName = new()
		{
			{ "AnglerBossP1", DiskCardGame.Opponent.Type.AnglerBoss },
			{ "AnglerBossP2", DiskCardGame.Opponent.Type.AnglerBoss },
			{ "LeshyBossP1", DiskCardGame.Opponent.Type.LeshyBoss },
			{ "ProspectorBossP1", DiskCardGame.Opponent.Type.ProspectorBoss },
			{ "ProspectorBossP2", DiskCardGame.Opponent.Type.ProspectorBoss },
			{ "TrapperTraderBossP1", DiskCardGame.Opponent.Type.TrapperTraderBoss },
		};

		private void Awake()
		{
			Log = base.Logger;

			resourceList = UnityEngine.Resources
				.LoadAll<DiskCardGame.EncounterBlueprintData>("Data/EncounterBlueprints/Part1").ToList();

			var harmony = new Harmony(PluginGuid);
			harmony.PatchAll();
		}

		private bool toggleEncounterMenu = false;

		private void OnGUI()
		{
			string[] names = resourceList.Select(res => res.name).ToArray();

			toggleEncounterMenu = UnityEngine.GUI.Toggle(
				new UnityEngine.Rect(20, 280, 200, 20),
				toggleEncounterMenu,
				"Encounter Menu"
			);

			if (!toggleEncounterMenu) return;

			int selectedButton = UnityEngine.GUI.SelectionGrid(
				new UnityEngine.Rect(25, 300, 300, 300),
				-1,
				names,
				2
			);

			if (selectedButton > 0)
			{
				DiskCardGame.EncounterBlueprintData encounter = resourceList[selectedButton];
				// the asset names have P1 or P2 at the end,
				//	so we'll remove it so that we can correctly get a boss if a boss was selected
				string scrubbedName = encounter.name.Replace("P1", "").Replace("P2", "");

				DiskCardGame.CardBattleNodeData node = new DiskCardGame.CardBattleNodeData()
				{
					difficulty = DiskCardGame.RunState.Run.regionTier * 6 + 3 / 3 - 1, blueprint = resourceList[selectedButton]
				};

				if (Enum.TryParse(scrubbedName, true, out DiskCardGame.Opponent.Type bossType))
				{
					node = new DiskCardGame.BossBattleNodeData();
					((DiskCardGame.BossBattleNodeData)node).specialBattleId =
						DiskCardGame.BossBattleSequencer.GetSequencerIdForBoss(bossType);
					((DiskCardGame.BossBattleNodeData)node).bossType = bossType;
				}

				DiskCardGame.Opponent opponent = Singleton<DiskCardGame.TurnManager>.Instance.Opponent;
				if (opponent is not null && !Singleton<DiskCardGame.TurnManager>.Instance.GameIsOver())
				{
					Log.LogDebug($"Setting NumLives to zero");
					opponent.NumLives = 1;
					Log.LogDebug($"Game is not over, making opponent surrender");
					opponent.SurrenderImmediate();
					Log.LogDebug($"Playing LifeLostSequence for [{opponent.GetType()}]");
					base.StartCoroutine(opponent.LifeLostSequence());
				}

				Log.LogDebug($"Transitioning to encounter [{resourceList[selectedButton].name}]");
				CustomCoroutine.WaitOnConditionThenExecute(
					() => Singleton<DiskCardGame.GameFlowManager>.Instance.CanTransitionToFirstPerson(), delegate
					{
						Log.LogDebug($"-> No longer in transitioning state, transitioning to new encounter");
						Singleton<DiskCardGame.GameFlowManager>.Instance.TransitionToGameState(DiskCardGame.GameState.CardBattle,
							node);
					});
			}

			// EncounterData encounterData = EncounterBuilder.Build(bossBattleNodeData, true);
			// encounterData.opponentTurnPlan =
			// 	EncounterBuilder.BuildOpponentTurnPlan(encounterData.Blueprint, bossBattleNodeData.difficulty, false);
			// encounterData.opponentType = bossBattleNodeData.bossType;
			//
			// EncounterData.StartCondition startCondition = new EncounterData.StartCondition
			// {
			// 	cardsInPlayerSlots = new[] { CardLoader.GetCardByName("Geck") },
			// 	cardsInOpponentSlots = new[] { CardLoader.GetCardByName("Geck") }
			// };
			// encounterData.startConditions.Add(startCondition);
		}
	}

	[HarmonyPatch(typeof(DiskCardGame.Part1FinaleSceneSequencer), nameof(DiskCardGame.Part1FinaleSceneSequencer.Start))]
	public class Part1FinaleSceneSequencerDebugTransitionPatch
	{
		[HarmonyTranspiler]
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			// Plugin.Log.LogDebug($"Setting DebugTransition to true");
			bool InstructionIsFieldInfo(CodeInstruction ins)
				=> ins.opcode == OpCodes.Stfld && ins.operand is FieldInfo { Name: "debugTransition" };

			return new CodeMatcher(instructions)
				.Start()
				.MatchForward(false,
					new CodeMatch(OpCodes.Ldc_I4_0),
					new CodeMatch(InstructionIsFieldInfo)
				)
				.SetOpcodeAndAdvance(OpCodes.Ldc_I4_1)
				.InstructionEnumeration();
		}
	}
}
