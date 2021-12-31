using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace DebugPatches
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	public class Plugin : BaseUnityPlugin
	{
		public const string PluginGuid = "julianperge.inscryption.debugEnabling";
		public const string PluginName = "EnableCertainDebugging";
		private const string PluginVersion = "0.1.0";

		internal static ManualLogSource Log;

		private void Awake()
		{
			Log = base.Logger;

			var harmony = new Harmony(PluginGuid);
			harmony.PatchAll();
		}
	}

	[HarmonyPatch(typeof(DiskCardGame.Part1FinaleSceneSequencer), nameof(DiskCardGame.Part1FinaleSceneSequencer.Start))]
	public class Part1FinaleSceneSequencerDebugTransitionPatch
	{
		[HarmonyTranspiler]
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			// Plugin.Log.LogDebug($"Setting DebugTransition to true");
			bool InstructionIsFieldInfo(CodeInstruction ins)
				=> ins.opcode == OpCodes.Stfld && ins.operand is FieldInfo { Name: "debugTransition" };

			return new CodeMatcher(instructions)
				.Start()
				.MatchForward(false,
					new CodeMatch(OpCodes.Ldc_I4_0),
					new CodeMatch(InstructionIsFieldInfo)
				)
				.SetOpcodeAndAdvance(OpCodes.Ldc_I4_1)
				.InstructionEnumeration();
		}
	}
}
