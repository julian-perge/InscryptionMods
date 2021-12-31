# Logic

In `Opponent.QueueNewCards`, there is a conditional check to determine whether or not the opponent will queue up new
cards:

```
if (this.NumTurnsTaken < this.TurnPlan.Count) {
	// logic for queueing new cards
}
```

This mod will reset `NumTurnsTaken` back to zero once `NumTurnsTaken` is greater than or equal to `TurnPlan.Count`,
allowing the opponent to forever queue up cards.

## Notes

This does not randomize which set of cards the opponent will play. Meaning, if you play an opponent long enough, you can
memorize which cards come next.
