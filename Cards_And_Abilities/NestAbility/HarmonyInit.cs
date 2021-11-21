using System.Collections.Generic;
using APIPlugin;
using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;

namespace NestCard
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
	public class HarmonyInit : BaseUnityPlugin
	{
		public const string PluginGuid = "julian.inscryption.abilities.nest";
		private const string PluginName = "Custom Nest Ability";
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
