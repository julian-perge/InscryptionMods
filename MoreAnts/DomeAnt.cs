using System;
using System.Collections.Generic;
using DiskCardGame;

namespace MoreAnts
{
	public class DomeAnt : Ant
	{
		public override int[] GetStatValues()
		{
			Console.WriteLine("GetStatValues called with DomeAnt");
			List<CardSlot> list = base.PlayableCard.Slot.IsPlayerSlot ? Singleton<BoardManager>.Instance.PlayerSlotsCopy : Singleton<BoardManager>.Instance.OpponentSlotsCopy;
			int num = 0;
			foreach (CardSlot cardSlot in list)
			{
				if (cardSlot.Card != null && cardSlot.Card.Info.HasTrait(Trait.Ant))
				{
					num++;
				}
			}
			int[] array = new int[2];
			array[1] = num;
			return array;
		}
	}
}
