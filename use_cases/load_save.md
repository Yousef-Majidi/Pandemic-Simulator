# Loading a save from the game (Ian)
## Actor (Player)
The player playing the game
## Pre-conditions
The player must already have made a save in the game i.e must already have progressed in the game 
## Main Flow
1. Player clicks on load game on the main menu 
2. The player clicks on a save based on a list of previously saved games
3. The system prompts players if they are sure they want to load the save
4. The player clicks on yes
5. Systems loads and sets all the saved game information from the save game.save file
6. The game is displayed as it was when the to save was created
## Alternate Flow
1. The player makes an in-session load (load a save file when they are currently in a game), and the system loads up that save information from that file
2. The Player chooses not to load the file and returns to the main menu
## Postconditions
The player is playing their game from the saved state
