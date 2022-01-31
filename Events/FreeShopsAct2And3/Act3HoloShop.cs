namespace FreeShopsAct2And3;

/// <summary>
/// Set all nodes in Act 3 (Hologram theme) to always be zero dollars
/// </summary>
[HarmonyLib.HarmonyPatch(typeof(DiskCardGame.HoloMapShopNode), "TotalCost", HarmonyLib.MethodType.Getter)]
public class Act3HoloShopPatch
{
	[HarmonyLib.HarmonyPrefix]
	static bool Prefix(ref int __result, ref int ___cost, ref bool ___increasingCost)
	{
		__result = 0;
		return false;
	}
}
