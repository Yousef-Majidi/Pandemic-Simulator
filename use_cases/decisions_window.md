
# Opening The Decisions Window (Yousef)

## Actor (Player)

The player playing the game

## Pre-conditions

The game is running and the player has started a new game or loaded a save file

## Main Flow

1. The player clicks on the Decisions button located on the stat bar
2. The system loads all of the scriptable decisions objects and calculates which ones are available to interact with
3. The system opens a new window containing a table with the loaded decisions. Unavailable decisions will be grayed out

## Alternate Flow

N/A

## Postconditions

The decision window opens for the player to interact. All the decisions the player can make will be displayed, but only the available decisions can be clicked on. 
