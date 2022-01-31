using BepInEx;
using HarmonyLib;

namespace FirePitIncreaseStatBoost;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class HarmonyInit : BaseUnityPlugin
{
	private const string PluginGuid = "julianperge.inscryption.act1.firePitIncreaseStatBoost";
	private const string PluginName = "Increase stats gained from the Fire Pit event";
	private const string PluginVersion = "1.2";

	void Awake()
	{
		var harmony = new Harmony(PluginGuid);
		harmony.PatchAll();
	}
}
