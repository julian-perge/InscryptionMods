namespace IncreaseActOneCardSlots.Patches
{
	[HarmonyLib.HarmonyPatch(typeof(DiskCardGame.EncounterData.StartCondition), HarmonyLib.MethodType.Constructor)]
	public class IncreaseDefaultSlotStartConditions
	{
		[HarmonyLib.HarmonyTranspiler]
		static System.Collections.Generic.IEnumerable<HarmonyLib.CodeInstruction> Transpiler(
			System.Collections.Generic.IEnumerable<HarmonyLib.CodeInstruction> instructions)
		{
			Plugin.Log.LogDebug($"Setting EncounterData.StartCondition constructor arrays from 4 to 5");
			foreach (HarmonyLib.CodeInstruction instruction in instructions)
			{
				if (instruction.opcode == System.Reflection.Emit.OpCodes.Ldc_I4_4)
				{
					instruction.opcode = System.Reflection.Emit.OpCodes.Ldc_I4_5;
				}

				yield return instruction;
			}
		}
	}
}
