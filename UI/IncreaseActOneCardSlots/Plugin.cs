using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace IncreaseActOneCardSlots
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "julianperge.inscryption.act1.increaseCardSlots";
        public const string PluginName = "IncreaseActOneCardSlots";
        private const string PluginVersion = "1.0.0";

        internal static ManualLogSource Log;

        private void Awake()
        {
            Log = base.Logger;

            var harmony = new Harmony(PluginGuid);
            harmony.PatchAll();
        }
    }
}
