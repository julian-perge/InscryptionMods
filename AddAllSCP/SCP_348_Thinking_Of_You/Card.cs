using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace AddAllSCP.SCP_348_Thinking_Of_You
{
	public static class Card
	{
		public const string Name = "SCP_348_Thinking_Of_You";

		public static void InitCard()
		{
			NewAbility ability = ThinkingOfYouAbility.InitAbility();
			List<CardMetaCategory> metaCategories = CardUtils.getNormalCardMetadata;
			
			Texture2D defaultTexture =
				CardUtils.getAndloadImageAsTexture("BepInEx/plugins/CardLoader/Artwork/scp_348_small.png");

			var displayName = "Thinking Of You";
			var desc =
				"Those who eat from SCP-348 several times often express a feeling of contentment, stating that though they are eating by themselves, they do not feel lonely.";
			var abIds = new List<AbilityIdentifier>() { ability.id };

			NewCard.AddToPool(Name, displayName, 0, 1,
				metaCategories, CardComplexity.Simple, CardTemple.Nature,
				desc, bloodCost: 1, defaultTex: defaultTexture, abilityIdsParam: abIds
			);
		}
	}
}
