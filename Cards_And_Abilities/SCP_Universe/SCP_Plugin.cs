using System.Reflection;
using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using Sirenix.Utilities;
using UnityEngine;

namespace SCP_Universe;

[BepInEx.BepInPlugin(PluginGuid, PluginName, PluginVersion)]
[BepInEx.BepInDependency(CyantistInscryptionAPI)]
public partial class SCP_Plugin : BepInEx.BaseUnityPlugin
{
	public const string CyantistInscryptionAPI = "cyantist.inscryption.api";

	public const string PluginGuid = "julianperge.inscryption.scpUniverse";
	public const string PluginName = "scp_universe_SCP_Universe";
	public const string PluginVersion = "0.1.0";

	public static Sprite[] AllSpriteAssets;
	public static Texture[] AllAbilityAssets;

	internal static BepInEx.Logging.ManualLogSource Log;

	private static Harmony _harmony;

	public const bool EnableHotReload = true;

	private void Awake()
	{
		Log = base.Logger;

		_harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);

		CheckForHotReload();

		LoadAssets();

		// TODO: Need to mimic DiskCardGame.Transformer/DiskCardGame.Evolve-like transformations;
		Add_Scp034_RitualKnife();

		// WORKS
		Add_Scp035_PorcelainMask();

		// WORKS
		Add_Scp049_PlagueDoctor();

		// WORKS
		Add_Scp087_TheStairwell();

		// TODO: Still needs to implement following card if card has strafe
		// Add_Scp096_And_Transforms();

		// WORKS
		Add_Scp348_ThinkingOfYou();

		// WORKS
		Add_Scp354_BloodPond_And_Creatures();

		// WORKS
		Add_Scp999_TickleMonster();
	}

	private void OnDestroy()
	{
		_harmony?.UnpatchSelf();
	}

	public static void CheckForHotReload()
	{
		if (EnableHotReload)
		{
			Log.LogDebug($"[SCP] Is in hot reload scripts folder");
			if (!CardLoader.allData.IsNullOrEmpty())
			{
				bool IsScpCard(CardInfo card) => card.name.StartsWith("scp_universe");

				NewCard.cards.RemoveAll(IsScpCard);
				int removed = CardLoader.allData.RemoveAll(IsScpCard);
				Log.LogDebug($"All data is not null, concatting GrimoraMod cards. Removed [{removed}] cards.");
				CardLoader.allData = CardLoader.allData.Concat(NewCard.cards.Where(IsScpCard))
					.Distinct()
					.ToList();
			}

			if (!AbilitiesUtil.allData.IsNullOrEmpty())
			{
				Log.LogDebug($"All data is not null, concatting GrimoraMod abilities");
				AbilitiesUtil.allData.RemoveAll(info =>
					NewAbility.abilities.Exists(na => na.id.ToString().StartsWith(PluginGuid) && na.ability == info.ability));
				NewAbility.abilities.RemoveAll(ab => ab.id.ToString().StartsWith(PluginGuid));

				AbilitiesUtil.allData = AbilitiesUtil.allData
					.Concat(
						NewAbility.abilities.Where(ab => ab.id.ToString().StartsWith(PluginGuid)).Select(_ => _.info)
					)
					.ToList();
			}
		}
	}

	private static void LoadAssets()
	{
		Log.LogDebug($"Loading asset bundles");

		AssetBundle abilityBundle = AssetBundle.LoadFromFile(FileUtils.FindFileInPluginDir("scp_universe_abilities"));
		AssetBundle spritesBundle = AssetBundle.LoadFromFile(FileUtils.FindFileInPluginDir("scp_universe_sprites_cards"));

		AllAbilityAssets = abilityBundle.LoadAllAssets<Texture>();
		abilityBundle.Unload(false);
		// Log.LogDebug($"{string.Join(",", AllAbilityAssets.Select(_ => _.name))}");

		AllSpriteAssets = spritesBundle.LoadAllAssets<Sprite>();
		spritesBundle.Unload(false);
		// Log.LogDebug($"{string.Join(",", AllSpriteAssets.Select(_ => _.name))}");
	}
}
