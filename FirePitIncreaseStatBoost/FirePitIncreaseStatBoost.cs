using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx;
using HarmonyLib;
using PeterHan.PLib.Utils;

namespace InscryptionModsFramework
{
	[BepInPlugin("com.bongmaster.firePitIncreaseStatBoost", "Increase Stat Boost", "1.0")]
	[HarmonyPatch(typeof(DiskCardGame.CardStatBoostSequencer), "ApplyModToCard")]
	class FirePitIncreaseStatBoost
	{
		void Awake()
		{
			FileLog.Log("=====================================");
			FileLog.Log($"[{DateTime.Now}] Starting julianperge harmony patch with path {FileLog.logPath}");
			var harmony = new Harmony("com.julianperge.firePitIncreaseStatBoost");
			harmony.PatchAll();
		}

		[HarmonyTranspiler]
		internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			// replace the attack modifier first
			instructions = PPatchTools.ReplaceConstant(instructions, 1, 4, false);
			// now replace the health modifier and return the instructions
			return PPatchTools.ReplaceConstant(instructions, 2, 8, false);
		}
	}
}
