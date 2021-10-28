using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx;
using DiskCardGame;
using HarmonyLib;

namespace PeltsAreAlwaysFree
{

	[BepInPlugin("com.bongmaster.peltsAreAlwaysFree", "Pelts Are Always Free", "1.0")]
	public class PeltsAreAlwaysFree : BaseUnityPlugin
	{
		void Awake()
		{
			FileLog.Log("=====================================");
			FileLog.Log($"[{DateTime.Now}] Starting harmony patch for PeltsAreAlwaysFree with path {FileLog.logPath}");
			var harmony = new Harmony("com.bongmaster.PeltsAreAlwaysFree");
			harmony.PatchAll();
		}
	}

	[HarmonyPatch(typeof(BuyPeltsSequencer), "BuyPelts")]
	public class PeltsPatcher
	{

		[HarmonyPostfix]
		static void Prefix(ref BuyPeltsSequencer __instance)
		{
			string prices = "PeltPrices";
			FileLog.Log($"Returning all zeroes for pelts ");
			int[] allZeros = new int[] { 0, 0, 0 };
			// var prices = AccessTools.Method(__instance.GetType(), "PeltPrices").;
			// prices = allZeros;
			var trav = Traverse.Create(typeof(BuyPeltsSequencer));

			Console.WriteLine($"Field {trav.Field(prices)} Property {trav.Property(prices)}");
			// .SetValue(new int[] { 0, 0, 0 });
			
			// Console.WriteLine($"PeltPrices {method.GetValue()} {method.GetType()}");
			// foreach (var field in method.Fields())
			// {
			// 	Console.WriteLine($"PeltPrices fields {method.Fields()}");
			// }
			// FileLog.Log("" + method.GetValue());
			// ___PeltPrices = allZeros;



			// __instance.PeltPrices = allZeros;
			for (int i = 0; i < __instance.PeltPrices.Length; i++)
			{
				Console.WriteLine($"Pelt price idx {i} Price {__instance.PeltPrices[i]} Getter {__instance.PeltPrices.GetValue(i)}");
				__instance.PeltPrices.SetValue(0, i);
			}
		}

	}
}
