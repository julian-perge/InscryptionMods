# Installation

To install this plugin you first need to install BepInEx as a mod loader for Inscryption. A guide to do this can be
found [here](https://docs.bepinex.dev/articles/user_guide/installation/index.html#where-to-download-bepinex)

You will also need version 2.0.1+ of the [InscryptionAPI](https://github.com/ScottWilson0903/InscryptionAPI) plugin.

To install this mod, you simply need to put the **HealthForAnts.dll** file in the same folder that the **InscryptionAPI
plugin** exists.

## Using HealthForAnts for your cards

In your project, you must add a reference to the
HealthForAnts.dll. [Example here](https://github.com/julian-perge/InscryptionMods/blob/main/Cards_And_Abilities/AntsTest/AntsTest.cs)

Once you've done that, create a class file and add these lines of code at the top. This will load the HealthForAnts
ability and all the necessary abilities before your card gets loaded:

```c#
[BepInDependency("cyantist.inscryption.api")]
[BepInDependency("julianperge.inscryption.cards.healthForAnts")]
public class NameOfYourClass : BaseUnityPlugin {
// code
}
```

Next, creating a custom Ant card will look something like this:

```c#
[BepInDependency("cyantist.inscryption.api")]
[BepInDependency("julianperge.inscryption.cards.healthForAnts")]
public class AntsTest : BaseUnityPlugin
{
  void Awake()
 {
  const string name = "DomeAnt";
  const string displayedName = "Dome Ant";
  const string descryption = "Loves to guard his friends";

  CardInfo info = InscryptionAPI.Card.CardManager.New(
     name,
     displayedName,
     0,
     1,
     descryption
    )
    .AddSpecialAbilities(HealthForAnts.FullSpecial.Id)
    .AddTraits(Trait.Ant)
    .AddTribes(Tribe.Insect)
    .SetCost(1)
    .SetDefaultPart1Card()
    .SetEvolve("AntQueen", 1)
    .SetPortrait("dome_ant.png")
   ;

  info.specialStatIcon = HealthForAnts.FullStatIcon.Id;
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

---

If you want help debugging you can find me on [Inscryption modding discord](https://discord.gg/QrJEF5Denm) as
xXxStoner420BongMasterxXx.
