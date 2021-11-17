using System;
using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;

namespace MoreAnts
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency("cyantist.inscryption.api")]
	public class MoreAnts : BaseUnityPlugin
	{
		public const string PluginGuid = "julianperge.inscryption.cards.moreAnts";
		private const string PluginName = "MoreAnts";
		private const string PluginVersion = "1.0";
		internal static ManualLogSource Log;

		void Awake()
		{
			Log = base.Logger;

			DomeAnt.InitCard();

			Harmony harmony = new Harmony(PluginGuid);
			harmony.PatchAll();
		}
	}

	// add this to your deck by scrolling upwards/pressing w key when at the map
	[HarmonyPatch(typeof(DeckReviewSequencer), nameof(DeckReviewSequencer.OnEnterDeckView))]
	public class AddCardsToDeckPatch
	{
		private static bool allowSettingDeck = true;

		[HarmonyPrefix]
		public static void AddCardsToDeck()
		{
			if (allowSettingDeck)
			{
				Console.WriteLine("Starting to load Exodia cards into deck");
				CardInfo card = CardLoader.GetCardByName("DomeAnt");

				// CardUtils.PrintCardInfo(card);

				// SaveManager.SaveFile.CurrentDeck.Cards.Clear();

				// SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName("Geck"));
				// SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName("Ant"));
				SaveManager.SaveFile.CurrentDeck.Cards.Add(card);
			}
		}
	}

	// [HarmonyPatch(typeof(DiskCardGame.CardAbilityIcons))]
	// public class CardAbilityPatches
	// {
	// 	[HarmonyPatch(nameof(CardAbilityIcons.SetIconFlipped))]
	// 	[HarmonyPrefix]
	// 	public static void SetIconFlippedPrefix(Ability ability, bool flipped)
	// 	{
	// 		FileLog.Log($"SetIconFlipped called. Ability [{ability}] Flipped [{flipped}]\n");
	// 	}
	//
	// 	[HarmonyPatch(nameof(CardAbilityIcons.ChangeIconForAbility))]
	// 	[HarmonyPrefix]
	// 	public static void ChangeIconForAbilityPrefix(Ability ability, Texture texture)
	// 	{
	// 		FileLog.Log($"ChangeIconForAbility called. Ability [{ability}] Texture [{texture.name}]\n");
	// 	}
	//
	// 	[HarmonyPatch(nameof(CardAbilityIcons.UpdateAbilityIcons))]
	// 	[HarmonyPrefix]
	// 	public static void LogAbilityIcons(CardInfo info, List<CardModificationInfo> mods, PlayableCard playableCard,
	// 		List<Ability> hiddenAbilities)
	// 	{
	// 		if (info is not null && info.name.Contains("Ant"))
	// 		{
	// 			FileLog.Log($"UpdateAbilityIcons was called with card info {info.name} Mods count {mods.Count}");
	// 			foreach (var mod in mods)
	// 			{
	// 				FileLog.Log($"-> card mod id {mod.singletonId}");
	// 			}
	//
	// 			foreach (var ability in hiddenAbilities)
	// 			{
	// 				FileLog.Log($"-> hidden ability {ability}");
	// 			}
	//
	// 			FileLog.Log("\n");
	// 		}
	// 	}
	//
	// 	// [HarmonyPatch(nameof(CardAbilityIcons.ApplyAbilitiesToIcons))]
	// 	// [HarmonyPrefix]
	// 	// public static void Prefix(List<Ability> abilities, List<AbilityIconInteractable> icons, Material iconMat,
	// 	// 	CardInfo info, PlayableCard playableCard)
	// 	// {
	// 	// 	if (info is not null && (info.name.Contains("Ant") || info.name.Contains("ant")))
	// 	// 	{
	// 	// 		FileLog.Log($"-> ApplyAbilitiesToIcons was called with card info {info.name} and count of icons {icons.Count} abilities count {abilities.Count}");
	// 	// 		FileLog.Log($"--> CardInfo {info.name} PlayableCard {playableCard.name}");
	// 	// 		foreach (var ability in abilities)
	// 	// 		{
	// 	// 			FileLog.Log($"--> Ability {ability}");
	// 	// 		}
	// 	//
	// 	// 		foreach (var icon in icons)
	// 	// 		{
	// 	// 			FileLog.Log($"--> icon GO {icon.gameObject}");
	// 	// 		}
	// 	// 		FileLog.Log("\n");
	// 	// 	}
	// 	// }
	// }
	//
	// [HarmonyPatch(typeof(AbilityIconInteractable))]
	// public class AbilityIconInteractable_Patch
	// {
	// 	[HarmonyPatch(nameof(AbilityIconInteractable.AssignAbility))]
	// 	[HarmonyPrefix]
	// 	public static void PrefixAbilityIcon(Ability ability, CardInfo info, PlayableCard card)
	// 	{
	// 		if (info is not null && card is not null && info.name.Equals("Ant"))
	// 		{
	// 			FileLog.Log(
	// 				$"---> AssignAbility was called with Ability {ability} Info {info.name} PlayableCard {card.name}\n");
	// 		}
	// 	}
	// }

	// [HarmonyPatch(typeof(PlayableCard))]
	// public class P_Card_Patch
	// {
	// 	[HarmonyPatch(nameof(PlayableCard.AddTemporaryMod))]
	// 	[HarmonyPrefix]
	// 	public static void PrefixAbilityIcon(CardModificationInfo mod)
	// 	{
	// 		if (mod is not null)
	// 		{
	// 			FileLog.Log($"-> AddTempMod was called, {mod.singletonId}");
	// 			foreach (var ability in mod.abilities)
	// 			{
	// 				FileLog.Log($"--> Adding ability, {ability}");
	// 			}
	//
	// 			foreach (var ability in mod.specialAbilities)
	// 			{
	// 				FileLog.Log($"--> Adding spec ability, {ability}");
	// 			}
	//
	// 			FileLog.Log("\n");
	// 		}
	// 	}
	// }
	//
	// [HarmonyPatch(typeof(DiskCardGame.CardDisplayer))]
	// public class CardDisplayerPatch
	// {
	// 	[HarmonyPatch(nameof(CardDisplayer.DisplayInfo))]
	// 	[HarmonyPrefix]
	// 	public static void CheckHiddenCosts(ref CardDisplayer __instance, CardRenderInfo renderInfo,
	// 		PlayableCard playableCard)
	// 	{
	// 		if (__instance is not null && renderInfo is not null && playableCard is not null)
	// 		{
	// 			if (__instance.info is not null && __instance.info.name.Equals("Ant"))
	// 			{
	// 				FileLog.Log(
	// 					$"CardDisplayer.DisplayInfo called\n-> Info hidden attack [{__instance.info.hideAttackAndHealth}] Render Info hidden attack [{renderInfo.hiddenAttack}] hidden health [{renderInfo.hiddenHealth}]");
	// 				FileLog.Log($"->Stats text attack [{renderInfo.attack}] health [{renderInfo.health}]\n");
	// 				// renderInfo.hiddenAttack = false;
	// 				// renderInfo.hiddenHealth = true;
	// 			}
	// 		}
	// 	}
	// }

	// [HarmonyPatch(typeof(DiskCardGame.CardDisplayer3D))]
	// public class CardDisplay3dFix
	// {
	// 	[HarmonyPatch(nameof(CardDisplayer3D.DisplayInfo))]
	// 	[HarmonyPrefix]
	// 	public static void DisplayInfoPatch(ref CardDisplayer3D __instance, CardRenderInfo renderInfo,
	// 		PlayableCard playableCard)
	// 	{
	// 		if (__instance is not null && playableCard is not null && __instance.info is not null)
	// 		{
	// 			if (__instance.info.name.Equals("Ant")
	// 			    && __instance.info.SpecialStatIcon != SpecialStatIcon.None
	// 			    && !renderInfo.showSpecialStats)
	// 			{
	// 				FileLog.Log("CardDisplayer3D.DisplayInfo was called - \n");
	// 				// __instance.SetHealthAndAttackIconsActive(false, true);
	// 				// __instance.StatIcons.AssignStatIcon(__instance.info.SpecialStatIcon, playableCard);
	// 				// return false;
	// 			}
	//
	// 			// __instance.SetHealthAndAttackIconsActive(false, false);
	// 		}
	//
	// 		// return true;
	// 	}
	//
	// 	[HarmonyPatch(nameof(CardDisplayer3D.DisplaySpecialStatIcons))]
	// 	[HarmonyPrefix]
	// 	public static bool ChangeIcon(ref CardDisplayer3D __instance, CardRenderInfo renderInfo,
	// 		PlayableCard playableCard)
	// 	{
	// 		if (__instance is not null && playableCard is not null && __instance.info.name.Equals("Ant"))
	// 		{
	// 			if (__instance.info.SpecialStatIcon != SpecialStatIcon.None && !renderInfo.showSpecialStats)
	// 			{
	// 				FileLog.Log("DisplaySpecialStatIcons - was called, setting values false,true\n");
	// 				// __instance.SetHealthAndAttackIconsActive(false, true);
	// 				// __instance.StatIcons.AssignStatIcon(__instance.info.SpecialStatIcon, playableCard);
	// 				// return false;
	// 			}
	//
	// 			// __instance.SetHealthAndAttackIconsActive(false, false);
	// 		}
	//
	// 		return true;
	// 	}
	//
	// 	[HarmonyPatch(nameof(CardDisplayer3D.SetHealthAndAttackIconsActive))]
	// 	[HarmonyPrefix]
	// 	public static void ChangeAbilityIcon(ref CardDisplayer3D __instance, ref bool attackIconActive,
	// 		ref bool healthIconActive)
	// 	{
	// 		if (__instance is not null && __instance.info.name.Equals("Ant"))
	// 		{
	// 			FileLog.Log(
	// 				$"SetHealthAndAttackIconsActive was called. Attack Icon Active [{attackIconActive}] Health [{healthIconActive}]");
	// 			if (attackIconActive && !healthIconActive)
	// 			{
	// 				// attackIconActive = false;
	// 				// healthIconActive = true;
	// 			}
	//
	// 			FileLog.Log(
	// 				$"-> Attack Icon Active [{attackIconActive}] Health [{healthIconActive}]\n");
	// 		}
	// 	}
	//
	// 	[HarmonyPatch(nameof(CardDisplayer3D.DisplayAbilityIcons))]
	// 	[HarmonyPrefix]
	// 	public static void ChangeAbilityIcon(ref CardDisplayer3D __instance, CardRenderInfo renderInfo,
	// 		PlayableCard playableCard)
	// 	{
	// 		if (__instance is not null && playableCard is not null)
	// 		{
	// 			if (__instance.info.SpecialStatIcon != SpecialStatIcon.None && !renderInfo.showSpecialStats)
	// 			{
	// 				FileLog.Log("DisplayAbilityIcons - was called\n");
	// 				// __instance.SetHealthAndAttackIconsActive(false, true);
	// 				// __instance.StatIcons.AssignStatIcon(__instance.info.SpecialStatIcon, playableCard);
	// 				// return false;
	// 			}
	// 		}
	//
	// 		// return true;
	// 	}
	// }


	// [HarmonyPatch(nameof(CardStatIcons.UpdateIconsActive))]
	// [HarmonyPostfix]
	// public static void ChangeIcon(ref CardStatIcons __instance, bool attackActive, bool healthActive)
	// {
	// 	FileLog.Log($"UpdateIconsActive called. Attack active [{attackActive}] Health active [{healthActive}]\n");
	// }

	[HarmonyPatch(typeof(CustomType))]
	public class CardPatch
	{
		[HarmonyPatch(nameof(CustomType.GetType), typeof(string), typeof(string))]
		[HarmonyPrefix]
		public static void ChangeAfterMethodRan(string nameSpace, string typeName)
		{
			MoreAnts.Log.LogInfo($"Called CustomType.GetType with nameSpace [{nameSpace}] typeName [{typeName}]");
		}

		//
		// [HarmonyPatch(nameof(Card.AttachAbilities))]
		// [HarmonyPrefix]
		// public static void PrefixAttach(ref Card __instance, CardInfo info)
		// {
		// 	if (info.HasTrait(Trait.Ant))
		// 	{
		// 		MoreAnts.Log.LogInfo($"Card AttachAbilities was called with {info.name}");
		// 		// foreach (var a in info.SpecialAbilities)
		// 		// {
		// 		// 	// MoreAnts.Log.LogInfo($"-> SpecAbility {a.ToString()} Base GO {__instance.gameObject}");
		// 		// 	foreach (var value in Enum.GetValues(typeof(SpecialTriggeredAbility)))
		// 		// 	{
		// 		// 		MoreAnts.Log.LogInfo($"--> spectrigabil [{(int)value}] {value}");
		// 		// 	}
		// 		// }
		//
		// 		// FileLog.Log("\n");
		// 	}
		// }
	}


	// [HarmonyPatch(typeof(PlayableCard))]
	// public class SelectCardInitPatch
	// {
	// 	[HarmonyPatch(nameof(PlayableCard.AttachAbilities))]
	// 	[HarmonyPrefix]
	// 	public static void Prefix(ref PlayableCard __instance, CardInfo info)
	// 	{
	// 		if (info.HasTrait(Trait.Ant))
	// 		{
	// 			FileLog.Log(
	// 				$"PlayableCard AttachAbilities was called with {info.name} Temp mods {__instance.TemporaryMods.Count}");
	// 			foreach (var a in info.SpecialAbilities)
	// 			{
	// 				FileLog.Log($"-> SpecAbility {a.ToString()} Base GO {__instance.gameObject}");
	// 			}
	// 		}
	// 	}
	// }


	// [HarmonyPatch(typeof(CardTriggerHandler))]
	// public class CardTriggerHandlerPatch
	// {
	// 	[HarmonyPatch("GetType", typeof(string))]
	// 	[HarmonyPrefix]
	// 	public static void PrefixTest2(string typeName)
	// 	{
	// 		MoreAnts.Log.LogInfo($"Called CardTriggerHandler.GetType with typeName {typeName}");
	// 	}
	// }

	// [HarmonyPatch(typeof(DiskCardGame.StatIconInfo))]
	// public class StatIconInfoPatch
	// {
	// 	[HarmonyPatch(nameof(StatIconInfo.AllIconInfo), MethodType.Getter)]
	// 	[HarmonyPostfix]
	// 	public static void AddToList(ref List<StatIconInfo> __result)
	// 	{
	// 		StatIconInfo info = ScriptableObject.CreateInstance<StatIconInfo>();
	// 		info.metaCategories = new List<AbilityMetaCategory>() { AbilityMetaCategory.Part1Rulebook };
	// 		info.rulebookDescription = "Testing EXODIA";
	// 		info.rulebookName = "EXODIA, THE FORBIDDEN ONE";
	// 		info.gbcDescription = info.rulebookDescription;
	//
	// 		var imgBytes = System.IO.File.ReadAllBytes("BepInEx/plugins/CardLoader/Artwork/skele2.png");
	// 		Texture2D tex = new Texture2D(2, 2);
	// 		tex.LoadImage(imgBytes);
	// 	}
	//
	// 	[HarmonyPatch(nameof(StatIconInfo.GetIconInfo))]
	// 	[HarmonyPrefix]
	// 	public static void PrefixHealth(SpecialStatIcon icon)
	// 	{
	// 		// if (icon == SpecialStatIcon.Ants)
	// 		// FileLog.Log($"IconAppliesToHealth -> SSIcon is {icon}");
	// 	}
	// }
}
