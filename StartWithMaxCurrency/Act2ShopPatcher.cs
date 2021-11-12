using GBC;
using HarmonyLib;

namespace StartWithMaxCurrency
{
	[HarmonyPatch(typeof(ShopUIPricetag), nameof(ShopUIPricetag.SetPrice))]
	public class Act2ShopPatcher
	{

		/// <summary>
		/// Set Act 2 (GBC theme) cards and packs to zero dollars. 
		/// </summary>
		/// <param name="price"></param>
		/// <returns></returns>
		[HarmonyPrefix]
		static bool Prefix(ref int price)
		{
			price = 0;
			return false;
		}

	}
}
