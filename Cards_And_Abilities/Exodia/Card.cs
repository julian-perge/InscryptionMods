using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using SigilADay_julianperge;
using UnityEngine;

namespace Exodia
{
	public partial class Plugin
	{
		public const string Name = "Exodia";

		public static void AddExodiaCards()
		{
			AddCardExodia();
		}

		private static void AddCardExodia()
		{
			// find the ability since it will be created in SigilADay
			NewAbility ability = NewAbility.abilities.Find(ab => ab.ability == SigilADay_julianperge.Exodia.ability);
			List<CardMetaCategory> metaCategories = CardUtils.getNormalCardMetadata;

			Texture2D defaultTexture = SigilUtils.LoadTextureFromResource(Properties.Resources.card_exodia);

			var displayName = "The Forbidden One";
			var desc = "WHAT, THAT'S NOT POSSIBLE?!";
			var abIds = new List<AbilityIdentifier>() { ability.id };

			NewCard.Add(Name, displayName, 1, 1,
				metaCategories, CardComplexity.Simple, CardTemple.Nature,
				desc, bloodCost: 1, defaultTex: defaultTexture, abilityIdsParam: abIds, onePerDeck: true
			);
		}
	}
}
