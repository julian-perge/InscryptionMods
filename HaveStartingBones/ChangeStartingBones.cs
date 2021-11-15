using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using PeterHan.PLib.Utils;

namespace HaveStartingBones
{
	[HarmonyPatch]
	public class ChangeStartingBones
	{
		[HarmonyTargetMethods]
		static IEnumerable<MethodBase> ReturnMoveNextMethodFromNestedEnumerator(Harmony _)
		{
			// StatBoostSequence is the IEnumerator method, but there's a hidden compiler class, <ActivatePreCombatBoons>d__4,
			//	that actually has all the byte code to look for.
			Type getEnumeratorType = AccessTools.TypeByName("DiskCardGame.BoonsHandler+<ActivatePreCombatBoons>d__4");
			return AccessTools.GetDeclaredMethods(getEnumeratorType).Where(m => m.Name.Equals("MoveNext"));
		}

		[HarmonyTranspiler]
		internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			return PPatchTools.ReplaceConstant(instructions, 8, 20, false);
		}
	}
}
