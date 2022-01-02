using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace FirePitAlwaysAbleToUpgrade
{
	[HarmonyPatch]
	public class AlwaysAbleToUpgrade
	{
		[HarmonyTargetMethods]
		static IEnumerable<MethodBase> ReturnMoveNextMethodFromNestedEnumerator(Harmony _)
		{
			// StatBoostSequence is the IEnumerator method, but there's a hidden compiler class, <StatBoostSequence>d__12,
			//	that actually has all the byte code to look for.
			Type getEnumeratorType = AccessTools.TypeByName("DiskCardGame.CardStatBoostSequencer+<StatBoostSequence>d__12");
			return AccessTools.GetDeclaredMethods(getEnumeratorType).Where(m => m.Name.Equals("MoveNext"));
		}

		[HarmonyTranspiler]
		internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			// instructions that are passed in is the MoveNext method from the StatBoostSequence enumerator
			foreach (var codeInstruction in instructions)
			{
				var opcode = codeInstruction.opcode;
				var operand = codeInstruction.operand;
				if (opcode == OpCodes.Ldc_R4)
				{
					if (operand.Equals(0.225f))
					{
						codeInstruction.operand = 0f;
					}
					else if (operand.Equals(0.5f))
					{
						codeInstruction.operand = 1f;
					}
				}

				yield return codeInstruction;
			}
			// kept here for future reference for a possible Inscryption specific API for replacing constants/methods
			// return PPatchTools.ReplaceConstant(instructions, 0.225f, 0f, false);
		}
	}
}
