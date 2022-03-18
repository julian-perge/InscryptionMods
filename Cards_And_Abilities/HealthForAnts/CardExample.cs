using DiskCardGame;
using InscryptionAPI.Card;

namespace HealthForAnts;

public class CardExample
{
	public static void InitCard()
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
