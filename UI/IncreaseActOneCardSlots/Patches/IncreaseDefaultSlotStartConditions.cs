using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;

namespace IncreaseActOneCardSlots.Patches;

[HarmonyPatch(typeof(DiskCardGame.EncounterData.StartCondition), MethodType.Constructor)]
public class IncreaseDefaultSlotStartConditions
{
	[HarmonyTranspiler]
	static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
	{
		Plugin.Log.LogDebug($"Setting EncounterData.StartCondition constructor arrays from 4 to 5");
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
