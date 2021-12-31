using Enumerable = System.Linq.Enumerable;

namespace IncreaseActOneCardSlots.Patches
{
	[HarmonyLib.HarmonyPatch]
	public class PickAxeSlam
	{
		[HarmonyLib.HarmonyTargetMethods]
		static System.Collections.Generic.IEnumerable<System.Reflection.MethodBase>
			ReturnMoveNextMethodFromNestedEnumerator(HarmonyLib.Harmony _)
		{
			// StrikeCardSlot is the IEnumerator method, but there's a hidden compiler class, <StrikeCardSlot>d__2,
			//	that actually has all the byte code to look for.
			System.Type getEnumeratorType =
				HarmonyLib.AccessTools.TypeByName("DiskCardGame.PickAxeSlam+<StrikeCardSlot>d__2");
			return Enumerable.Where(HarmonyLib.AccessTools.GetDeclaredMethods(getEnumeratorType),
				m => m.Name.Equals("MoveNext"));
		}

		[HarmonyLib.HarmonyTranspiler]
		internal static System.Collections.Generic.IEnumerable<HarmonyLib.CodeInstruction> Transpiler(
			System.Collections.Generic.IEnumerable<HarmonyLib.CodeInstruction> instructions)
		{
			return new HarmonyLib.CodeMatcher(instructions)
				.Start()
				// this is the OpCode before the integer for 3 is used
				// the reason we go to this one is that there is another ldc_i4_3 later in the instructions that we do not want to modify
				.MatchForward(true,
					new HarmonyLib.CodeMatch(System.Reflection.Emit.OpCodes.Ldloca_S),
					new HarmonyLib.CodeMatch(System.Reflection.Emit.OpCodes.Ldc_I4_3)
				)
				// set the 3 to a 4
				.Set(System.Reflection.Emit.OpCodes.Ldc_I4_4, null)
				// now look for the -1.4 float number to change where the pick axe strikes the center of the card
				.MatchForward(false, new HarmonyLib.CodeMatch(System.Reflection.Emit.OpCodes.Ldc_R4, -1.4f))
				// change to -1.275
				.SetOperandAndAdvance(-1.275f)
				.InstructionEnumeration();
		}
	}
}
