using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;

namespace SummonerCard
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
	[BepInDependency("julianperge.inscryption.sigiladay", BepInDependency.DependencyFlags.HardDependency)]
	public class HarmonyInit : BaseUnityPlugin
	{
		public const string PluginGuid = "julian.inscryption.cards.testing";
		private const string PluginName = "TestingDeck";
		private const string PluginVersion = "1.0.0";

		internal static ManualLogSource Log;

		void Awake()
		{
			Log = base.Logger;

			var harmony = new Harmony(PluginGuid);
			harmony.PatchAll();
		}

		// [HarmonyPatch(typeof(CardDisplayer3D), "UpdateTribeIcon")]
		// public class Displayer
		// {
		// 	[HarmonyPrefix]
		// 	public static bool PrefixIcons(CardInfo info, CardDisplayer3D __instance)
		// 	{
		// 		if (info is not null)
		// 			Log.LogDebug($"[UpdateTribeIcon] Prefix - CardInfo [{info.name}]");
		// 		if (__instance.tribeIconRenderers is not null)
		// 		{
		// 			Log.LogDebug(
		// 				$"[UpdateTribeIcon] Current TribeIcons [{string.Join(", ", __instance.tribeIconRenderers.Select(rend => rend.sprite).ToList())}]");
		// 		}
		// 		
		// 		List<Tribe> tribes = Enum.GetValues(typeof(Tribe)).Cast<Tribe>().ToList();
		// 		foreach (var tribe in tribes.Where(tribe => info.IsOfTribe(tribe)))
		// 		{
		// 			foreach (SpriteRenderer spriteRenderer in __instance.tribeIconRenderers)
		// 			{
		// 				SpriteRenderer spriteRenderer2 = spriteRenderer;
		// 					// string str = "Art/Cards/TribeIcons/tribeicon_";
		// 					// Tribe tribe = (Tribe)i;
		// 					Texture2D texture = new Texture2D(2, 2);
		// 					texture.LoadImage(File.ReadAllBytes(Directory.GetFiles(Paths.PluginPath, "YisusLeshyDecalColorFull.png", SearchOption.AllDirectories)[0]));
		// 					Sprite test = Sprite.Create(texture, new Rect(0f,0f, 109f, 149f), CardUtils.DefaultVector2);
		// 					spriteRenderer2.sprite = test;
		// 					break;
		// 			}
		// 		}
		//
		//
		// 		return false;
		// 	}
		// 	
		// 	[HarmonyPostfix]
		// 	public static void PostfixIcons(CardInfo info, CardDisplayer3D __instance)
		// 	{
		// 		if (info is not null)
		// 			Log.LogDebug($"[UpdateTribeIcon] Postfix - CardInfo [{info.name}]");
		// 		if (__instance.tribeIconRenderers is not null)
		// 		{
		// 			Log.LogDebug(
		// 				$"[UpdateTribeIcon] Current TribeIcons [{string.Join(", ", __instance.tribeIconRenderers.Select(rend => rend.sprite).ToList())}]");
		// 		}
		// 	}
		// }

		// add this to your deck by scrolling upwards/pressing w key when at the map
		[HarmonyPatch(typeof(DeckReviewSequencer), "OnEnterDeckView")]
		public class AddCardsToDeckPatch
		{
			private static bool allowSettingDeck = true;

			[HarmonyPrefix]
			public static void AddCardsToDeck()
			{
				if (allowSettingDeck)
				{
					// CardInfo card = CardLoader.GetCardByName("TalkingCard");
					// Log.LogDebug($"[Summoner] Card [{card.name}] has abilities [{string.Join(",", card.abilities)}]");

					// SaveManager.SaveFile.CurrentDeck.Cards.Clear();

					// SaveManager.SaveFile.CurrentDeck.Cards.Add(card);
					// SaveManager.SaveFile.CurrentDeck.Cards.Add(card);
					SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName("TalkingCard"));
				}
			}
		}
	}
}
