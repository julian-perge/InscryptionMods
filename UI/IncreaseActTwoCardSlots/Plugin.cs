namespace IncreaseActTwoCardSlots;

[BepInEx.BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BepInEx.BaseUnityPlugin
{
	public const string PluginGuid = "julianperge.inscryption.act2.increaseCardSlots";
	public const string PluginName = "IncreaseActTwoCardSlots";
	private const string PluginVersion = "0.1.0";

	internal static BepInEx.Logging.ManualLogSource Log;

	private void Awake()
	{
		Log = base.Logger;

		var harmony = new HarmonyLib.Harmony(PluginGuid);
		harmony.PatchAll();
	}
}
