namespace IncreaseActOneCardSlots;

[BepInEx.BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BepInEx.BaseUnityPlugin
{
	public const string PluginGuid = "julianperge.inscryption.act1.increaseCardSlots";
	public const string PluginName = "IncreaseActOneCardSlots";
	private const string PluginVersion = "1.3.0";

	internal static BepInEx.Logging.ManualLogSource Log;

	private void Awake()
	{
		Log = base.Logger;

		var harmony = new HarmonyLib.Harmony(PluginGuid);
		harmony.PatchAll();
	}
}
