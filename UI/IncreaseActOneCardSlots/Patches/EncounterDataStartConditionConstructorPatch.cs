using System.Collections.Generic;
using System.Reflection.Emit;
using DiskCardGame;
using HarmonyLib;

namespace IncreaseActOneCardSlots.Patches
{
	[HarmonyPatch(typeof(EncounterData.StartCondition), MethodType.Constructor)]
	public class EncounterDataStartConditionConstructorPatch
	{
		[HarmonyTranspiler]
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			foreach (CodeInstruction instruction in instructions)
			{
				if (instruction.opcode == OpCodes.Ldc_I4_4)
				{
					instruction.opcode = OpCodes.Ldc_I4_5;
				}
				yield return instruction;
			}
		}
	}
}
