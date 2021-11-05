using BepInEx;
using BepInEx.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.IO;
using DiskCardGame;
using HarmonyLib;
using PeterHan.PLib.Utils;
using UnityEngine;

namespace FirePitIncreaseStatBoost
{
	[BepInPlugin("com.bongmaster.firePitIncreaseStatBoost", "Increase Stat Boost", "1.1")]
	public class FirePitIncreaseStatBoost : BaseUnityPlugin
	{
		void Awake()
		{
			// FileLog.Log("=====================================");
			// FileLog.Log($"[{DateTime.Now}] Starting julianperge harmony patch with path {FileLog.logPath}");
			var harmony = new Harmony("com.julianperge.firePitIncreaseStatBoost");
			harmony.PatchAll();
		}

	}

	[HarmonyPatch(typeof(DiskCardGame.CardStatBoostSequencer), "ApplyModToCard")]
	public class StatPatch
	{
		[HarmonyTranspiler]
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			// replace the attack modifier first
			instructions = PPatchTools.ReplaceConstant(instructions, 1, 4, false);
			// now replace the health modifier and return the instructions
			return PPatchTools.ReplaceConstant(instructions, 2, 8, false);
		}
	}
}
