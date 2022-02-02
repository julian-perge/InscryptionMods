using System.Reflection;
using System.Reflection.Emit;
using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DebugPatches;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
	public const string PluginGuid = "julianperge.inscryption.enableDebugTools";
	public const string PluginName = "julianperge_EnableDebuggingTools";
	private const string PluginVersion = "0.2.0";

	internal static ManualLogSource Log;

	private List<EncounterBlueprintData> resourceList => Resources
		.LoadAll<EncounterBlueprintData>("Data/EncounterBlueprints/Part1").ToList();

	private Dictionary<String, Opponent.Type> BossTypesByName = new()
	{
		{ "AnglerBossP1", Opponent.Type.AnglerBoss },
		{ "AnglerBossP2", Opponent.Type.AnglerBoss },
		{ "LeshyBossP1", Opponent.Type.LeshyBoss },
		{ "ProspectorBossP1", Opponent.Type.ProspectorBoss },
		{ "ProspectorBossP2", Opponent.Type.ProspectorBoss },
		{ "TrapperTraderBossP1", Opponent.Type.TrapperTraderBoss },
	};

	private void Awake()
	{
		Log = base.Logger;

		var harmony = new Harmony(PluginGuid);
		harmony.PatchAll();
	}

	private bool toggleEncounterMenu = false;

	private void OnGUI()
	{
		if (SceneManager.GetActiveScene().name != "Part1_Cabin") return;

		string[] names = resourceList.Select(res => res.name).ToArray();

		toggleEncounterMenu = GUI.Toggle(
			new Rect(20, 280, 200, 20),
			toggleEncounterMenu,
			"Encounter Menu"
		);

		if (!toggleEncounterMenu) return;

		int selectedButton = GUI.SelectionGrid(
			new Rect(25, 300, 300, 300),
			-1,
			names,
			2
		);

		if (selectedButton >= 0)
		{
			EncounterBlueprintData encounter = resourceList[selectedButton];
			// the asset names have P1 or P2 at the end,
			//	so we'll remove it so that we can correctly get a boss if a boss was selected
			string scrubbedName = encounter.name.Replace("P1", "").Replace("P2", "");

			CardBattleNodeData node = new CardBattleNodeData()
			{
				difficulty = RunState.Run.regionTier * 6 + 3 / 3 - 1, blueprint = resourceList[selectedButton]
			};

			if (Enum.TryParse(scrubbedName, true, out Opponent.Type bossType))
			{
				node = new BossBattleNodeData();
				((BossBattleNodeData)node).specialBattleId =
					BossBattleSequencer.GetSequencerIdForBoss(bossType);
				((BossBattleNodeData)node).bossType = bossType;
			}

			Opponent opponent = Singleton<TurnManager>.Instance.Opponent;
			if (opponent is not null && !Singleton<TurnManager>.Instance.GameIsOver())
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
				() => Singleton<GameFlowManager>.Instance.CanTransitionToFirstPerson(), delegate
				{
					Log.LogDebug($"-> No longer in transitioning state, transitioning to new encounter");
					Singleton<GameFlowManager>.Instance.TransitionToGameState(GameState.CardBattle,
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

// [HarmonyPatch(typeof(DiskCardGame.Part1FinaleSceneSequencer), nameof(DiskCardGame.Part1FinaleSceneSequencer.Start))]
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

[HarmonyPatch]
public class DisableAchievementHandlers
{
	[HarmonyPrefix, HarmonyPatch(typeof(AchievementManager), nameof(AchievementManager.InitializePlatformHandler))]
	public static bool DisableAchievementManager()
	{
		return false;
	}

	[HarmonyPrefix, HarmonyPatch(typeof(GogGalaxyManager), nameof(GogGalaxyManager.Awake))]
	public static bool DisableManagerGog()
	{
		return false;
	}

	[HarmonyPrefix, HarmonyPatch(typeof(SteamManager), nameof(SteamManager.Initialized), MethodType.Getter)]
	public static bool DisableSteamManager()
	{
		return false;
	}
}
