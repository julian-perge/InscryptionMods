using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace IncreaseActOneCardSlots.Patches
{
	[HarmonyPatch]
	public class PickAxeSlamPatch
	{
		[HarmonyTargetMethods]
		static IEnumerable<MethodBase> ReturnMoveNextMethodFromNestedEnumerator(Harmony _)
		{
			// StrikeCardSlot is the IEnumerator method, but there's a hidden compiler class, <StrikeCardSlot>d__2,
			//	that actually has all the byte code to look for.
			Type getEnumeratorType = AccessTools.TypeByName("DiskCardGame.PickAxeSlam+<StrikeCardSlot>d__2");
			return AccessTools.GetDeclaredMethods(getEnumeratorType).Where(m => m.Name.Equals("MoveNext"));
		}

		[HarmonyTranspiler]
		internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			return new CodeMatcher(instructions)
				.Start()
				// this is the OpCode before the integer for 3 is used
				// the reason we go to this one is that there is another ldc_i4_3 later in the instructions that we do not want to modify
				.MatchForward(false, new CodeMatch(OpCodes.Ldloca_S))
				// go to the next instruction, which is the ldc_i4_3 OpCode
				.Advance(1)
				// set the 3 to a 4
				.Set(OpCodes.Ldc_I4_4, null)
				.InstructionEnumeration();
		}
	}
}
