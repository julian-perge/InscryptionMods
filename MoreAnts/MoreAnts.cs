using System;
using BepInEx;
using BepInEx.Logging;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.IO;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using APIPlugin;
using CardLoaderPlugin.lib;
using PluginInfo = API.PluginInfo;

namespace MoreAnts
{
	[BepInPlugin("com.julianperge.moreAnts", "Dome Ant", "1.0")]
	[BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
	public class MoreAnts : BaseUnityPlugin
	{
		private const string PluginGuid = "com.julianperge.moreAnts";
		private const string PluginName = "MoreAnts";
		private const string PluginVersion = "1.0.0.0";

		private void Awake()
		{
			// var imgBytes = System.IO.File.ReadAllBytes("BepInEx/plugins/CardLoader/Artwork/skele2.png");
			// Texture2D tex = new Texture2D(2, 2);
			// tex.LoadImage(imgBytes);

			// new CustomCard("Bullfrog") { tex: tex };

			// CardUtils.PrintAllCardInfo();

			// addDomeAnt();

			// var ants = StatIconInfo.AllIconInfo.Find((StatIconInfo x) => x.iconType == SpecialStatIcon.Ants);
			// ants.appliesToAttack = false;
			// ants.appliesToHealth = true;
			// StatIconInfo.AllIconInfo.Add(ants);

			Harmony harmony = new Harmony(PluginGuid);
			harmony.PatchAll();
		}

		[HarmonyPatch(typeof(DeckInfo), "InitializeAsPlayerDeck")]
		public class RandomizeBullfrogPatch : DeckInfo
		{
			// Token: 0x06000003 RID: 3 RVA: 0x00002098 File Offset: 0x00000298
			[HarmonyPrefix]
			public static bool Prefix(ref DeckInfo __instance)
			{
				bool flag = StoryEventsData.EventCompleted(StoryEvent.CageCardDiscovered) &&
				            !StoryEventsData.EventCompleted(StoryEvent.WolfCageBroken);
				if (flag)
				{
					__instance.AddCard(CardLoader.GetCardByName("CagedWolf"));
				}
				else
				{
					bool flag2 = StoryEventsData.EventCompleted(StoryEvent.TalkingWolfCardDiscovered);
					if (flag2)
					{
						__instance.AddCard(CardLoader.GetCardByName("Wolf_Talking"));
					}
					else
					{
						__instance.AddCard(CardLoader.GetCardByName("Wolf"));
					}
				}

				bool flag3 = StoryEventsData.EventCompleted(StoryEvent.StinkbugCardDiscovered);
				if (flag3)
				{
					__instance.AddCard(CardLoader.GetCardByName("Stinkbug_Talking"));
				}
				else
				{
					__instance.AddCard(CardLoader.GetCardByName("Opossum"));
				}

				__instance.AddCard(CardLoader.GetCardByName("Stoat_Talking"));
				__instance.AddCard(CardLoader.GetCardByName("DomeAnt"));
				return false;
			}
		}


		public static void addDomeAnt()
		{
			var imgBytesAnts = System.IO.File.ReadAllBytes("BepInEx/plugins/CardLoader/Artwork/dome_ant.png");
			Texture2D texAnts = new Texture2D(2, 2);
			texAnts.LoadImage(imgBytesAnts);

			StatIconInfo info2 = StatIconInfo.GetIconInfo(SpecialStatIcon.Ants);
			info2.rulebookName = "Ant Guardian";
			info2.rulebookDescription = "Defense is equal to number of ants on field";
			info2.metaCategories = new List<AbilityMetaCategory>() { AbilityMetaCategory.Part1Rulebook };
			// new NewAbility(info2, typeof(DomeAnt), texAnts);

			List<CardMetaCategory> metaCategories =
				new List<CardMetaCategory>() { CardMetaCategory.ChoiceNode, CardMetaCategory.TraderOffer };
			// metaCategories.Add(CardMetaCategory.Rare);
			string name = "DomeAnt";
			string displayedName = "Dome Ant";
			string descryption = "Loves to guard his friends";
			EvolveParams evolveParams =
				new EvolveParams() { turnsToEvolve = 1, evolution = CardLoader.GetCardByName("AntQueen") };
			List<Tribe> tribes = new List<Tribe>() { Tribe.Insect };
			List<Trait> traits = new List<Trait>() { Trait.Ant };

			// var imgBytes = System.IO.File.ReadAllBytes("BepInEx/plugins/CardLoader/Artwork/dome_ant.png");
			// Texture2D tex = new Texture2D(2, 2);
			// tex.LoadImage(imgBytes);

			List<SpecialTriggeredAbility> abilities = new List<SpecialTriggeredAbility>() { SpecialTriggeredAbility.Ant };
			NewCard.Add(
				name,
				metaCategories,
				CardComplexity.Advanced,
				CardTemple.Nature,
				displayedName,
				0, 2, description: descryption,
				evolveParams: evolveParams,
				cost: 1,
				tex: texAnts,
				specialStatIcon: SpecialStatIcon.Ants,
				specialAbilities: abilities,
				tribes: tribes,
				traits: traits
			);
		}
	}

	[HarmonyPatch(typeof(DiskCardGame.Ant))]
	public class VarStatPatch
	{
		[HarmonyPatch(nameof(Ant.GetStatValues))]
		[HarmonyPostfix]
		public static int[] ChangeStatValues(int[] __result)
		{
			var attack = __result[0];
			var health = __result[1];
			return new int[] { health, attack };
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

	[HarmonyPatch(typeof(DiskCardGame.CardStatIcons))]
	public class CardStatIconPatch
	{
		[HarmonyPatch(nameof(CardStatIcons.AssignStatIcon))]
		[HarmonyPrefix]
		public static bool ChangeIcon(CardStatIcons __instance, SpecialStatIcon icon, PlayableCard playableCard)
		{
			if (playableCard is not null && (playableCard.name.Contains("Ant") || playableCard.name.Contains("ant")))
			{
				
				FileLog.Log($"AssignStatIcon called for card [{playableCard.name}]." +
				            $"\n-->Attack icon {__instance.attackIconRenderer.material.mainTexture} " +
				            $"Health icon {__instance.healthIconRenderer.material.mainTexture}");
				
				StatIconInteractable component = __instance.attackIconRenderer.GetComponent<StatIconInteractable>();
				StatIconInteractable component2 = __instance.healthIconRenderer.GetComponent<StatIconInteractable>();
				__instance.attackIconRenderer.material.mainTexture = null;
				component.AssignStat(SpecialStatIcon.None, playableCard);
				__instance.healthIconRenderer.material.mainTexture = null;
				component2.AssignStat(SpecialStatIcon.None, playableCard);
				if (icon != SpecialStatIcon.None)
				{
					StatIconInfo iconInfo = StatIconInfo.GetIconInfo(icon);
					FileLog.Log($"-> Applies to attack [{iconInfo.appliesToAttack}]");
					FileLog.Log($"-> Applies to health [{iconInfo.appliesToHealth}]");
					if (!iconInfo.appliesToAttack)
					{
						FileLog.Log($"--> Applies to attack [{iconInfo.appliesToAttack}]");
						__instance.attackIconRenderer.material.mainTexture = iconInfo.iconGraphic;
						component.AssignStat(icon, playableCard);
					}
					if (!iconInfo.appliesToHealth)
					{
						FileLog.Log($"--> Applies to health [{iconInfo.appliesToHealth}]");
						__instance.healthIconRenderer.material.mainTexture = iconInfo.iconGraphic;
						component2.AssignStat(icon, playableCard);
					}
				}
				FileLog.Log("-> Finished assigning new vars\n");
				return false;
			}

			return true;
		}

		// [HarmonyPatch(nameof(CardStatIcons.UpdateIconsActive))]
		// [HarmonyPostfix]
		// public static void ChangeIcon(ref CardStatIcons __instance, bool attackActive, bool healthActive)
		// {
		// 	FileLog.Log($"UpdateIconsActive called. Attack active [{attackActive}] Health active [{healthActive}]\n");
		// }
	}

	// [HarmonyPatch(typeof(DiskCardGame.Card))]
	// public class CardPatch
	// {
	// 	[HarmonyPatch(nameof(Card.UpdateInteractableIcons))]
	// 	[HarmonyPostfix]
	// 	public static void ChangeAfterMethodRan(ref Card __instance)
	// 	{
	// 		if (__instance != null)
	// 		{
	// 			var pCard = (__instance as PlayableCard);
	// 			if (__instance is PlayableCard && (pCard.name.Contains("Ant") || pCard.name.Contains("ant")))
	// 			{
	// 				// FileLog.Log($"-> [UpdateInteractableIcons] Setting icons back to false, true");
	// 				// __instance.statIcons.UpdateIconsActive(false, true);
	// 				// FileLog.Log($"UpdateInteractableIcons was called");
	// 				// FileLog.Log($"-> pCard {pCard.name}");
	// 				// foreach (var hiddenAbility in pCard.Status.hiddenAbilities)
	// 				// {
	// 				// 	FileLog.Log($"-> hAbility {hiddenAbility}");
	// 				// }
	//
	// 				// FileLog.Log("\n");
	// 			}
	// 		}
	// 	}
	//
	// 	[HarmonyPatch(nameof(Card.AttachAbilities))]
	// 	[HarmonyPrefix]
	// 	public static void PrefixAttach(ref Card __instance, CardInfo info)
	// 	{
	// 		if (info.name.Equals("Ant"))
	// 		{
	// 			FileLog.Log($"Card AttachAbilities was called with {info.name}");
	// 			foreach (var a in info.SpecialAbilities)
	// 			{
	// 				FileLog.Log($"-> SpecAbility {a.ToString()} Base GO {__instance.gameObject}");
	// 			}
	//
	// 			FileLog.Log("\n");
	// 		}
	// 	}
	// }


	// [HarmonyPatch(typeof(DiskCardGame.PlayableCard))]
	// public class SelectCardInitPatch
	// {
	// 	[HarmonyPatch(nameof(PlayableCard.AttachAbilities))]
	// 	[HarmonyPrefix]
	// 	public static void Prefix(ref PlayableCard __instance, CardInfo info)
	// 	{
	// 		if (info.name.Equals("Ant"))
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


	// [HarmonyPatch(typeof(DiskCardGame.CardTriggerHandler))]
	// public class CardTriggerHandlerPatch
	// {
	// 	[HarmonyPatch(nameof(DiskCardGame.CardTriggerHandler.AddAbility), new[] { typeof(SpecialTriggeredAbility) })]
	// 	[HarmonyPrefix]
	// 	public static void PrefixTest(ref CardTriggerHandler __instance, SpecialTriggeredAbility ability)
	// 	{
	// 		FileLog.Log($"CardTriggerHandler addability was called [{ability}]\n");
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
