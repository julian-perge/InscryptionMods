using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using PeterHan.PLib.Utils;

namespace FirePitAlwaysAbleToUpgrade
{
	// DiskCardGame.CardStatBoostSequencer+<StatBoostSequence>d__12 <-- THIS IS THE ONE

	[BepInPlugin("com.julianperge.alwaysAbleToUpgrade", "Always Able To Upgrade Cards", "1.0")]
	public class FirePitAlwaysAbleToUpgrade : BaseUnityPlugin
	{
		void Awake()
		{
			// FileLog.Log("=====================================");
			// FileLog.Log($"[{DateTime.Now}] Starting harmony patch with log path {FileLog.logPath}");
			var harmony = new Harmony("com.julianperge.alwaysAbleToUpgrade");
			harmony.PatchAll();
		}
	}

	[HarmonyPatch]
	public class Patch
	{
		[HarmonyTargetMethods]
		static IEnumerable<MethodBase> ReturnMoveNextMethodFromNestedEnumerator(Harmony _)
		{
			// StatBoostSequence is the IEnumerator method, but there's a hidden compiler class, <StatBoostSequence>d__12,
			//	that actually has all the byte code to look for.
			Type getEnumeratorType = AccessTools.TypeByName("DiskCardGame.CardStatBoostSequencer+<StatBoostSequence>d__12");
			return AccessTools.GetDeclaredMethods(getEnumeratorType).Where(m => m.Name.Equals("MoveNext"));
		}

		[HarmonyTranspiler]
		internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			// instructions that are passed in is the MoveNext method from the StatBoostSequence enumerator
			return PPatchTools.ReplaceConstant(instructions, 0.225f, 0f, false);
		}
	}

}
