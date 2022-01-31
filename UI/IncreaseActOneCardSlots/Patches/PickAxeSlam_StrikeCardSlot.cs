using System.Linq;
using HarmonyLib;

namespace IncreaseActOneCardSlots.Patches;

[HarmonyPatch]
public class PickAxeSlam_StrikeCardSlot
{
	[HarmonyTargetMethods]
	static System.Collections.Generic.IEnumerable<System.Reflection.MethodBase>
		ReturnMoveNextMethodFromNestedEnumerator(Harmony _)
	{
		// StrikeCardSlot is the IEnumerator method, but there's a hidden compiler class, <StrikeCardSlot>d__2,
		//	that actually has all the byte code to look for.
		System.Type getEnumeratorType = AccessTools.TypeByName("DiskCardGame.PickAxeSlam+<StrikeCardSlot>d__2");
		Plugin.Log.LogDebug($"Enumerator type in pick axe slam [{getEnumeratorType}]");
		return AccessTools.GetDeclaredMethods(getEnumeratorType).Where(m => m.Name == ("MoveNext"));
	}

	[HarmonyTranspiler]
	internal static System.Collections.Generic.IEnumerable<CodeInstruction> Transpiler(
		System.Collections.Generic.IEnumerable<CodeInstruction> instructions
	)
	{
		Plugin.Log.LogDebug($"Inside transpiler for pick axe slam");
		return new CodeMatcher(instructions)
			.Start()
			// this is the OpCode before the integer for 3 is used
			// the reason we go to this one is that there is another ldc_i4_3 later in the instructions that we do not want to modify
			.MatchForward(false,
				new CodeMatch(System.Reflection.Emit.OpCodes.Ldc_I4_3)
			)
			// set the 3 to a 4
			.SetOpcodeAndAdvance(System.Reflection.Emit.OpCodes.Ldc_I4_4)
			// now look for the -1.4 float number to change where the pick axe strikes the center of the card
			.MatchForward(false, new CodeMatch(System.Reflection.Emit.OpCodes.Ldc_R4, -1.4f))
			// change to -1.275
			.SetOperandAndAdvance(-1.275f)
			.InstructionEnumeration();
	}
}
