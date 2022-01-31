using System;
using System.Collections.Generic;
using DiskCardGame;

namespace SummonerCard;

public static class PrintingCardUtils
{
	public static void PrintList<T>(string title, List<T> items)
	{
		if (items.Count == 0) return;
		Plugin.Log.LogInfo($"{title} [{string.Join(", ", items)}]");
	}

	public static void PrintAllCardInfo(CardTemple temple)
	{
		var cardList = CardLoader.AllData;
		cardList.Sort((info, cardInfo) => String.Compare(info.name, cardInfo.name, StringComparison.Ordinal));

		if ((int)temple != -1)
		{
			cardList = cardList.FindAll(info => info.temple == temple);
		}

		foreach (var info in cardList)
		{
			PrintCardInfo(info);
		}
	}

	public static void PrintCardInfo(CardInfo info)
	{
		Plugin.Log.LogInfo($"===============");
		Plugin.Log.LogInfo($"Name [{info.name}]");
		Plugin.Log.LogInfo($"Displayed Name [{info.displayedName}]");
		PrintList("Abilities", info.Abilities);
		PrintList("Appearance Behavior", info.appearanceBehaviour);
		Plugin.Log.LogInfo($"Attack [{info.baseAttack}] Health [{info.baseHealth}]");
		Plugin.Log.LogInfo($"Bones Cost [{info.bonesCost}]");
		Plugin.Log.LogInfo($"Boon [{info.boon}]");
		Plugin.Log.LogInfo($"Card Complexity [{info.cardComplexity}]");
		Plugin.Log.LogInfo($"Cost [{info.cost}]");
		Plugin.Log.LogInfo($"Description [{info.description}]");
		Plugin.Log.LogInfo($"Energy Cost [{info.energyCost}]");
		if (info.evolveParams is not null)
		{
			Plugin.Log.LogInfo(
				$"EvolveParams = Turns to evolve [{info.evolveParams.turnsToEvolve}] " +
				$"Evolves into [{info.evolveParams.evolution.name}]");
		}
		Plugin.Log.LogInfo($"Gemified? [{info.Gemified}]");
		PrintList("Gems Cost", info.GemsCost);
		if (info.iceCubeParams is not null)
		{
			Plugin.Log.LogInfo($"IceCubeParams = [{info.iceCubeParams.creatureWithin.name}]");
		}
		PrintList("Meta Category", info.metaCategories);
		Plugin.Log.LogInfo($"One per deck? [{info.onePerDeck}]");
		// TODO: possible to get an NRE if the power level is not set?
		Plugin.Log.LogInfo($"Power Level [{info.PowerLevel}]");
		Plugin.Log.LogInfo($"Sacrificable? [{info.Sacrificable}]");
		Plugin.Log.LogInfo($"Special Stat Icon [{info.specialStatIcon}]");
		PrintList("Special Abilities", info.SpecialAbilities);
		Plugin.Log.LogInfo($"Temple [{info.temple}]");
		PrintList("Traits", info.traits);
		PrintList("Tribes", info.tribes);

		// PrintCardModInfoList(info.mods);

		Plugin.Log.LogInfo($"===============\n");
	}
}
