using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;

namespace HealthForAnts;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
[BepInDependency(InscryptionAPI.InscryptionAPIPlugin.ModGUID)]
public class HealthForAntsPlugin : BaseUnityPlugin
{
	public const string PluginGuid = "julianperge.inscryption.specialAbilities.healthForAnts";
	private const string PluginName = "HealthForAnts";
	private const string PluginVersion = "1.0.2";
	internal static ManualLogSource Log;

	public static SpecialTriggeredAbility AntHealthSpecialTriggeredAbility;
	public static SpecialStatIcon AntHealthSpecialStatIcon;

	void Awake()
	{
		Log = base.Logger;

		HealthForAnts.InitStatIconAndAbility();
		AntHealthSpecialTriggeredAbility = HealthForAnts.FullSpecial.Id;
		AntHealthSpecialStatIcon = HealthForAnts.FullStatIcon.Id;

		Harmony harmony = new Harmony(PluginGuid);
		harmony.PatchAll();
	}
}
