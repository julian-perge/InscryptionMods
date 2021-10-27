using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx;
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

	[HarmonyPatch(typeof(DiskCardGame.BuyPeltsSequencer), "PeltPrices")]
	public class PeltsPatcher
	{

		[HarmonyPostfix]
		static int[] Postfix(ref int[] __result)
		{
			FileLog.Log($"Returning all zeroes for pelts");
			return new int[] { 0, 0, 0 };
		}

	}
}
