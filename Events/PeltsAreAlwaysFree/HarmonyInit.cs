using BepInEx;
using HarmonyLib;

namespace PeltsAreAlwaysFree;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class HarmonyInit : BaseUnityPlugin
{
	private const string PluginGuid = "julianperge.inscryption.act1.peltsAreAlwaysFree";
	private const string PluginName = "Pelts from the trader are always free.";
	private const string PluginVersion = "1.1";

	void Awake()
	{
		var harmony = new Harmony(PluginGuid);
		harmony.PatchAll();
	}
}
