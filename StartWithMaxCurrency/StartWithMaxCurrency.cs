using System;
using BepInEx;
using DiskCardGame;
using GBC;
using HarmonyLib;

namespace StartWithMaxCurrency
{
	[BepInPlugin("com.julianperge.startWithMaxCurrency", "Start With Max Currency", "1.0")]
	public class Init : BaseUnityPlugin
	{
		void Awake()
		{
			// FileLog.Log("=====================================");
			// FileLog.Log($"[{DateTime.Now}] Starting harmony patch for PeltsAreAlwaysFree with log path {FileLog.logPath}");
			var harmony = new Harmony("com.julianperge.startWithMaxCurrency");
			harmony.PatchAll();
		}
	}

	[HarmonyPatch(typeof(ShopUIPricetag), nameof(ShopUIPricetag.SetPrice))]
	public class ShopPatcher
	{

		[HarmonyPrefix]
		static bool Prefix(ref int price)
		{
			price = 0;
			Console.WriteLine("Set price to zero");
			return false;
		}

	}

	[HarmonyPatch(typeof(HoloMapShopNode), "TotalCost", MethodType.Getter)]
	public class HoloShopPatch
	{
		[HarmonyPrefix]
		static bool Prefix(ref int __result, ref int ___cost, ref bool ___increasingCost)
		{
			Console.WriteLine($"Result is {__result} Cost {___cost} IncreasingCost {___increasingCost} Currency {Part3SaveData.Data.currency}");
			__result = 0;
			return false;
		}

	}
	
}
