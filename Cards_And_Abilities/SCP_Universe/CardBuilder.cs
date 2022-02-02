using APIPlugin;
using DiskCardGame;
using UnityEngine;
using static SCP_Universe.SCP_Plugin;

namespace GrimoraMod;

public class CardBuilder
{
	private readonly CardInfo _cardInfo = ScriptableObject.CreateInstance<CardInfo>();

	private CardBuilder()
	{
	}

	public static CardBuilder Builder => new();

	public CardInfo Build()
	{
		if (_cardInfo.metaCategories.Contains(CardMetaCategory.Rare))
		{
			_cardInfo.appearanceBehaviour = CardUtils.getRareAppearance;
		}

		return _cardInfo;
	}

	internal CardBuilder SetTribes(Tribe tribes)
	{
		return SetTribes(new List<Tribe>() { tribes });
	}

	internal CardBuilder SetTribes(List<Tribe> tribes)
	{
		_cardInfo.tribes = tribes;
		return this;
	}

	private CardBuilder SetPortrait(string cardName)
	{
		cardName = cardName.Replace("scp_universe_", "");
		_cardInfo.portraitTex = AllSpriteAssets.Single(
			spr => spr.name.Equals(cardName, StringComparison.OrdinalIgnoreCase)
		);

		// TODO: refactor when API 2.0 comes out

		return this;
	}

	private CardBuilder SetAltPortrait(string cardName)
	{
		cardName = cardName.Replace("scp_universe_", "");
		// Log.LogDebug($"Looking in AllSprites for [{cardName}]");
		_cardInfo.portraitTex = AllSpriteAssets.Single(
			spr => spr.name.Equals(cardName + "_alt", StringComparison.OrdinalIgnoreCase)
		);

		// TODO: refactor when API 2.0 comes out

		return this;
	}

	internal CardBuilder SetBoneCost(int bonesCost)
	{
		_cardInfo.bonesCost = bonesCost;
		return this;
	}

	internal CardBuilder SetBloodCost(int bloodCost)
	{
		_cardInfo.cost = bloodCost;
		return this;
	}

	internal CardBuilder SetBaseAttackAndHealth(int baseAttack, int baseHealth)
	{
		_cardInfo.baseAttack = baseAttack;
		_cardInfo.baseHealth = baseHealth;
		return this;
	}

	internal CardBuilder SetNames(string name, string displayedName)
	{
		_cardInfo.name = name;
		_cardInfo.displayedName = displayedName;

		return SetPortrait(name);
	}

	internal CardBuilder SetAsNormalCard()
	{
		return SetMetaCategories(CardUtils.getNormalCardMetadata);
	}

	internal CardBuilder SetAsRareCard()
	{
		return SetMetaCategories(CardMetaCategory.Rare);
	}

	internal CardBuilder SetDescription(string description)
	{
		_cardInfo.description = description;
		return this;
	}

	internal CardBuilder SetMetaCategories(CardMetaCategory category)
	{
		return SetMetaCategories(new List<CardMetaCategory>() { category });
	}

	internal CardBuilder SetMetaCategories(List<CardMetaCategory> categories)
	{
		_cardInfo.metaCategories = categories;
		return this;
	}

	internal CardBuilder SetAbilities(Ability ability)
	{
		return SetAbilities(new List<Ability>() { ability });
	}

	internal CardBuilder SetAbilities(Ability ability1, Ability ability2)
	{
		return SetAbilities(new List<Ability>() { ability1, ability2 });
	}

	internal CardBuilder SetAbilities(Ability ability1, Ability ability2, Ability ability3)
	{
		return SetAbilities(new List<Ability>() { ability1, ability2, ability3 });
	}

	internal CardBuilder SetAbilities(List<Ability> abilities)
	{
		_cardInfo.abilities = abilities;
		return this;
	}

	internal CardBuilder SetAbilities(SpecialTriggeredAbility ability)
	{
		return SetAbilities(new List<SpecialTriggeredAbility>() { ability });
	}

	internal CardBuilder SetAbilities(SpecialTriggeredAbility ability1, SpecialTriggeredAbility ability2)
	{
		return SetAbilities(new List<SpecialTriggeredAbility>() { ability1, ability2 });
	}

	internal CardBuilder SetAbilities(List<SpecialTriggeredAbility> abilities)
	{
		_cardInfo.specialAbilities = abilities;
		return this;
	}

	internal CardBuilder SetTraits(Trait trait)
	{
		return SetTraits(new List<Trait>() { trait });
	}

	internal CardBuilder SetTraits(List<Trait> traits)
	{
		_cardInfo.traits = traits;
		return this;
	}

	internal CardBuilder SetDecals(Texture decal)
	{
		return SetDecals(new List<Texture>() { decal });
	}

	internal CardBuilder SetDecals(List<Texture> decals)
	{
		_cardInfo.decals = decals;
		return this;
	}
}
