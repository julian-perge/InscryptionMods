using System;
using System.Collections.Generic;
using System.IO;
using APIPlugin;
using BepInEx;
using CardLoaderPlugin.lib;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using Logger = HarmonyLib.Tools.Logger;

namespace ThePaleManCard
{
	[BepInPlugin("com.julianperge.thePaleMan", "The Pale Man", "1.0")]
	[BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
	public class Patch : BaseUnityPlugin
	{
		public void Awake()
		{
			var harmony = new Harmony("com.julianperge.thePaleMan");
			harmony.PatchAll();

			AddPaleManAbility();
			AddPaleManCard();
		}

		[HarmonyPatch(typeof(DeckReviewSequencer), nameof(DeckReviewSequencer.OnEnterDeckView))]
		public class AddPaleManToDeckPatch
		{
			[HarmonyPrefix]
			public static void AddPaleMan()
			{
				CardInfo info = CardLoader.GetCardByName("ThePaleMan");
				var currDeck = SaveManager.SaveFile.CurrentDeck;
				if (!currDeck.Cards.Exists(card => card.displayedName == info.displayedName))
				{
					SaveManager.SaveFile.CurrentDeck.Cards.RemoveRange(0, 1);
					SaveManager.SaveFile.CurrentDeck.Cards.Add(info);
				}
			}
		}
		
		public NewAbility AddPaleManAbility()
		{
			// setup ability
			AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
			info.powerLevel = 0;
			info.rulebookName = "The Pale Man";
			info.rulebookDescription =
				"The Pale Man. He may seem harmless, but no one has ever seen his face and survived...";
			info.metaCategories = new List<AbilityMetaCategory>()
			{
				AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part1Rulebook
			};

			// get and load artwork
			var imgBytes = System.IO.File.ReadAllBytes("BepInEx/plugins/CardLoader/Artwork/exodia.png");
			Texture2D tex = new Texture2D(2, 2);
			tex.LoadImage(imgBytes);

			// set ability to behavior class
			NewAbility paleManAbility = new NewAbility(info, typeof(ThePaleMan), tex);
			ThePaleMan.ability = paleManAbility.ability;

			return paleManAbility;
			// new CustomCard("Bullfrog") { abilities = new List<Ability>() { ((Ability)100) } };
		}

		public void AddPaleManCard()
		{
			List<CardMetaCategory> metaCategories = CardUtils.getRareCardMetadata;
			List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = CardUtils.getRareAppearance;

			byte[] imgBytes = System.IO.File.ReadAllBytes("BepInEx/plugins/CardLoader/Artwork/skele2.png");
			Texture2D tex = new Texture2D(2, 2);
			tex.LoadImage(imgBytes);

			byte[] altBytes = System.IO.File.ReadAllBytes("BepInEx/plugins/CardLoader/Artwork/skele1.png");
			Texture2D altTexture = new Texture2D(2, 2);
			altTexture.LoadImage(altBytes);

			NewCard.Add("ThePaleMan", metaCategories, CardComplexity.Simple, CardTemple.Nature, "The Pale Man",
				0, 6,
				description: "The Pale Man. He may seem harmless, but no one has ever seen his face and survived...",
				cost: 1, appearanceBehaviour: appearanceBehaviour, tex: tex, altTex: altTexture,
				abilities: new List<Ability> { ThePaleMan.ability });
		}
	}
}
