using System.Collections.Generic;
using APIPlugin;
using CardLoaderPlugin.lib;
using DiskCardGame;
using UnityEngine;

namespace AddAllSCP.SCP_049_Plague_Doctor
{
	public static class Card
	{
		public static void InitCard()
		{
			List<CardMetaCategory> metaCategories = CardUtils.getRareCardMetadata;

			List<Ability> abilities = new List<Ability> { DoubleDeathTweaked.ability };

			byte[] defaultTextureBytes =
				System.IO.File.ReadAllBytes("BepInEx/plugins/CardLoader/Artwork/scp_049_small.png");
			Texture2D defaultTexture = new Texture2D(2, 2);
			defaultTexture.LoadImage(defaultTextureBytes);

			NewCard.Add("SCP_049_PlagueDoctor", 
				metaCategories, 
				CardComplexity.Advanced, CardTemple.Undead, 
				"Plague Doctor",
				0, 2,
				"I wouldn't get near him. The skin is known to be poisonous on contact.",
				bonesCost: 6, tex: defaultTexture, abilities: abilities);
		}
	}
}
