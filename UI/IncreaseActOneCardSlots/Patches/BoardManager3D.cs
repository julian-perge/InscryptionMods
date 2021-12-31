namespace IncreaseActOneCardSlots.Patches
{
	[HarmonyLib.HarmonyPatch(typeof(DiskCardGame.BoardManager3D))]
	public class BoardManager3DPatch
	{
		private static readonly DiskCardGame.CardSlot CardSlotPrefab =
			ResourceBank.Get<DiskCardGame.CardSlot>("Prefabs/Cards/CardSlot");

		private static readonly DiskCardGame.HighlightedInteractable OpponentQueueSlotPrefab =
			ResourceBank.Get<DiskCardGame.HighlightedInteractable>("Prefabs/Cards/QueueSlot");

		[HarmonyLib.HarmonyPrefix, HarmonyLib.HarmonyPatch(nameof(DiskCardGame.BoardManager3D.Initialize))]
		public static void ModifyPositionsOfItemsPatch(DiskCardGame.BoardManager3D __instance)
		{
			UnityEngine.Transform boardObj = UnityEngine.GameObject.Find("CardBattle").transform.Find("Board");
			UnityEngine.Transform playerSlots = boardObj.Find("PlayerSlots");
			if (playerSlots.childCount == 4)
			{
				/// moving items so that they don't overlap on the board
				Plugin.Log.LogDebug($"Setting new Items position");
				UnityEngine.GameObject.Find("Items").transform.localPosition = new UnityEngine.Vector3(5.2f, 5.01f, 0f);

				ModifyBoardChildrenPositions(boardObj);

				/// creating new slots
				// player
				DiskCardGame.CardSlot playerSlot5 = CreateSlot<DiskCardGame.CardSlot>(
					"PlayerCardSlot5", playerSlots.transform, new UnityEngine.Vector3(3.4f, 0f, 0f), CardSlotPrefab
				);

				// opponent
				UnityEngine.Transform oppSlots = boardObj.Find("OpponentSlots");
				DiskCardGame.CardSlot opponentSlot5 = CreateSlot<DiskCardGame.CardSlot>(
					"OpponentCardSlot5", oppSlots.transform, new UnityEngine.Vector3(3.4f, 0f, 0f), CardSlotPrefab
				);
				UnityEngine.Transform oppCardSlot5Quad = opponentSlot5.transform.Find("Quad");
				// rotate slot on y-axis so that the paw image faces the Player like the other slots
				oppCardSlot5Quad.rotation = UnityEngine.Quaternion.Euler(90f, 180f, 0f);

				// opponent queue
				DiskCardGame.HighlightedInteractable opponentQueueSlot5 = CreateSlot<DiskCardGame.HighlightedInteractable>(
					"OpponentQueueSlot5",
					oppSlots.transform,
					new UnityEngine.Vector3(3.4f, 0f, 2.01f),
					OpponentQueueSlotPrefab
				);

				/// opposing slots
				playerSlot5.opposingSlot = opponentSlot5;
				opponentSlot5.opposingSlot = playerSlot5;

				/// now add each slot to the respective list
				AddSlotsToBoardInstance(__instance, playerSlot5, opponentSlot5, opponentQueueSlot5);
			}
		}

		private static void AddSlotsToBoardInstance(
			DiskCardGame.BoardManager3D __instance,
			DiskCardGame.CardSlot playerSlot5,
			DiskCardGame.CardSlot opponentSlot5,
			DiskCardGame.HighlightedInteractable opponentQueueSlot5
		)
		{
			Plugin.Log.LogDebug($"Adding player slot 5 to BoardManager3D field");
			__instance.playerSlots.Add(playerSlot5);

			Plugin.Log.LogDebug($"Adding opponent slot 5 to BoardManager3D field");
			__instance.opponentSlots.Add(opponentSlot5);

			Plugin.Log.LogDebug($"Adding opponent queue slot 5 to BoardManager3D field");
			__instance.opponentQueueSlots.Add(opponentQueueSlot5);
		}

		private static void ModifyBoardChildrenPositions(UnityEngine.Transform boardObj)
		{
			// board
			Plugin.Log.LogDebug($"Setting new Board position");
			boardObj.localPosition = new UnityEngine.Vector3(0f, 5f, 0f);

			// card draw piles
			Plugin.Log.LogDebug($"Setting new CardDrawPiles position");
			boardObj.Find("CardDrawPiles").transform.localPosition = new UnityEngine.Vector3(0.75f, 0f, 0f);

			// sacrifice tokens
			Plugin.Log.LogDebug($"Setting new SacrificeTokens position");
			boardObj.Find("SacrificeTokens").transform.localPosition = new UnityEngine.Vector3(4.2f, 0.03f, -0.4f);
		}

		private static T CreateSlot<T>(
			string name, UnityEngine.Transform parent, UnityEngine.Vector3 newPosition,
			DiskCardGame.HighlightedInteractable prefab)
			where T : DiskCardGame.HighlightedInteractable
		{
			Plugin.Log.LogDebug($"Creating slot {name}");
			T slot = (T)UnityEngine.Object.Instantiate(prefab, parent);
			slot.name = name;
			slot.transform.localPosition = newPosition;
			return slot;
		}
	}
}
