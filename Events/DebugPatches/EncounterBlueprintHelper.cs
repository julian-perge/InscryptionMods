using DiskCardGame;
using UnityEngine;
using UnityEngine.SceneManagement;
using static DebugPatches.Plugin;

namespace DebugPatches;

public class EncounterBlueprintHelper : ManagedBehaviour
{
	private static readonly EncounterBlueprintData[] Part1EncounterBlueprints = Resources
		.LoadAll<EncounterBlueprintData>("Data/EncounterBlueprints/Part1");

	private static readonly string[] Part1EncounterNames = Part1EncounterBlueprints
		.Select(res => res.name.Replace("P1", "").Replace("P2", ""))
		.ToArray();

	private static readonly EncounterBlueprintData[] Part2EncounterBlueprints = Resources
		.LoadAll<EncounterBlueprintData>("Data/EncounterBlueprints/Part2");

	private static readonly string[] Part2EncounterNames = Part2EncounterBlueprints
		.Select(res => res.name.Replace("P1", "").Replace("P2", ""))
		.ToArray();

	private static readonly EncounterBlueprintData[] Part3EncounterBlueprints = Resources
		.LoadAll<EncounterBlueprintData>("Data/EncounterBlueprints/Part3");

	private static readonly string[] Part3EncounterNames = Part3EncounterBlueprints
		.Select(res => res.name.Replace("_P1", "").Replace("_P2", ""))
		.ToArray();

	private bool toggleEncounterMenu;


	private EncounterBlueprintData[] activeEncounterBlueprints;
	private string[] activeEncounterNames;

	private void OnGUI()
	{
		toggleEncounterMenu = GUI.Toggle(
			new Rect(20, 280, 200, 20),
			toggleEncounterMenu,
			"Encounter Menu"
		);

		if (toggleEncounterMenu)
		{
			if (SaveManager.SaveFile.IsPart1)
			{
				activeEncounterBlueprints = Part1EncounterBlueprints;
				activeEncounterNames = Part1EncounterNames;
			}
			else if (SaveManager.SaveFile.IsPart2)
			{
				activeEncounterBlueprints = Part2EncounterBlueprints;
				activeEncounterNames = Part2EncounterNames;
			}
			else if (SaveManager.SaveFile.IsPart3)
			{
				activeEncounterBlueprints = Part3EncounterBlueprints;
				activeEncounterNames = Part3EncounterNames;
			}

			int selectedButton = GUI.SelectionGrid(
				new Rect(25, 300, 300, 300),
				-1,
				activeEncounterNames,
				2
			);

			if (selectedButton >= 0)
			{
				EncounterBlueprintData encounter = activeEncounterBlueprints[selectedButton];
				// the asset names have P1 or P2 at the end,
				//	so we'll remove it so that we can correctly get a boss if a boss was selected
				string scrubbedName = encounter.name.Replace("P1", "").Replace("P2", "");

				CardBattleNodeData node = new CardBattleNodeData
				{
					difficulty = RunState.Run.regionTier * 6 + 3 / 3 - 1, blueprint = activeEncounterBlueprints[selectedButton]
				};

				if (Enum.TryParse(scrubbedName, true, out Opponent.Type bossType))
				{
					node = new BossBattleNodeData();
					((BossBattleNodeData)node).specialBattleId = BossBattleSequencer.GetSequencerIdForBoss(bossType);
					((BossBattleNodeData)node).bossType = bossType;
				}

				Opponent opponent = TurnManager.Instance.Opponent;
				if (opponent && !TurnManager.Instance.GameIsOver())
				{
					Log.LogDebug($"Setting NumLives to 1");
					opponent.NumLives = 1;
					Log.LogDebug($"Game is not over, making opponent surrender");
					opponent.SurrenderImmediate();
					Log.LogDebug($"Playing LifeLostSequence for [{opponent.GetType()}]");
					base.StartCoroutine(opponent.LifeLostSequence());
				}

				Log.LogDebug($"Transitioning to encounter [{activeEncounterBlueprints[selectedButton].name}]");
				CustomCoroutine.WaitOnConditionThenExecute(
					() => GameFlowManager.Instance.CanTransitionToFirstPerson(), delegate
					{
						Log.LogDebug($"-> No longer in transitioning state, transitioning to new encounter");
						GameFlowManager.Instance.TransitionToGameState(GameState.CardBattle, node);
					});
			}
		}
	}
}
