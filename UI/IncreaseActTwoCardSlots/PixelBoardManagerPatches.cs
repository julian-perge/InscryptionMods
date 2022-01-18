using System.Collections;
using System.Reflection;
using System.Resources;
using DiskCardGame;
using GBC;
using HarmonyLib;
using UnityEngine;
using static IncreaseActTwoCardSlots.Plugin;

namespace IncreaseActTwoCardSlots
{
	[HarmonyPatch(typeof(TurnManager))]
	public class CardBattleNPCPatches
	{
		private static readonly PixelCardSlot PrefabCardSlot =
			ResourceBank.Get<PixelCardSlot>("Prefabs/GBCCardBattle/PixelCardSlot");

		private static readonly PixelQueuedCardSlot PrefabOpponentQueueCardSlot =
			ResourceBank.Get<PixelQueuedCardSlot>("Prefabs/GBCCardBattle/PixelOpponentQueueSlot");

		[HarmonyPostfix, HarmonyPatch(nameof(TurnManager.SetupPhase))]
		public static void AddingSlotsAfterLoad(
			TurnManager __instance, EncounterData encounterData
			)
		{
			// Log.LogDebug($"Loading card battle");
			// SceneLoader.Load("GBC_CardBattle");

			// CustomCoroutine.WaitOnCondition(() => GameObject.FindObjectOfType<PixelBoardManager>() is not null);
			Log.LogDebug($"Finished routine");

			PixelBoardManager pixelBoardManager = GameObject.FindObjectOfType<PixelBoardManager>();

			Log.LogDebug($"Finished routine 2, pixel board manager [{pixelBoardManager}]");

			if (pixelBoardManager is not null)
			{

				// player slots
				Log.LogDebug($"Getting player slots");
				Transform playerSlots = pixelBoardManager.transform.GetChild(1);
				PixelCardSlot playerSlot5 = UnityEngine.Object.Instantiate(
					playerSlots.GetChild(3).gameObject,
					new Vector3(1.08f, -0.162f, 0f),
					Quaternion.identity,
					playerSlots.transform
				).GetComponent<PixelCardSlot>();

				Log.LogDebug($"Setting name for 5th slot");
				playerSlot5.name = "PlayerCardSlot_5";
				// playerSlot5.transform.position = new Vector3(1.08f, 0.48f, 0);

				BoardManager.Instance.playerSlots.Add(playerSlot5);
				playerSlots.position = new Vector3(-0.222f, -0.162f, 0f);

				Log.LogDebug($"moving opponent slots");
				// opponent slots
				AddOpponentSlots(pixelBoardManager, playerSlot5);


				Log.LogDebug($"moving scales");
				Transform scales = pixelBoardManager.transform.GetChild(4);
				scales.localPosition = new Vector3(-1.655f, -0.13f, 0f);

				FieldInfo field = typeof(PixelScales).GetField(
					"defaultPosition",
					System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
				Log.LogDebug($"Setting static field [{field.Name}] with new default position");
				field.SetValue(scales.GetComponent<PixelScales>(), new Vector2(-1.655f, -0.13f));

				Log.LogDebug($"moving combat bell");
				Transform combatBell = pixelBoardManager.transform.GetChild(5);
				combatBell.localPosition = new Vector3(-1.655f, 0.55f, 0f);

				Log.LogDebug($"moving resources manager");
				Transform resourcesManager = pixelBoardManager.transform.GetChild(6);
				resourcesManager.localPosition = new Vector3(-1.264f, 0.98f, 0f);

				Log.LogDebug($"moving card preview panel");
				Transform cardPreviewPanel = pixelBoardManager.transform.GetChild(9);
				cardPreviewPanel.localPosition = new Vector3(1.733f, 0.315f, 0f);

				Log.LogDebug($"excess damage panel");
				Transform excessDamagePanel = pixelBoardManager.transform.GetChild(10);
				excessDamagePanel.localPosition = new Vector3(-1.64f, 0.174f, 0f);

				Log.LogDebug($"updating lengths for highlight box");
				UpdateLengthsForHighlightBox(pixelBoardManager);

				Transform background = pixelBoardManager.transform.GetChild(3);
				background.localPosition = new Vector3(0.025f,0f);
				background.localScale = new Vector3(1.25f,1f, 1f);
			}

		}

		private static void UpdateLengthsForHighlightBox(Component pixelBoardManager)
		{
			GameObject slotsFrame = pixelBoardManager.transform.GetChild(14).gameObject;
			Transform leftSideBox = slotsFrame.transform.GetChild(2);
			Transform rightSideBox = slotsFrame.transform.GetChild(3);

			leftSideBox.localPosition = new Vector3(-11.1f, -0.1602f, 0f);
			rightSideBox.localPosition = new Vector3(11.1f, -0.1602f, 0f);
		}

		private static void AddOpponentSlots(Component pixelBoardManager, PixelCardSlot playerSlot5)
		{
			Transform opponentSlots = pixelBoardManager.transform.GetChild(2);

			PixelCardSlot opponentSlot5 = UnityEngine.Object.Instantiate(
				opponentSlots.GetChild(3).gameObject,
				new Vector3(1.08f, 0.48f, 0f),
				Quaternion.identity,
				opponentSlots.transform
			).GetComponent<PixelCardSlot>();

			opponentSlot5.name = "OpponentCardSlot_5";
			// opponentSlot5.transform.position = new Vector3(1.08f, 0.48f, 0);

			PixelCardSlot opponentQueueSlot5 = UnityEngine.Object.Instantiate(
				opponentSlots.GetChild(4),
				new Vector3(1.08f, 0.931f, 0f),
				Quaternion.identity,
				opponentSlots.transform
			).GetComponent<PixelQueuedCardSlot>();

			opponentQueueSlot5.name = "OpponentQueueCardSlot_5";
			// opponentQueueSlot5.transform.position = new Vector3(1.08f, 0.48f, 0);

			opponentSlot5.opposingSlot = playerSlot5;
			playerSlot5.opposingSlot = opponentSlot5;
			BoardManager.Instance.opponentSlots.Add(opponentSlot5);
			BoardManager.Instance.opponentQueueSlots.Add(opponentQueueSlot5);

			opponentSlots.position = new Vector3(-0.222f, 0.481f, 0f);
		}
	}
}
