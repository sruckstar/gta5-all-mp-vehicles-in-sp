# gta5-all-mp-vehicles-in-sp
The mod adds all the vehicles from GTA Online to GTA 5. The new cars can be found in parking lots throughout Los Santos and Blaine County and on the road in general traffic.

- All content from GTA Online is added, including air and water vehicles, except vehicles from previous updates that are available in Single Player (SP).
- Over 100 new vehicle spawn points.
- You may come across a DLC car in traffic. Each class of car can only be found on certain roads and sections of road.
- Parking lot vehicle spawns are constantly changing.
- All military vehicles from GTA Online (including both versions of the Oppressor) will be added to the military base at Fort Zancudo.
- Specific spawn points are chosen for different types of specialized vehicles to justify their presence in the game's lore.

Join my Discord server: https://discord.gg/vvxmKP5y5J

# Add-On vehicles in traffic and parking lots
Since version 2.0.5 you can add Add-On vehicles to traffic and parking lots. 

Open the NewVehiclesList.txt file and add a new line:
SpawnModelName,class

For example: gstghell1,muscle

It is mandatory to capitalize the vehicle class as shown in the following example: boats, commercial, compacts, coupes, cycles, emergency, helicopters, industrial, karting, military, motorcycles, muscle, openwheel, offroad, planes, sedans, service, sports, sportsclassics, super, suvs, vans.

If the author did not specify which class the add-on vehicle belongs to, choose the class that seems most appropriate to you. 
This setting does not affect the performance of the car. It is necessary for correct spawning of cars in the following locations.

# Adding cars to the blacklist
Starting with version 2.0.0 you can add cars to the blacklist. In this case they will not appear in parking lots and traffic. You will still be able to get these cars with a trainer or other mods.
Open the text file mp_blacklist.txt and add the names of the DLC cars you do not want to see in traffic or parking to the column.

# Installation

1. Download [ScriptHookV](http://dev-c.com/gtav/scripthookv/)
2. Download [ScriptHookVDotNet](https://github.com/scripthookvdotnet/scripthookvdotnet/releases/latest)
3. Move the `scripts` folder into your main GTAV folder (press _Replace the files in the destination_ if Windows asks you to).

# Script Settings

You can edit the `AllMpVehiclesInSp.ini` file (located in your `scripts` folder) to modify the script settings.

0 - disable
1 - enable

- doors - enable locking the parked DLC vehicle's doors. If enabled, the player will have to break a window or lockpick it (depending on the vehicle) to enter.
- blips - enable or disable parked DLC car blips on the map.
- tuning - if enabled, spawned DLC cars will receive random tuning items.
- spawn_traffic - enable or disable spawning of DLC cars in traffic.
- traffic_cars_blips - enable or disable blips of DLC cars in traffic.
