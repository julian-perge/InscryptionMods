using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;

namespace Excavator
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
	public class HarmonyInit : BaseUnityPlugin
	{
		public const string PluginGuid = "julian.inscryption.abilities.excavator";
		private const string PluginName = "Custom_Ability_Excavator";
		private const string PluginVersion = "1.1";

		internal static ManualLogSource Log;

		void Awake()
		{
			Log = base.Logger;

			NewAbility ability = NestAbility.InitAbility();
			var abilities = new List<Ability>() { ability.ability };
			new CustomCard("Stinkbug_Talking") { abilities = abilities };

			var harmony = new Harmony(PluginGuid);
			harmony.PatchAll();
		}
	}
}
