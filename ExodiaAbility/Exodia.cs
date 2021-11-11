using System;
using System.Collections;
using System.Collections.Generic;
using APIPlugin;
using BepInEx;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace ExodiaAbility
{
	[BepInPlugin("com.julianperge.exodia", "Exodia", "1.0")]
	[BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
	public class ExodiaPatch : BaseUnityPlugin
	{
		private void Awake()
		{
			AddExodia();
		}

		public class ExodiaAbility : DiskCardGame.AbilityBehaviour
		{
			public override Ability Ability
			{
				get
				{
					return ability;
				}
			}

			public static Ability ability;

			public override bool RespondsToResolveOnBoard()
			{
				return true;
			}

			public override IEnumerator OnResolveOnBoard()
			{
				yield return base.PreSuccessfulTriggerSequence();
				yield return new WaitForSeconds(0.25f);
				Singleton<TextDisplayer>.Instance.StartCoroutine(
					Singleton<TextDisplayer>.Instance.ShowThenClear("EXODIA, OBLITERATE!", -0.65f, 0.4f,
						Emotion.Laughter, TextDisplayer.LetterAnimation.WavyJitter, DialogueEvent.Speaker.Bonelord)
				);
				yield return new WaitForSeconds(1f);
				int dmgToWin = Singleton<LifeManager>.Instance.DamageUntilPlayerWin;
				// show damage on scale, taken from Pliers and modified the dmg done
				yield return Singleton<LifeManager>.Instance.ShowDamageSequence(
					dmgToWin,
					1, false, 0.25f,
					ResourceBank.Get<GameObject>("Prefabs/Environment/ScaleWeights/Weight_RealTooth"),
					0.25f);
				yield return new WaitForSeconds(0.25f);
				yield return base.LearnAbility(0f);
				yield break;
			}
		}


		public static NewAbility AddExodia()
		{
			// setup ability
			AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
			info.powerLevel = 0;
			info.rulebookName = "Exodia";
			info.rulebookDescription = "You automatically win from the power of Exodia!";
			info.metaCategories = new List<AbilityMetaCategory>()
			{
				AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part1Rulebook
			};
			
			// get and load artwork
			var imgBytes = System.IO.File.ReadAllBytes("BepInEx/plugins/CardLoader/Artwork/exodia.png");
			Texture2D tex = new Texture2D(2, 2);
			tex.LoadImage(imgBytes);
			
			// set ability to behavior class
			NewAbility ability =  new NewAbility(info, typeof(ExodiaAbility), tex);
			ExodiaAbility.ability = ability.ability;
			
			return ability;
			// new CustomCard("Bullfrog") { abilities = new List<Ability>() { ((Ability)100) } };
		}

		// public static NewSpecialAbility loadExodiaSpecial()
		// {
		// 	// setup ability
		// 	StatIconInfo info = ScriptableObject.CreateInstance<StatIconInfo>();
		// 	info.rulebookName = "Exodia";
		// 	info.rulebookDescription = "You automatically win from the power of Exodia!";
		// 	info.metaCategories = new List<AbilityMetaCategory>()
		// 	{
		// 		AbilityMetaCategory.Part1Rulebook
		// 	};
		// 	
		// 	// get and load artwork
		// 	var imgBytes = System.IO.File.ReadAllBytes("BepInEx/plugins/CardLoader/Artwork/exodia.png");
		// 	Texture2D tex = new Texture2D(2, 2);
		// 	tex.LoadImage(imgBytes);
		// 	
		// 	// set ability to behavior class
		// 	NewSpecialAbility ability =  new NewSpecialAbility(info, typeof(ExodiaAbility));
		// 	
		// 	return ability;
		// }
	}

	public class Exodia : SpecialCardBehaviour
	{
		public override bool RespondsToSacrifice()
		{
			return true;
		}

		public override IEnumerator OnSacrifice()
		{
			this.sacrificeCount++;
			if (this.sacrificeCount >= 2)
			{
				Console.WriteLine($"Exodia was sacrificed [{sacrificeCount}] times!");
				yield return new WaitForSeconds(0.5f);
			}
			yield break;
		}

		private int sacrificeCount;
	}
	
	
}
