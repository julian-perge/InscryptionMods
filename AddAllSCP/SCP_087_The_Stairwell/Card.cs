using System.Collections.Generic;
using System.IO;
using APIPlugin;
using CardLoaderPlugin.lib;
using DiskCardGame;
using UnityEngine;

namespace AddAllSCP.SCP_087_The_Stairwell
{
	public static class Card
	{
		public static void InitCard()
		{
			List<CardMetaCategory> metaCategories = CardUtils.getNormalCardMetadata;

			List<Ability> abilities = new List<Ability> { TheStairwellAbility.ability };

			Texture2D defaultTexture =
				CardUtils.getAndloadImageAsTexture("BepInEx/plugins/CardLoader/Artwork/scp_087_small.png");

			string name = "SCP_087_TheStairwell";
			string displayName = "The Stairwell";
			string desc =
				"Subjects report and audio recordings confirm the distressed vocalizations from what is presumed to be a child between the ages of █ and ██";

			NewCard.AddToPool(name, displayName, 0, 6,
				metaCategories, CardComplexity.Simple, CardTemple.Nature,
				desc, 2, defaultTexture: defaultTexture, abilities: abilities
			);
		}
	}
}
