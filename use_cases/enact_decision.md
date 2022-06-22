
# Opening The Decisions Window (Yousef)

## Actor (Player)

The player playing the game

## Pre-conditions

The game is running and the player has started a new game or loaded a save file

## Main Flow

1. The player clicks on the Decisions button located on the stat bar
2. The system loads all of the scriptable decisions objects and calculates which ones are available to interact with
3. The system opens a new window containing a table with the loaded decisions. Unavailable decisions will be grayed out
4. The player clicks on the desired decision and selects the amount of AP to spend on the action
5. The system calculates the percentage of the population affected by the decision and changes their modifiers based on the chosen decision

## Alternate Flow

N/A

## Postconditions

The decision window opens for the player to interact. The player's decisions will be displayed, but only the available decisions can be clicked on. A certain amount of the population (based on the amount of AP spent) will have different modifiers.  
