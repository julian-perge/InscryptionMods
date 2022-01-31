using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace OpponentsNeverStopPlayingCards;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public partial class Plugin : BaseUnityPlugin
{
	public const string PluginGuid = "julianperge.inscryption.opponentsNeverStopPlayingCards";
	public const string PluginName = "OpponentsNeverStopPlayingCards";
	private const string PluginVersion = "1.0.0";

	internal static ManualLogSource Log;

	private void Awake()
	{
		Log = base.Logger;

		var harmony = new Harmony(PluginGuid);
		harmony.PatchAll();
	}
}

[HarmonyPatch(typeof(DiskCardGame.Opponent), nameof(DiskCardGame.Opponent.QueueNewCards))]
public class OpponentQueueNewCardsPatch
{
	[HarmonyPrefix]
	static void ResetNumberOfTurnsTaken(DiskCardGame.Opponent __instance, bool doTween = true, bool changeView = true)
	{
		// FinaleWizardBattleOpponent has different logic, so we don't want to change it
		if (__instance is DiskCardGame.FinaleWizardBattleOpponent) { return; }

		PropertyInfo f_numTurnsTaken = AccessTools.Property(typeof(DiskCardGame.Opponent), "NumTurnsTaken");
		PropertyInfo f_turnPlan = AccessTools.Property(typeof(DiskCardGame.Opponent), "TurnPlan");

		int numTurnsTaken = (int)f_numTurnsTaken.GetValue(__instance);
		Plugin.Log.LogDebug($"NumTurnsTaken for Opponent is [{numTurnsTaken}]");

		int turnPlanCount = ((List<List<DiskCardGame.CardInfo>>)f_turnPlan.GetValue(__instance)).Count;
		Plugin.Log.LogDebug($"TurnPlanCount for Opponent is [{turnPlanCount}]");

		if (numTurnsTaken >= turnPlanCount)
		{
			Plugin.Log.LogDebug(
				$"NumTurnsTaken [{numTurnsTaken}] is greater than or equal to turn plan count [{turnPlanCount}]. Resetting NumTurnsTaken back to zero");
			f_numTurnsTaken.SetValue(__instance, 0);
			__instance.StartCoroutine(Singleton<DiskCardGame.TextDisplayer>
				.Instance
				.ShowThenClear("Oh? Looks like I need to get some more cards to play...", 3f)
			);
		}
	}
}
