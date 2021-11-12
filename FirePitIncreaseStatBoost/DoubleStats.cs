using System.Collections.Generic;
using HarmonyLib;
using PeterHan.PLib.Utils;

namespace FirePitIncreaseStatBoost
{
	[HarmonyPatch(typeof(DiskCardGame.CardStatBoostSequencer), "ApplyModToCard")]
	public class DoubleStats
	{
		[HarmonyTranspiler]
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			// replace the health modifier first
			instructions = PPatchTools.ReplaceConstant(instructions, 2, 4, false);
			// now replace the attack modifier and return the instructions
			return PPatchTools.ReplaceConstant(instructions, 1, 2, false);
		}
	}
}
