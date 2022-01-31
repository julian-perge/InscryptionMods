using DiskCardGame;
using HarmonyLib;

namespace IncreaseActOneCardSlots.Patches;

[HarmonyPatch(typeof(TradeCardsForPelts))]
public class TradeForPeltsPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(TradeCardsForPelts.TradePhase))]
	public static void PrefixChangeDefaultValues(
		ref int numQueueCards,
		ref int numOpponentSlotCards,
		int queueCostTier = 3,
		int opponentSlotCostTier = 2,
		string preTradeDialogueId = "TrapperTraderPreTrade",
		string postTradeDialogueId = "TrapperTraderPostTrade")
	{
		if (numQueueCards == 4)
		{
			numQueueCards = 5;
		}

		if (numOpponentSlotCards == 4)
		{
			numOpponentSlotCards = 5;
		}
	}
}
