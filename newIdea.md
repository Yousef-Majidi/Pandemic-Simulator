# Group_02

This project aims to create a 3D simulation that will show how a virus spreads through a population, and the effects of puplic health measures on the spread.
The environment will consist of x number of `Residents`.
Each `Resident` will have a two visible attributes: `Health` and `Stamina`. 
They will reside in `Homes` where they will regain their `Stamina`. Once full, they will try to either go to a `Shop` or `Workplace`/`School`. Once drained, they will go back to their `Home`.
They will also have hidden attributes such as `Cough Rate`, `Sweat Rate`, `Health loss`, `Fatigue`. These hidden attributes will be affected by the virus by the means of multipliers for each attribute.
The virus will siginificantly reduce the resident's `Health`. Once lower than a threashold, the resident will try to visit the `Hospital`. If unable, and `Health` reaches 0, they will die.
Everytime the virus is spread, it will have the exact same multipliers, except with a chance of mutation to increase or decrease the multipliers. This will simulation the evolution of a virus and emergence of different variants.

The player in this simulation is playing the role of _The Mayor_ of this town and can make certain decisions. These decisions will include, mandatory mask wearing, curfew, quarantine and other public health measures.
Each of these decisions will affect the spread in unique ways. However, each decision is associated with a cost of `Action Points` (perhaps represented as `Budget` or `Political Power`) and the number of `AP` the player spends on each decision will determine the severity of the action in fighting the spread.
The data from this simulation will be presented to the user in the form of graphs and tables.

The goal is to make a fun, interactive simulation that can also be used as an educational tool to showcase the spread of a viral desease, and at the same time, the effects of public health measures that we all have had to live with for the past two years. 

This game will be created using the Unity game engine and written in C#.

| Project Role | Student |
| ----------- |  ----------- |
| Team Lead | Yousef|
| TBD | Yousef|
| TBD | Suhail|
| TBD | Ian|
