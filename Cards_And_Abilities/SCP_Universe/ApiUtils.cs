using System.Reflection;
using APIPlugin;
using DiskCardGame;
using Sirenix.Utilities;
using UnityEngine;
using static SCP_Universe.SCP_Plugin;

namespace SCP_Universe;

public static class ApiUtils
{
	public static AbilityInfo CreateInfoWithDefaultSettings(
		string rulebookName, string rulebookDescription, bool activated
	)
	{
		AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
		info.powerLevel = 0;
		info.activated = activated;
		// Pascal split will make names like "AreaOfEffectStrike" => "Area Of Effect Strike"
		// "Possessive" => "Possessive"
		info.rulebookName = rulebookName.Contains(" ") ? rulebookName : rulebookName.SplitPascalCase();
		// Log.LogDebug($"[CreateAbility] Rulebook name is [{info.rulebookName}]");
		info.rulebookDescription = rulebookDescription;
		info.metaCategories = new List<AbilityMetaCategory>()
		{
			AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part1Rulebook
		};

		return info;
	}

	public static NewAbility CreateAbility<T>(
		string rulebookDescription,
		string rulebookName = null,
		bool activated = false
	) where T : AbilityBehaviour
	{
		rulebookName ??= typeof(T).Name;
		string nameToLookFor = "ability_" + typeof(T).Name;
		return CreateAbility<T>(
			CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, activated),
			AllAbilityAssets.Single(
				t => string.Equals(t.name, nameToLookFor, StringComparison.OrdinalIgnoreCase)
			)
		);
	}

	private static NewAbility CreateAbility<T>(
		AbilityInfo info,
		Texture texture
	) where T : AbilityBehaviour
	{
		Type type = typeof(T);
		// instantiate
		var newAbility = new NewAbility(
			info, type, texture, GetAbilityId(info.rulebookName)
		);

		// Get static field
		FieldInfo field = type.GetField("ability",
			BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance
		);
		// GrimoraPlugin.Log.LogDebug($"Setting static field [{field.Name}] for [{type}] with value [{newAbility.ability}]");
		field.SetValue(null, newAbility.ability);

		return newAbility;
	}

	public static AbilityIdentifier GetAbilityId(string rulebookName)
	{
		return AbilityIdentifier.GetID(PluginGuid, rulebookName);
	}
}
