using System;
using System.Collections.Generic;
using System.Linq;
using APIPlugin;
using CardLoaderPlugin.lib;
using DiskCardGame;
using UnityEngine;

namespace MoreAnts
{
	public class DomeAnt : VariableStatBehaviour
	{
		public static SpecialStatIcon specialStatIcon;

		public override SpecialStatIcon IconType { get { return specialStatIcon; } }

		public override int[] GetStatValues()
		{
			Console.WriteLine("[DomeAnt] GetStatValues called with DomeAnt");

			List<CardSlot> list = base.PlayableCard.Slot.IsPlayerSlot
				? Singleton<BoardManager>.Instance.PlayerSlotsCopy
				: Singleton<BoardManager>.Instance.OpponentSlotsCopy;

			int num = list
				.Where(slot => slot.Card is not null)
				.Count(cardSlot => cardSlot.Card.Info.HasTrait(Trait.Ant));

			int[] array = new int[2];
			array[1] = num;
			return array;
		}

		public static StatIconInfo InitIconAndAbility()
		{
			StatIconInfo info = ScriptableObject.CreateInstance<StatIconInfo>();
			info.iconType = (SpecialStatIcon)8;
			info.appliesToAttack = false;
			info.appliesToHealth = true;
			info.rulebookName = "Ants (Defense)";
			info.rulebookDescription =
				"The value represented with this sigil will be equal to the number of Ants that the owner has on their side of the table. DEFENSE";
			info.metaCategories = new() { AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part1Rulebook };

			var defaultTex = new Texture2D(2, 2);
			byte[] imgBytes = System.IO.File.ReadAllBytes("BepInEx/plugins/CardLoader/Artwork/ability_drawant.png");
			bool isLoaded = defaultTex.LoadImage(imgBytes);
			defaultTex.LoadImage(imgBytes);

			var sId = SpecialAbilityIdentifier.GetID(MoreAnts.PluginGuid, info.rulebookName);

			var domeAntSpecAbility = new NewSpecialAbility(info, typeof(DomeAnt), defaultTex, sId);
			specialStatIcon = domeAntSpecAbility.statIconInfo.iconType;

			return info;
		}

		public static void InitCard()
		{
			StatIconInfo info = DomeAnt.InitIconAndAbility();

			var defaultTex = new Texture2D(2, 2);
			byte[] imgBytes = System.IO.File.ReadAllBytes("BepInEx/plugins/CardLoader/Artwork/dome_ant.png");
			bool isLoaded = defaultTex.LoadImage(imgBytes);
			defaultTex.LoadImage(imgBytes);

			List<CardMetaCategory> metaCategories = CardUtils.getNormalCardMetadata;

			string name = "DomeAnt";
			string displayedName = "Dome Ant";
			string descryption = "Loves to guard his friends";

			EvolveParams evolveParams = new() { turnsToEvolve = 1, evolution = CardLoader.GetCardByName("AntQueen") };
			List<Tribe> tribes = new() { Tribe.Insect };
			List<Trait> traits = new() { Trait.Ant };
			List<SpecialTriggeredAbility> abilities = new() { (SpecialTriggeredAbility)26 };

			NewCard.Add(
				name, metaCategories, CardComplexity.Advanced, CardTemple.Nature,
				displayedName, 0, 1, descryption,
				evolveParams: evolveParams, cost: 1, tex: defaultTex,
				specialStatIcon: (SpecialStatIcon)8, specialAbilities: abilities,
				tribes: tribes, traits: traits
			);
		}
	}
}
