using System.Collections.Generic;
using System.Reflection.Emit;
using DiskCardGame;
using HarmonyLib;

namespace FirePitIncreaseStatBoost
{
	[HarmonyPatch(typeof(CardStatBoostSequencer), "ApplyModToCard")]
	public class DoubleStats
	{
		[HarmonyTranspiler]
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			foreach (var codeInstruction in instructions)
			{
				var opcode = codeInstruction.opcode;
				if (opcode == OpCodes.Ldc_I4_1)
				{
					// replace the health modifier first
					codeInstruction.opcode = OpCodes.Ldc_I4_2;
				}
				else if (opcode == OpCodes.Ldc_I4_2)
				{
					// now replace the attack modifier
					codeInstruction.opcode = OpCodes.Ldc_I4_4;
				}

				yield return codeInstruction;
			}
		}
	}
}
