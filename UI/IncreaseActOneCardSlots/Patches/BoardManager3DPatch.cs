using DiskCardGame;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

namespace IncreaseActOneCardSlots
{

	//[HarmonyPatch(typeof(Part1BossOpponent), nameof(Part1BossOpponent.SpawnScenery))]
	//public class ModifyTreePositionsForProspector
	//{
	//	private static readonly List<int> treesToModifyXPosition = new List<int>() { 2, 4, 5 };

	//	[HarmonyPostfix]
	//	static void Postfix(string id)
	//	{
	//		if (id.Equals("ForestTableEffects"))
	//		{
	//			Plugin.Log.LogDebug($"ForestTableEffects is being spawned");
	//			GameObject surroundingTrees = UnityEngine.GameObject.Find("ForestTableEffects(Clone)").transform.Find("SurroundingTrees").gameObject;
	//			treesToModifyXPosition.ForEach(t =>
	//			{
	//				Transform treeTransform = surroundingTrees.transform.GetChild(t);
	//				Vector3 treeLocalPosition = treeTransform.transform.localPosition;
	//				Plugin.Log.LogDebug($"moving tree {t} back a little...");
	//				treeTransform.localPosition = new Vector3(treeLocalPosition.x - 0.4f, treeLocalPosition.y, treeLocalPosition.z);
	//			});
	//		}
	//	}
	//}

	[HarmonyPatch(typeof(DiskCardGame.BoardManager3D), nameof(DiskCardGame.BoardManager3D.Initialize))]
	public class BoardManager3DPatch
	{

		private static CardSlot cardSlotPrefab = ResourceBank.Get<CardSlot>("Prefabs/Cards/CardSlot");
		private static HighlightedInteractable opponentQueueSlotPrefab = ResourceBank.Get<HighlightedInteractable>("Prefabs/Cards/QueueSlot");

		[HarmonyPostfix]
		public static void ChangePrefabAfter(BoardManager3D __instance)
		{
			Transform boardObj = UnityEngine.GameObject.Find("CardBattle").transform.Find("Board");
			Transform playerSlots = boardObj.Find("PlayerSlots");
			if (playerSlots.childCount == 4)
			{
				// candle
				Transform objectsOnTable = UnityEngine.GameObject.Find("Environment").transform.Find("ObjectsOnTable");
				objectsOnTable.Find("CandleHolder").transform.localPosition = new Vector3(6f, 0f, 1.1f);
				// rulebook
				objectsOnTable.Find("TableRuleBook").transform.localPosition = new Vector3(-4.69f, 0f, -4f);

				/// moving items so that they don't overlap on the board
				Plugin.Log.LogInfo("Setting items position farther to the right");
				UnityEngine.GameObject.Find("Items").transform.localPosition = new Vector3(5.35f, 5.01f, 0f);

				// board
				boardObj.localPosition = new Vector3(0f, 5f, 0f);

				// card draw piles
				boardObj.Find("CardDrawPiles").transform.localPosition = new Vector3(0.75f, 0f, 0f);


				/// creating new slots
				// player
				CardSlot playerSlot5 = CreateSlot<CardSlot>("PlayerCardSlot5", playerSlots.transform, new Vector3(3.4f, 0f, 0f), cardSlotPrefab);

				// opponent
				Transform oppSlots = boardObj.Find("OpponentSlots");
				CardSlot opponentSlot5 = CreateSlot<CardSlot>("OpponentCardSlot5", oppSlots.transform, new Vector3(3.4f, 0f, 0f), cardSlotPrefab);
				Transform oppCardSlot5Quad = opponentSlot5.transform.Find("Quad");
				// rotate slot on y-axis so that the paw image faces the Player like the other slots
				oppCardSlot5Quad.rotation = Quaternion.Euler(90f, 180f, 0f);

				// opponent queue
				HighlightedInteractable opponentQueueSlot5 = CreateSlot<HighlightedInteractable>("OpponentQueueSlot5", oppSlots.transform, new Vector3(3.4f, 0f, 2.01f), opponentQueueSlotPrefab);

				/// opposing slots
				playerSlot5.opposingSlot = opponentSlot5;
				opponentSlot5.opposingSlot = playerSlot5;

				/// now add each slot to the respective list
				Plugin.Log.LogInfo($"Adding player slot 5 to BoardManager3D field");
				__instance.playerSlots.Add(playerSlot5);

				Plugin.Log.LogInfo($"Adding opponent slot 5 to BoardManager3D field");
				__instance.opponentSlots.Add(opponentSlot5);

				Plugin.Log.LogInfo($"Adding opponent queue slot 5 to BoardManager3D field");
				__instance.opponentQueueSlots.Add(opponentQueueSlot5);
			}
		}

		static T CreateSlot<T>(string name, Transform parent, Vector3 newPosition, HighlightedInteractable prefab) where T : HighlightedInteractable
		{
			Plugin.Log.LogInfo($"Creating slot {name}");
			T slot = (T)UnityEngine.Object.Instantiate(prefab, parent);
			slot.name = name;
			slot.transform.localPosition = newPosition;
			return slot;
		}
	}
}
