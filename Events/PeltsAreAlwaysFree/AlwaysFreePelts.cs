namespace PeltsAreAlwaysFree
{
	// PeltPrices is a Property with a custom getter, which means we have to specify the MethodType in order to access it
	[HarmonyLib.HarmonyPatch(typeof(DiskCardGame.BuyPeltsSequencer), nameof(DiskCardGame.BuyPeltsSequencer.PeltPrices),
		HarmonyLib.MethodType.Getter)]
	public class AlwaysFreePelts
	{
		[HarmonyLib.HarmonyPrefix]
		// ref int[] __result means the result that is returned from the call.
		static bool Prefix(ref int[] __result)
		{
			__result = new[] { 0, 0, 0 };
			return false; // skips the rest of the real code
		}
	}
}
