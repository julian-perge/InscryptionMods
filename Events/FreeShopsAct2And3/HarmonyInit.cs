using BepInEx;
using HarmonyLib;

namespace FreeShopsAct2And3;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class HarmonyInit : BaseUnityPlugin
{
	private const string PluginGuid = "julianperge.inscryption.freeShopsAct2And3";
	private const string PluginName = "Free packs/cards in Act2. Free shops in Act 3.";
	private const string PluginVersion = "1.1";

	void Awake()
	{
		var harmony = new Harmony(PluginGuid);
		harmony.PatchAll();
	}
}
