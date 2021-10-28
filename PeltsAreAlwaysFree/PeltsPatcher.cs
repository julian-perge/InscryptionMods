using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx;
using DiskCardGame;
using HarmonyLib;

namespace PeltsAreAlwaysFree
{

	[BepInPlugin("com.julianperge.peltsAreAlwaysFree", "Pelts Are Always Free", "1.0")]
	public class PeltsAreAlwaysFree : BaseUnityPlugin
	{
		void Awake()
		{
			// FileLog.Log("=====================================");
			// FileLog.Log($"[{DateTime.Now}] Starting harmony patch for PeltsAreAlwaysFree with log path {FileLog.logPath}");
			var harmony = new Harmony("com.julianperge.PeltsAreAlwaysFree");
			harmony.PatchAll();
		}
	}

	// PeltPrices is a Property with a custom getter, which means we have to specify the MethodType in order to access it
	[HarmonyPatch(typeof(BuyPeltsSequencer), "PeltPrices", MethodType.Getter)]
	public class PeltsPatcher
	{

		[HarmonyPrefix]
		static bool Prefix(ref int[] __result)
		{
			__result = new int[] { 0, 0, 0 };
			return false;
		}

	}
}
