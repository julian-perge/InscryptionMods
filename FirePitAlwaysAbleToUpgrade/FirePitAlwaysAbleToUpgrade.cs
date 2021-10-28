using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using BepInEx;
using UnityEngine;
using DiskCardGame;
using HarmonyLib;
using PeterHan.PLib.Utils;

namespace InscryptionModsFramework
{
	// DiskCardGame.CardStatBoostSequencer+<StatBoostSequence>d__12 <-- THIS IS THE ONE

	[BepInPlugin("com.julianperge.alwaysAbleToUpgrade", "Always Able To Upgrade Cards", "1.0")]
	[HarmonyPatch]
	class FirePitAlwaysAbleToUpgrade
	{

		void Awake()
		{
			FileLog.Log("=====================================");
			FileLog.Log($"[{DateTime.Now}] Starting harmony patch with log path {FileLog.logPath}");
			var harmony = new Harmony("com.julianperge.alwaysAbleToUpgrade");
			harmony.PatchAll();
		}

		[HarmonyTargetMethods]
		static IEnumerable<MethodBase> ReturnMoveNextMethodFromNestedEnumerator(Harmony _)
		{
			Type getEnumeratorType = AccessTools.TypeByName("DiskCardGame.CardStatBoostSequencer+<StatBoostSequence>d__12");
			return AccessTools.GetDeclaredMethods(getEnumeratorType).Where(m => m.Name.Equals("MoveNext"));
		}

		[HarmonyTranspiler]
		internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			// instructions that are passed in is the MoveNext method from the StatBoostSequence enumerator
			return PPatchTools.ReplaceConstant(instructions, 0.225f, 0f, false);
		}

	}

}
