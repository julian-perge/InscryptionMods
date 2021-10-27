using BepInEx;
using HarmonyLib;
using System;

namespace InscryptionModsFramework
{
	[BepInPlugin("com.bongmaster.alwaysAbleToUpgrade", "Always Able To Update Cards", "1.0")]
	public class FreePelts : BaseUnityPlugin
	{
		void Awake()
		{
			FileLog.Log("=====================================");
			FileLog.Log($"[{DateTime.Now}] Starting BongMaster harmony patch with path {FileLog.logPath}");
			var harmony = new Harmony("com.bongmaster.alwaysAbleToUpgrade");
			harmony.PatchAll();
		}

	}
}
