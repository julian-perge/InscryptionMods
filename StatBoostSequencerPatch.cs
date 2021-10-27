using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using DiskCardGame;
using HarmonyLib;
using PeterHan.PLib.Utils;

namespace InscryptionModsFramework
{
	// DiskCardGame.StatBoostSequencerPatch+<StatBoostSequence>d__12 <-- THIS IS THE ONE

	[HarmonyPatch]
	class StatBoostSequencerPatch
	{

		[HarmonyTargetMethods]
		static IEnumerable<MethodBase> ReturnMoveNextMethodFromNestedEnumerator(Harmony _)
		{
			Type targetType = AccessTools.TypeByName("DiskCardGame.CardStatBoostSequencer+<StatBoostSequence>d__12");
			return AccessTools.GetDeclaredMethods(targetType).Where(m => m.Name.Equals("MoveNext"));
		}

		[HarmonyTranspiler]
		internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			return PPatchTools.ReplaceConstant(instructions, 0.225f, 0f, false);
		}

	}

}
