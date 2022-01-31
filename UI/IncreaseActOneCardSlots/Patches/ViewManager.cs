using DiskCardGame;
using HarmonyLib;

namespace IncreaseActOneCardSlots.Patches;

[HarmonyPatch(typeof(DiskCardGame.ViewManager))]
public class ViewManager
{
	[HarmonyPostfix, HarmonyPatch(nameof(DiskCardGame.ViewManager.GetViewInfo))]
	public static void ChangeFieldOfViewAfter(ref ViewInfo __result, ref View view)
	{
		switch (view)
		{
			case View.Board:
			case View.BoardCentered:
				__result.fov = 60f;
				break;
		}
	}
}
