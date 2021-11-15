using BepInEx;

namespace Exodia
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
	public class HarmonyInit : BaseUnityPlugin
	{
		public const string PluginGuid = "com.julianperge";
		private const string PluginName = "exodia";
		private const string PluginVersion = "1.2";

		private void Awake()
		{
			Exodia.Card.InitCards();
		}
	}
}
