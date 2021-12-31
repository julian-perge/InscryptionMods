namespace FirePitIncreaseStatBoost
{
	[HarmonyLib.HarmonyPatch(typeof(DiskCardGame.CardStatBoostSequencer),
		nameof(DiskCardGame.CardStatBoostSequencer.ApplyModToCard))]
	public class DoubleStats
	{
		[HarmonyLib.HarmonyTranspiler]
		static System.Collections.Generic.IEnumerable<HarmonyLib.CodeInstruction> Transpiler(
			System.Collections.Generic.IEnumerable<HarmonyLib.CodeInstruction> instructions)
		{
			foreach (var codeInstruction in instructions)
			{
				var opcode = codeInstruction.opcode;
				if (opcode == System.Reflection.Emit.OpCodes.Ldc_I4_1)
				{
					// replace the health modifier first
					codeInstruction.opcode = System.Reflection.Emit.OpCodes.Ldc_I4_2;
				}
				else if (opcode == System.Reflection.Emit.OpCodes.Ldc_I4_2)
				{
					// now replace the attack modifier
					codeInstruction.opcode = System.Reflection.Emit.OpCodes.Ldc_I4_4;
				}

				yield return codeInstruction;
			}
		}
	}
}
