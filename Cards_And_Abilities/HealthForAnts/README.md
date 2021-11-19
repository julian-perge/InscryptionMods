## Installation

***Warning:*** Incompatible with thunderstore mod loader

To install this plugin you first need to install BepInEx as a mod loader for Inscryption. A guide to do this can be
found [here](https://docs.bepinex.dev/articles/user_guide/installation/index.html#where-to-download-bepinex)

You will also need version 1.11+ of the [InscryptionAPI](https://github.com/ScottWilson0903/InscryptionAPI) plugin.

To install this mod, you simply need to put the **AntsForHealth** folder in **BepInEx/plugins**.

## Using HealthForAnts for your cards

In your project, you must add a reference to the HealthForAnts.dll.

Once you've done that, create a class file and add these lines of code at the top. This will load the HealthForAnts
ability and all the necessary abilities before your card gets loaded:

```
[BepInDependency("cyantist.inscryption.api")]
[BepInDependency("julianperge.inscryption.cards.healthForAnts")]
public class NameOfYourClass : BaseUnityPlugin {
// code
}
```

Next, creating a custom Ant card will look something like this:

```
[BepInDependency("cyantist.inscryption.api")]
[BepInDependency("julianperge.inscryption.cards.healthForAnts")]
public class AntsTest : BaseUnityPlugin
{
    void Awake()
    {
        Texture2D defaultTex = new Texture2D(2, 2);
        byte[] imgBytes = File.ReadAllBytes("BepInEx/plugins/CardLoader/Artwork/dome_ant.png");
        bool isLoaded = defaultTex.LoadImage(imgBytes);
        defaultTex.LoadImage(imgBytes);

        List<CardMetaCategory> metaCategories = CardUtils.getNormalCardMetadata;

        string name = "DomeAnt";
        string displayedName = "Dome Ant";
        string descryption = "Loves to guard his friends";

        EvolveParams evolveParams = new() { turnsToEvolve = 1, evolution = CardLoader.GetCardByName("AntQueen") };
        List<Tribe> tribes = new() { Tribe.Insect };
        List<Trait> traits = new() { Trait.Ant };

        var antHealthAbility = HealthForAnts.HarmonyInit.antHealthSpecialAbility;
        var sAbIds = new List<SpecialAbilityIdentifier>() { antHealthAbility.id };

        NewCard.Add(
            name, metaCategories, CardComplexity.Advanced, CardTemple.Nature,
            displayedName, 0, 1, descryption,
            evolveParams: evolveParams, cost: 1, tex: defaultTex,
            specialStatIcon: antHealthAbility.statIconInfo.iconType, specialAbilitiesIdsParam: sAbIds,
            tribes: tribes, traits: traits
        );
    }
}
```

## Debugging

The easiest way to check if the plugin is working properly or to debug an error is to enable the console. This can be
done by changing

```
[Logging.Console]
\## Enables showing a console for log output.
\# Setting type: Boolean
\# Default value: false
Enabled = false
```

to

```
[Logging.Console]
\## Enables showing a console for log output.
\# Setting type: Boolean
\# Default value: false
Enabled = true
```

in **Inscryption/BepInEx/Config/BepInEx.cfg**

___

If you want help debugging you can find me on [Inscryption modding discord](https://discord.gg/QrJEF5Denm) as
xXxStoner420BongMasterxXx.
