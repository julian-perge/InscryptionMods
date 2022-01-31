using DiskCardGame;

namespace IncreaseActOneCardSlots.Patches;

[HarmonyLib.HarmonyPatch]
public class ModifyLocalPositionsOfTableObjects
{
	// KnivesTableEffects(Clone)/RightSide/RepeatingConveyorKnives, change z-axis to 4.5

		[HarmonyLib.HarmonyPatch(
				typeof(DiskCardGame.Part1BossOpponent),
				nameof(DiskCardGame.Part1BossOpponent.SpawnScenery)
			)
		]
		public class ModifyTreePositionsForProspector
		{
			private static readonly System.Collections.Generic.List<int> TreesToModifyPositiveZedPosition =
				new() { 2, 3, 4, 5 };

			private static readonly System.Collections.Generic.List<int> KnivesEachSideIndex =
				new() { 0, 1 };

			static void SetNewPositionForGameObject(UnityEngine.Transform transform, UnityEngine.Vector3 newPosition)
			{
				transform.localPosition = newPosition;
			}

			[HarmonyLib.HarmonyPostfix]
			static void Postfix(DiskCardGame.Part1BossOpponent __instance)
			{
				var sceneryObj = __instance.sceneryObject;

				if (__instance is DiskCardGame.ProspectorBossOpponent)
				{
					Plugin.Log.LogDebug(
						$"[{__instance.GetType()}] ForestTableEffects has been spawned, scene object is [{sceneryObj}]");

					foreach (var pineTree in TreesToModifyPositiveZedPosition)
					{
						// ForestTableEffects -> GetChild(1) == SurroundingTrees
						UnityEngine.Transform pineTreeObj = sceneryObj.transform.GetChild(1).GetChild(pineTree);
						UnityEngine.Vector3 newPos = new UnityEngine.Vector3(0f, 0f, pineTree == 4 ? -2f : 2f);
						SetNewPositionForGameObject(pineTreeObj.GetChild(0), newPos);
						SetNewPositionForGameObject(pineTreeObj.GetChild(1), newPos);

						Plugin.Log.LogDebug($"moving tree {pineTree} to new position [{newPos.ToString()}]");
					}
				}
				else if (__instance is DiskCardGame.TrapperTraderBossOpponent)
				{
					Plugin.Log.LogDebug(
						$"[{__instance.GetType()}] KnivesTableEffects has been spawned, scene object is [{sceneryObj}]");

					foreach (var knifeSide in KnivesEachSideIndex)
					{
						// KnivesTableEffects -> GetChild(0) == LeftSide
						// KnivesTableEffects -> GetChild(1) == RightSide

						UnityEngine.Transform knifeSideObj = sceneryObj.transform.GetChild(knifeSide);
						for (int i = 0; i < knifeSideObj.childCount; i++)
						{
							Plugin.Log.LogDebug($"moving Side [{knifeSide}] repeatingConveyorKnife [{i}] back a little...");
							UnityEngine.Transform repeatingConveyorKnife = knifeSideObj.GetChild(i);
							UnityEngine.Vector3 localPosition = repeatingConveyorKnife.localPosition;
							UnityEngine.Vector3 newPosition = new UnityEngine.Vector3(
								localPosition.x - 1f, localPosition.y, localPosition.z);
							SetNewPositionForGameObject(repeatingConveyorKnife, newPosition);
						}
					}
				}
			}
		}

	[HarmonyLib.HarmonyPostfix,
	 HarmonyLib.HarmonyPatch(
		 typeof(DiskCardGame.BoardManager3D),
		 nameof(DiskCardGame.BoardManager3D.TransitionAndResolveCreatedCard)
	 )
	]
	public static void ChangeScaleOfMoonCardToFitAcrossAllSlots(
		DiskCardGame.PlayableCard card, DiskCardGame.CardSlot slot, float transitionLength, bool resolveTriggers = true
	)
	{
		if (card.Info.HasTrait(DiskCardGame.Trait.Giant)
		    && card.Info.specialAbilities.Exists(a => a == SpecialTriggeredAbility.GiantMoon))
		{
			Plugin.Log.LogDebug($"Setting new scaling and position of the moon");
			// Card -> Quad -> CardBase
			UnityEngine.Transform cardBase = card.transform.GetChild(0).GetChild(0);

				UnityEngine.Vector3 localScale = cardBase.localScale;
				cardBase.localScale = new UnityEngine.Vector3(5.75f, localScale.y, localScale.z);
				Plugin.Log.LogDebug($"Successfully set new scaling for the moon");

				UnityEngine.Vector3 localPosition = cardBase.localPosition;
				cardBase.localPosition = new UnityEngine.Vector3(-2.25f, localPosition.y, localPosition.z);
				Plugin.Log.LogDebug($"Successfully set new localPosition for the moon");
			}
		}

		[HarmonyLib.HarmonyPrefix,
		 HarmonyLib.HarmonyPatch(typeof(DiskCardGame.TableRuleBook), nameof(DiskCardGame.TableRuleBook.Awake))]
		public static void ChangeTableRuleBookDefaultPosition(DiskCardGame.TableRuleBook __instance)
		{
			// check for this specific name as we don't want to do anything to the Part 3 Rulebook
			if (__instance.name == "TableRuleBook" && __instance.isActiveAndEnabled)
			{
				Plugin.Log.LogDebug($"Setting new position for TableRuleBook");
				__instance.transform.localPosition = new UnityEngine.Vector3(-4.69f, 0f, -4f);
				Plugin.Log.LogDebug($"Successfully set new position for TableRuleBook");
			}
		}

	[HarmonyLib.HarmonyPrefix,
	 HarmonyLib.HarmonyPatch(typeof(DiskCardGame.CandleHolder), nameof(DiskCardGame.CandleHolder.Awake))]
	public static void ChangeCandleHolderLocalPosition(DiskCardGame.CandleHolder __instance)
	{
		if (SaveManager.SaveFile.IsPart1)
		{
			Plugin.Log.LogDebug($"Setting new position for CandleHolder");
			__instance.transform.localPosition = new UnityEngine.Vector3(6f, -0.006000042f, 1.1f);
			Plugin.Log.LogDebug($"Successfully set new position for CandleHolder");
		}
	}
}
