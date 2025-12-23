# UNO Game Project

A simple console-based implementation of the popular UNO card game in C#.

---

## Overview

This project simulates a game of UNO for 2 to 10 players. The game features:

- Standard UNO deck with **108 cards**:
  - Number cards (0–9) in four colors: Red, Blue, Green, Yellow
  - Special colored cards: Skip, Reverse, Draw Two
  - Wild cards: Wild, Wild Draw Four
- Turn-based gameplay following UNO rules
- Direction changes and special card effects
- Automatic handling of empty decks by reshuffling the discard pile
- Game ends when:
  - A player has no cards left (wins)
  - All cards are in players' hands (winner determined by fewest cards)

---

## Features

- **Colored terminal output**:
  - Number cards are printed in their color
  - Special cards are displayed using initials: `S` = Skip, `R` = Reverse, `+2` = Draw Two, `W` = Wild, `+4` = Wild Draw Four
- Player hands displayed in one line with the count of cards
- Card effects (Skip, Reverse, Draw Two, Wild, Wild Draw Four) applied automatically
