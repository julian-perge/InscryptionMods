using APIPlugin;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace HealthForAnts;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
[BepInDependency("cyantist.inscryption.api")]
public class HarmonyInit : BaseUnityPlugin
{
	public const string PluginGuid = "julianperge.inscryption.specialAbilities.healthForAnts";
	private const string PluginName = "HealthForAnts";
	private const string PluginVersion = "1.1";
	internal static ManualLogSource Log;
	public static NewSpecialAbility antHealthSpecialAbility;

	void Awake()
	{
		Log = base.Logger;

		var newSpecialAbility = HealthForAnts.InitStatIconAndAbility();
		antHealthSpecialAbility = newSpecialAbility;

		Harmony harmony = new Harmony(PluginGuid);
		harmony.PatchAll();
	}
}
