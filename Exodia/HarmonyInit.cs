using System.Collections.Generic;
using System.IO;
using APIPlugin;
using BepInEx;
using DiskCardGame;
using UnityEngine;

namespace Exodia
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
	public class HarmonyInit : BaseUnityPlugin
	{
		private const string PluginGuid = "com.julianperge";
		private const string PluginName = "exodia";
		private const string PluginVersion = "1.1";

		private void Awake()
		{
			ExodiaAbility.AddExodiaAbility();
		}
	}
}
