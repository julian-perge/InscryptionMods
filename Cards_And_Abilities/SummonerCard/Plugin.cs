using System;
using System.Collections.Generic;
using APIPlugin;
using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace SummonerCard
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
	// [BepInDependency("julianperge.inscryption.sigiladay", BepInDependency.DependencyFlags.HardDependency)]
	public class Plugin : BaseUnityPlugin
	{
		public const string PluginGuid = "julian.inscryption.cards.testing";
		private const string PluginName = "TestingDeck";
		private const string PluginVersion = "1.0.0";

		internal static ManualLogSource Log;

		void Awake()
		{
			Log = base.Logger;

			AddCard();

			var harmony = new Harmony(PluginGuid);
			harmony.PatchAll();
		}

		void AddCard()
		{
			var animatedBehaviour = new List<CardAppearanceBehaviour.Appearance>()
			{
				CardAppearanceBehaviour.Appearance.AnimatedPortrait
			};

			var specialTriggeredAbilities = new List<SpecialTriggeredAbility>()
			{
				SpecialTriggeredAbility.TalkingCardChooser
			};

			const string talkingCardName = "TalkingCardTest";

			NewTalkingCard.Add<TalkingCardTest>(talkingCardName);

			GameObject animatedPortraitObj = NewTalkingCard.CreateTalkingCardAnimation(
				facePng: "testingcard_character_face.png",
				eyesOpenPng: "talkingcard_eyes_open1.png", eyesClosedPng: "talkingcard_eyes_closed1.png",
				mouthOpenPng: "talkingcard_mouth_open1.png", mouthClosedPng: "talkingcard_mouth_closed1.png");

			NewCard.Add(
				talkingCardName, "Talking Card Test", 1, 1,
				CardUtils.getNormalCardMetadata, CardComplexity.Simple, CardTemple.Nature,
				appearanceBehaviour: animatedBehaviour, specialAbilities: specialTriggeredAbilities,
				animatedPortrait: animatedPortraitObj
			);
		}

		public class TalkingCardTest : StoatTalkingCard
		{
		}

		[HarmonyPatch(typeof(DialogueDataUtil.DialogueData), nameof(DialogueDataUtil.DialogueData.GetEvent))]
		public class DialogueDataUtilPatching
		{
			static void Prefix(DialogueDataUtil.DialogueData __instance, string id)
			{
				Log.LogDebug($"[DialogueDataUtil.DialogueData] " +
				             $"Id is [{id}] " +
				             $"id exists in events [{__instance.events.Find((DialogueEvent x) => String.Equals(x.id, id, StringComparison.InvariantCultureIgnoreCase))}]");
			}
		}

		[HarmonyPatch(typeof(PlayerHand3D), nameof(PlayerHand3D.MoveCardAboveHand))]
		public class PlayerHand3DPatching
		{
			static void Prefix(PlayableCard card)
			{
				Log.LogDebug($"MoveCardAboveHand called with card [{card.Info.name}]");
			}
		}

		[HarmonyPatch(typeof(DiskCardGame.TalkingCard), nameof(DiskCardGame.TalkingCard.OnDrawn))]
		public class TalkingCardDebuggingPatch
		{
			static void Prefix(TalkingCard __instance)
			{
				bool existsInCurrentSpeakers =
					Singleton<TalkingCardDialogueHandler>.Instance.CurrentSpeakers.Contains(__instance);
				if (existsInCurrentSpeakers)
				{
					Log.LogDebug($"Instance exists in CurrentSpeakers");
				}

				bool specialSequencerIsBoss = Singleton<TurnManager>.Instance.SpecialSequencer is BossBattleSequencer;
				bool isNotSpecialTrailerBuild = !ScriptDefines.SPECIAL_TRAILER_BUILD;

				if (specialSequencerIsBoss && isNotSpecialTrailerBuild)
				{
					Log.LogDebug($"SpecialSequencer is BossBattleSequencer and isNotSpecialTrailerBuild. " +
					             $"OnDrawnSpecialOppDialogueIds contains boss type? [{__instance.OnDrawnSpecialOpponentDialogueIds.ContainsKey(((BossBattleSequencer)Singleton<TurnManager>.Instance.SpecialSequencer).BossType)}]");
				}
			}
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

					SaveManager.SaveFile.CurrentDeck.Cards.Clear();

					SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName("Geck"));
					SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName("Geck"));
					SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName("TalkingCardTest"));
				}
			}
		}
	}
}
