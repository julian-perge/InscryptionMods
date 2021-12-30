using BepInEx;
using HarmonyLib;

namespace Exodia
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency(CyantistInscryptionAPI, BepInDependency.DependencyFlags.HardDependency)]
	[BepInDependency(SigilADay_julianPerge, BepInDependency.DependencyFlags.HardDependency)]
	public partial class Plugin : BaseUnityPlugin
	{
		public const string CyantistInscryptionAPI = "cyantist.inscryption.api";
		public const string SigilADay_julianPerge = "julianperge.inscryption.sigiladay";

		public const string PluginGuid = "julianperge.inscryption.cards.exodia";
		private const string PluginName = "Exodia_Cards";
		private const string PluginVersion = "1.3";

		private void Awake()
		{
			AddExodiaCards();

			var harmony = new Harmony(PluginGuid);
			harmony.PatchAll();
		}
	}
}
