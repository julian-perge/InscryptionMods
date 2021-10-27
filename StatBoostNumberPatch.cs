using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using PeterHan.PLib.Utils;

namespace InscryptionModsFramework
{
	[HarmonyPatch(typeof(DiskCardGame.CardStatBoostSequencer), "ApplyModToCard")]
	class StatBoostNumberPatch
	{
		[HarmonyTranspiler]
		internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			instructions = PPatchTools.ReplaceConstant(instructions, 1, 4, false);

			return PPatchTools.ReplaceConstant(instructions, 2, 8, false);
		}
	}
}
