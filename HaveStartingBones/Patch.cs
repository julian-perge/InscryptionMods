using System.Collections.Generic;
using BepInEx;
using DiskCardGame;
using HarmonyLib;

namespace HaveStartingBones
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	public class HarmonyInit : BaseUnityPlugin
	{
		public const string PluginGuid = "com.julianperge";
		private const string PluginName = "haveStartingBones";
		private const string PluginVersion = "1.0";

		public void Awake()
		{
			var harmony = new Harmony(PluginGuid);
			harmony.PatchAll();
		}
	}

	[HarmonyPatch(typeof(DeckInfo), "Boons", MethodType.Getter)]
	public class Patch
	{
		[HarmonyPostfix]
		public static List<BoonData> AddBoons(List<BoonData> __result)
		{
			__result.Add(BoonsUtil.GetData(BoonData.Type.StartingBones));
			__result.Add(BoonsUtil.GetData(BoonData.Type.StartingGoat));
			// __result.Add(BoonsUtil.GetData(BoonData.Type.DoubleDraw));
			return __result;
		}
	}
}
