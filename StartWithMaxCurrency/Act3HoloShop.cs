using DiskCardGame;
using HarmonyLib;

namespace StartWithMaxCurrency
{
	/// <summary>
	/// Set all nodes in Act 3 (Hologram theme) to always be zero dollars
	/// </summary>
	[HarmonyPatch(typeof(HoloMapShopNode), "TotalCost", MethodType.Getter)]
	public class Act3HoloShopPatch
	{
		[HarmonyPrefix]
		static bool Prefix(ref int __result, ref int ___cost, ref bool ___increasingCost)
		{
			__result = 0;
			return false;
		}

	}
}
