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
	// [BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
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

			Log.LogInfo("~NATURE~");
			PrintingCardUtils.PrintAllCardInfo(CardTemple.Nature);
			Log.LogInfo("~TECH~");
			PrintingCardUtils.PrintAllCardInfo(CardTemple.Tech);
			Log.LogInfo("~UNDEAD~");
			PrintingCardUtils.PrintAllCardInfo(CardTemple.Undead);
			Log.LogInfo("~WIZARD~");
			PrintingCardUtils.PrintAllCardInfo(CardTemple.Wizard);

			// AddTalkingCard();

			var harmony = new Harmony(PluginGuid);
			harmony.PatchAll();
		}

		private static void AddTalkingCard()
		{
			var animatedBehaviour = new List<CardAppearanceBehaviour.Appearance>()
			{
				CardAppearanceBehaviour.Appearance.AnimatedPortrait
			};

			var specialTriggeredAbilities = new List<SpecialTriggeredAbility>()
			{
				SpecialTriggeredAbility.TalkingCardChooser
			};

			NewTribe tribeRose = NewTribe.Add("tribeicon_rose.png", "Rose");
			var tribes = new List<Tribe>() { tribeRose.tribe };

			const string talkingCardName = "TalkingCardTest";

			NewTalkingCard.Add<TalkingCardTest>(talkingCardName);

			Sprite mouthClosedSprite = ImageUtils.CreateSpriteFromPng(
				"sprite_man_mouth_closed.png", new Vector2(0.7f, 0.75f), 125f
			);

			Sprite mouthOpenSprite =
				ImageUtils.CreateSpriteFromPng("sprite_man_mouth_open.png", new Vector2(0.65f, 0.6f), 125f);

			var emotionSprites = NewTalkingCard.CreateSpritesForEmotion(
				faceSprite: ImageUtils.CreateSpriteFromPng("sprite_inscryption_face.png", new Vector2(0.5f, 0f)),
				eyesOpenSprite: ImageUtils.CreateSpriteFromPng("sprite_man_eyes_open.png", new Vector2(0.55f, 0.55f)),
				eyesClosedSprite: ImageUtils.CreateSpriteFromPng("sprite_man_eyes_closed.png", new Vector2(0.55f, 0.55f)),
				eyesOpenEmissionSprite: ImageUtils.CreateSpriteFromPng("sprite_man_eyes_emis_2.png", new Vector2(0.55f, 0.55f)),
				eyesClosedEmissionSprite: ImageUtils.CreateSpriteFromPng("sprite_man_eyes_emis_2.png",
					new Vector2(0.55f, 0.55f)),
				mouthOpenSprite: mouthOpenSprite,
				mouthClosedSprite: mouthClosedSprite
			);

			NewCard.Add(
				talkingCardName, "Talking Card Test", 1, 1,
				CardUtils.getNormalCardMetadata, CardComplexity.Simple, CardTemple.Nature, tribes: tribes,
				appearanceBehaviour: animatedBehaviour, specialAbilities: specialTriggeredAbilities,
				animatedPortrait: NewTalkingCard.CreateTalkingCardAnimation(emotionSprites)
			);
		}

		public class TalkingCardTest : StoatTalkingCard
		{
			// Static method for easy access
			public static DialogueEvent.Speaker Speaker => (DialogueEvent.Speaker)100;

			// Only important for multi-speaker dialogs
			public override DialogueEvent.Speaker SpeakerType => Speaker;

			private static readonly Dictionary<string, DialogueEvent> Events = new Dictionary<string, DialogueEvent>();

			public static Dictionary<string, DialogueEvent> GetDictionary()
			{
				if (Events.Count != 0)
				{
					return Events;
				}

				Events.Add("TalkingEightBearsDrawn",
					DialogueUtils.CreateDialogue("TalkingEightBearsDrawn", Speaker, "*Bear Noises*"));

				Events.Add("TalkingEightBearsDrawn2",
					DialogueUtils.CreateDialogue("TalkingEightBearsDrawn2", Speaker, "*Bear Noises*"));

				Events.Add("TalkingEightBearsPlayed",
					DialogueUtils.CreateDialogue("TalkingEightBearsPlayed", Speaker, "*Bear Noises*"));

				Events.Add("TalkingEightBearsAttacked",
					DialogueUtils.CreateDialogue("TalkingEightBearsAttacked", Speaker, "*Bear Noises*"));

				Events.Add("TalkingEightBearsPositiveSelectable",
					DialogueUtils.CreateDialogue("TalkingEightBearsPositiveSelectable", Speaker, "*Bear Noises*"));

				Events.Add("TalkingEightBearsNegativeSelectable",
					DialogueUtils.CreateDialogue("TalkingEightBearsNegativeSelectable", Speaker, "*Bear Noises*"));

				Events.Add("TalkingEightBearsSacrificed",
					DialogueUtils.CreateDialogue("TalkingEightBearsSacrificed", Speaker, "*Bear Noises*"));

				Events.Add("TalkingEightBearsMerged",
					DialogueUtils.CreateDialogue("TalkingEightBearsMerged", Speaker, "*Bear Noises*"));

				Events.Add("TalkingEightBearsRemoved",
					DialogueUtils.CreateDialogue("TalkingEightBearsRemoved", Speaker, "*Bear Noises*"));

				Events.Add("TalkingEightBearsDeckTrial",
					DialogueUtils.CreateDialogue("TalkingEightBearsDeckTrial", Speaker, "*Bear Noises*"));

				Events.Add("TalkingEightBearsDiscovered",
					DialogueUtils.CreateDialogue("TalkingEightBearsDiscovered", Speaker, "*Bear Noises*"));

				return Events;
			}
		}


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

					// SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName("Geck"));
					// SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName("Geck"));
					SaveManager.SaveFile.CurrentDeck.Cards.Add(CardLoader.GetCardByName("TalkingCardTest"));
				}
			}
		}
	}
}


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
// }
