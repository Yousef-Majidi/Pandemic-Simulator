
# Project Charter

## Introduction 

- Project name: TBD
- Current version: 0.1
- Date: 14-June-2022
- Sponsor: n/a
- Project manager: Yousef 

### Summary: 

The game is presented in an isometric view where the player can see their city and NPCs (Non Playable Characters) moving around the environment. The city will consist of different types of buildings that have different uses. The NPCs will interact with the buildings based on their needs.

The main view will also contain a dashboard which will show pinned stats. The player can customize their dashboard to decide which stats to be displayed. A button on the dashboard will open up a panel which will contain different tabs. One tab will display the stats in a tabular format, another tab will display them in graphs, and another tab will be decision tab. 

The decisions that the player can make, will each have a negative and a positive effect. For example, the player can make a decision to enact a mask mandate which will drasticially reduce the spread, but decreases happiness of the citizens. The positives will be mainly related to the spread rate of the virus, and the negatives will affect happiness. These actions will cost the player certain amount of action points, and the overall happiness of the city will determine the action point regeneration rate. 

The main challenge of the game is to manage the viral spread and the overall happiness of the town. If the spread is unchecked, citizens will get sick, hospitals will overflow with capacity, and citizens will start to expire. If the overall happiness reaches 0, the citizens will impeach you and you lose the game. These are two lose conditions of the game. The win condition is when the player successfully eliminates the virus by enacting decisions. 

## Overview 

This project aims to create a pandemic simulation game where you take control of a city's government and make decisions to defend your community against an evolving virus.

## Milestones
- [ ] Decide which method to be utilized for implementing saving the game state
- [ ] Define a list of all classes and their interactions with each other
- [ ] Define a list of decisions the players can make within the game
- [ ] Design a general layout of the UI and the panels the user can interact with. This includes the decision tab and the statistics tab
- [ ] A working prototype of the game without using assets to showcase core mechanics. This will consist of temporary basic shapes used as placeholders for the assets with only two buildings implemented; the houses & the workplace. Other buildings to be added in future milestones
- [ ] Finalize the decision on which assets to use and list their sources
- [ ] Replace placeholder assets with the chosen ones (alpha 1)
- [ ] Implement the virus and start tweaking spread rates
- [ ] Introduce the hospital building and health decay rate (alpha 2)
- [ ] Implement the economy mechanic (alpha 3)
- [ ] Introduce the stat bar
- [ ] Introduce the decisions tab including all the decisions (alpha 4)
- [ ] Introduce shops and schools (and student citizens)
- [ ] Include environment assets (beta 1)
- [ ] Start balancing and debugging
- [ ] Introduce the stat tab which includes graphs and tables from the simulation (beta 2)
- [ ] Final touch ups, debugs, etc.

## Deliverables
- [ ] Alpha 1: Working prototype with basic functionalities. This version will not have the stat bar, the decision panel, or the virus spread. 
- [ ] Alpha 2: Virus spread and hospitals implemented
- [ ] Alpha 3: Economy system is implemented
- [ ] Alpha 4: Stat bar and decisions implemented 
- [ ] Beta 1: Core mechanics and final design implemented. All buildings included + environmental assets.
- [ ] Beta 2: Balancing and optimizing. Also includes stat graphs and tables
- [ ] v1.0: first release
