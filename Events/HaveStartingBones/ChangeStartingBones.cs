using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using DiskCardGame;
using HarmonyLib;

namespace HaveStartingBones
{
	[HarmonyPatch(typeof(DeckInfo), "Boons", MethodType.Getter)]
	public class AddStartingBonesPatch
	{
		[HarmonyPostfix]
		public static List<BoonData> AddBoons(List<BoonData> __result)
		{
			__result.Add(BoonsUtil.GetData(BoonData.Type.StartingBones));
			// __result.Add(BoonsUtil.GetData(BoonData.Type.StartingGoat));
			// __result.Add(BoonsUtil.GetData(BoonData.Type.DoubleDraw));
			return __result;
		}
	}

	[HarmonyPatch]
	public class ChangeStartingBones
	{
		[HarmonyTargetMethods]
		static IEnumerable<MethodBase> ReturnMoveNextMethodFromNestedEnumerator(Harmony _)
		{
			// StatBoostSequence is the IEnumerator method, but there's a hidden compiler class, <ActivatePreCombatBoons>d__4,
			//	that actually has all the byte code to look for.

			Type targetType = AccessTools.TypeByName("DiskCardGame.BoonsHandler+<ActivatePreCombatBoons>d__4");
			return AccessTools.GetDeclaredMethods(targetType).Where(m => m.Name.Equals("MoveNext"));
		}

		[HarmonyTranspiler]
		internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			foreach (var codeInstruction in instructions)
			{
				var ins = codeInstruction;
				var opcode = ins.opcode;
				if (opcode == OpCodes.Ldc_I4_8)
				{
					// Console.WriteLine($"Setting starting bones from 8 to 20");
					ins.opcode = OpCodes.Ldc_I4;
					ins.operand = 20;
				}

				yield return ins;
			}
		}
	}
}
