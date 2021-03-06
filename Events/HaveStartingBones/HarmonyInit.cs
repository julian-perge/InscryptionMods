using BepInEx;
using HarmonyLib;

namespace HaveStartingBones;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class HarmonyInit : BaseUnityPlugin
{
	public const string PluginGuid = "julianperge.inscryption.act1.addBoons";
	private const string PluginName = "AddBoneBoonAtStart";
	private const string PluginVersion = "1.3";

	public void Awake()
	{
		var harmony = new Harmony(PluginGuid);
		harmony.PatchAll();
	}
}
