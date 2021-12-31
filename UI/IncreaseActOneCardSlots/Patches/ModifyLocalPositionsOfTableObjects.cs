namespace IncreaseActOneCardSlots.Patches
{
	public class ModifyLocalPositionsOfTableObjects
	{
		// KnivesTableEffects(Clone)/RightSide/RepeatingConveyorKnives, change z-axis to 4.5

		[HarmonyLib.HarmonyPatch(typeof(DiskCardGame.Part1BossOpponent),
			nameof(DiskCardGame.Part1BossOpponent.SpawnScenery))]
		public class ModifyTreePositionsForProspector
		{
			private static readonly System.Collections.Generic.List<int> TreesToModifyPositiveZedPosition =
				new() { 2, 3, 4, 5 };

			static void SetNewPosForTree(UnityEngine.Transform transform, UnityEngine.Vector3 newPosition)
			{
				// group47
				transform.GetChild(0).localPosition = newPosition;
				// tree
				transform.GetChild(1).localPosition = newPosition;
			}

			[HarmonyLib.HarmonyPostfix]
			static void Postfix(DiskCardGame.Part1BossOpponent __instance)
			{
				var sceneryObj = __instance.sceneryObject;

				if (__instance is DiskCardGame.ProspectorBossOpponent)
				{
					Plugin.Log.LogDebug(
						$"[{__instance.GetType()}] ForestTableEffects has been spawned, scene object is [{sceneryObj}]");

					TreesToModifyPositiveZedPosition.ForEach(treeIndex =>
					{
						// ForestTableEffects -> GetChild(1) == SurroundingTrees
						UnityEngine.Vector3 newPos = new UnityEngine.Vector3(0f, 0f, treeIndex == 4 ? -2f : 2f);
						SetNewPosForTree(sceneryObj.transform.GetChild(1).GetChild(treeIndex), newPos);

						Plugin.Log.LogDebug($"moving tree {treeIndex} to new position [{newPos.ToString()}]");
					});
				}
				else if (__instance is DiskCardGame.TrapperTraderBossOpponent)
				{
					Plugin.Log.LogDebug(
						$"[{__instance.GetType()}] KnivesTableEffects has been spawned, scene object is [{sceneryObj}]");

					// KnivesTableEffects -> GetChild(0) == LeftSide
					UnityEngine.Transform knivesLeftSideTransform = sceneryObj.transform.GetChild(0);
					UnityEngine.Vector3 knivesLeftSideLocalPos = knivesLeftSideTransform.transform.localPosition;
					Plugin.Log.LogDebug($"moving left side knives back a little...");
					knivesLeftSideTransform.localPosition = new UnityEngine.Vector3(
						-1f, knivesLeftSideLocalPos.y, knivesLeftSideLocalPos.z
					);

					// KnivesTableEffects -> GetChild(1) == RightSide
					UnityEngine.Transform knivesRightSideTransform = sceneryObj.transform.GetChild(1);
					UnityEngine.Vector3 knivesRightSideLocalPos = knivesRightSideTransform.transform.localPosition;
					Plugin.Log.LogDebug($"moving right side knives back a little...");
					knivesRightSideTransform.localPosition = new UnityEngine.Vector3(
						1, knivesRightSideLocalPos.y, knivesRightSideLocalPos.z
					);
				}
			}
		}

		[HarmonyLib.HarmonyPostfix,
		 HarmonyLib.HarmonyPatch(nameof(DiskCardGame.BoardManager3D.TransitionAndResolveCreatedCard))]
		public static void ChangeScaleOfMoonCardToFitAcrossAllSlots(
			DiskCardGame.PlayableCard card, DiskCardGame.CardSlot slot, float transitionLength, bool resolveTriggers = true
		)
		{
			if (card.Info.HasTrait(DiskCardGame.Trait.Giant))
			{
				Plugin.Log.LogDebug($"Setting new scaling and position of the moon");
				// Card -> Quad -> CardBase
				var cardBase = card.transform.GetChild(0).GetChild(0);

				var localScale = cardBase.localScale;
				cardBase.localScale = new UnityEngine.Vector3(5.75f, localScale.y, localScale.z);

				var localPosition = cardBase.localPosition;
				cardBase.localPosition = new UnityEngine.Vector3(-2.25f, localPosition.y, localPosition.z);
			}
		}

		[HarmonyLib.HarmonyPatch]
		public class SetCandleAndRuleBookPositionsPatch
		{
			[HarmonyLib.HarmonyPrefix,
			 HarmonyLib.HarmonyPatch(typeof(DiskCardGame.TableRuleBook), nameof(DiskCardGame.TableRuleBook.Awake))]
			public static void ChangeTableRuleBookDefaultPosition(DiskCardGame.TableRuleBook __instance)
			{
				Plugin.Log.LogDebug($"Setting new position for TableRuleBook");
				__instance.transform.localPosition = new UnityEngine.Vector3(-4.69f, 0f, -4f);
			}

			[HarmonyLib.HarmonyPrefix,
			 HarmonyLib.HarmonyPatch(typeof(DiskCardGame.CandleHolder), nameof(DiskCardGame.CandleHolder.Awake))]
			public static void ChangeCandleHolderLocalPosition(DiskCardGame.CandleHolder __instance)
			{
				Plugin.Log.LogDebug($"Setting new position for CandleHolder");
				__instance.transform.localPosition = new UnityEngine.Vector3(6f, -0.006000042f, 1.1f);
			}
		}
	}
}
