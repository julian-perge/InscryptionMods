# Increasing Act 1 Slots

- [Full, non-resized image here](https://i.imgur.com/Liyo0R6.png)
- Now works with KC's mod!

## Objects positions/size to handle new layout (Act 1)

### Table related items

- Bell and Scales: Farther left.
- Candle Holder: Moved farther right.
- Card Draw Piles: Moved farther right.
- Consumable items: Moved to the right to avoid overlapping far\*right slot.
- Sacrifice Tokens: Moved farther right.
- Table RuleBook: Moved left and down.

### Boss related items

- Giant Moon: Scaled to now fit across 5 slots.
- Prospector: Tress on the left were moved farther outward so that the bell and scales could be seen easier.
- Trader/Trapper: Knives were moved outward to avoid clipping into the table.

## Changelog

### 1.3.0

- Added patch for TraderTrapper boss to now queue all 5 slots and not just 4 anymore.
	![All 5 queue slots](https://i.imgur.com/jvwSYtC.png "AllQueueSlots")

### 1.2.0

- Changed default Board and BoardCentered view FOV from 50 to 60 so that the board is easier to view.
- Changed TargetFramework when building the project to net472 from netstandard2.0 due to
	a [BepInEx issue](https://github.com/BepInEx/BepInEx/issues/328)
