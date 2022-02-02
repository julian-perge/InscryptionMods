using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
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

[HarmonyPatch(typeof(Opponent), nameof(Opponent.QueueNewCards))]
public class OpponentQueueNewCardsPatch
{
	[HarmonyPrefix]
	private static void ResetNumberOfTurnsTaken(Opponent __instance, bool doTween = true, bool changeView = true)
	{
		// FinaleWizardBattleOpponent has different logic, so we don't want to change it
		if (__instance is FinaleWizardBattleOpponent) { return; }

		PropertyInfo f_numTurnsTaken = AccessTools.Property(typeof(Opponent), "NumTurnsTaken");
		PropertyInfo f_turnPlan = AccessTools.Property(typeof(Opponent), "TurnPlan");

		int numTurnsTaken = (int)f_numTurnsTaken.GetValue(__instance);
		Plugin.Log.LogDebug($"NumTurnsTaken for Opponent is [{numTurnsTaken}]");

		int turnPlanCount = ((List<List<CardInfo>>)f_turnPlan.GetValue(__instance)).Count;
		Plugin.Log.LogDebug($"TurnPlanCount for Opponent is [{turnPlanCount}]");

		if (numTurnsTaken >= turnPlanCount)
		{
			Plugin.Log.LogDebug(
				$"NumTurnsTaken [{numTurnsTaken}] is greater than or equal to turn plan count [{turnPlanCount}]. Resetting NumTurnsTaken back to zero");
			f_numTurnsTaken.SetValue(__instance, 0);
			__instance.StartCoroutine(
				TextDisplayer.Instance
					.ShowThenClear("Oh? Looks like I need to get some more cards to play...", 3f)
			);
		}
	}
}
