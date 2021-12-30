using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace IncreaseActOneCardSlots.Patches
{
	[HarmonyPatch(typeof(DiskCardGame.GiantAnimatedPortrait), nameof(DiskCardGame.GiantAnimatedPortrait.ApplyAppearance))]
	internal class GiantAnimatedPortraitPatch
	{

		[HarmonyPrefix]
		public static bool ChangeSizeOfPortait(DiskCardGame.AnimatedPortrait __instance)
		{

			__instance.ApplyAppearance();
			Texture2D newText = new Texture2D(2,2);
			newText.LoadImage(File.ReadAllBytes(Directory.GetFiles(Paths.PluginPath, "card_empty_giant_5_slots", SearchOption.AllDirectories)[0]));
			newText.filterMode = FilterMode.Point;

			__instance.Card.RenderInfo.baseTextureOverride = (Texture)newText;
			return false;
		}

	}
}
