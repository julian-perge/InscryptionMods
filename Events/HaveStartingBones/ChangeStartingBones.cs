using System.Linq;

namespace HaveStartingBones;

[HarmonyLib.HarmonyPatch(typeof(DiskCardGame.DeckInfo), "Boons", HarmonyLib.MethodType.Getter)]
public class AddStartingBonesPatch
{
	[HarmonyLib.HarmonyPostfix]
	public static System.Collections.Generic.List<DiskCardGame.BoonData> AddBoons(
		System.Collections.Generic.List<DiskCardGame.BoonData> __result)
	{
		__result.Add(DiskCardGame.BoonsUtil.GetData(DiskCardGame.BoonData.Type.StartingBones));
		// __result.Add(BoonsUtil.GetData(BoonData.Type.StartingGoat));
		// __result.Add(BoonsUtil.GetData(BoonData.Type.DoubleDraw));
		return __result;
	}
}

[HarmonyLib.HarmonyPatch]
public class ChangeStartingBones
{
	[HarmonyLib.HarmonyTargetMethods]
	static System.Collections.Generic.IEnumerable<System.Reflection.MethodBase>
		ReturnMoveNextMethodFromNestedEnumerator(HarmonyLib.Harmony _)
	{
		// StatBoostSequence is the IEnumerator method, but there's a hidden compiler class, <ActivatePreCombatBoons>d__4,
		//	that actually has all the byte code to look for.

		System.Type targetType =
			HarmonyLib.AccessTools.TypeByName("DiskCardGame.BoonsHandler+<ActivatePreCombatBoons>d__4");
		return Enumerable.Where(HarmonyLib.AccessTools.GetDeclaredMethods(targetType), m => m.Name.Equals("MoveNext"));
	}

	[HarmonyLib.HarmonyTranspiler]
	internal static System.Collections.Generic.IEnumerable<HarmonyLib.CodeInstruction> Transpiler(
		System.Collections.Generic.IEnumerable<HarmonyLib.CodeInstruction> instructions)
	{
		foreach (var codeInstruction in instructions)
		{
			var ins = codeInstruction;
			var opcode = ins.opcode;
			if (opcode == System.Reflection.Emit.OpCodes.Ldc_I4_8)
			{
				// Console.WriteLine($"Setting starting bones from 8 to 20");
				ins.opcode = System.Reflection.Emit.OpCodes.Ldc_I4;
				ins.operand = 20;
			}

			yield return ins;
		}
	}
}
