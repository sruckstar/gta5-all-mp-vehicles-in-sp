# gta5-all-mp-vehicles-in-sp
The mod adds all the vehicles from GTA Online to GTA 5. The new cars can be found in parking lots throughout Los Santos and Blaine County and on the road in general traffic.

- All content from GTA Online is added, including air and water vehicles, except vehicles from previous updates that are available in Single Player.
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

For example `gstghell1,muscle`

It is mandatory to specify the vehicle class, you can choose from `boats`, `commercial`, `compacts`, `coupes`, `cycles`, `emergency`, `helicopters`, `industrial`, `karting`, `motorcycles`, `muscle`, `openwheel`, `offroad`, `planes`, `sedans`, `service`, `sports`, `sportsclassics`, `super`, `suvs`, `vans`.

If the author did not specify the add-on vehicle's class, choose the class that seems most appropriate to you. 
This setting does not affect the car's performance but is necessary for the correct spawning of cars in specific locations.

# Adding cars to the blacklist
Starting with version 2.0.0 you can add cars to the blacklist. In this case, they will not appear in parking lots and traffic. You will still be able to get these cars with a trainer or other mods.
Open the file `mp_blacklist.txt` and add the names of the DLC cars you do not want to see in traffic or parking to the column.

# Installation

1. Install [ScriptHookV](http://dev-c.com/gtav/scripthookv/)
2. Install [a nightly version of ScriptHookVDotNet](https://github.com/scripthookvdotnet/scripthookvdotnet-nightly/releases)
3. Move the `scripts` folder into your main GTAV folder (press _Replace the files in the destination_ if Windows asks you to).

# Script Settings

You can edit the `AllMpVehiclesInSp.ini` file (located in your `scripts` folder) to modify the script settings.

0 - disable
1 - enable

- `parking_lots_spawn` - spawning of cars in parking lots
- `doors` - enable locking the parked DLC vehicle's doors. If enabled, the player will have to break a window or lockpick it (depending on the vehicle) to enter.
- `blips` - enable or disable parked DLC car blips on the map.
- `tuning` - if enabled, spawned DLC cars will receive random tuning items.
- `spawn_traffic` - enable or disable spawning of DLC cars in traffic.
- `traffic_cars_blips` - enable or disable blips of DLC cars in traffic.
- `new_license_plates` - install unique license plates from the Chop Shop update on cars (may crash the game on earlier versions of the game)
- `blip_color` - set the color of car tags in parking lots (values from 0 to 85, [click here](https://docs.fivem.net/docs/game-references/blips/#blip-colors) to see what numbers are what colors)
- `blip_color_traffic` - set the color of car labels in traffic (values from 0 to 85, [click here](https://docs.fivem.net/docs/game-references/blips/#blip-colors) to see what numbers are what colors)
- `time_traffic_gen` - the time in milliseconds between cars being spawned in traffic (minimum value is 3000, default is 8000)
- `tuning_hsw` - if enabled, HSW vehicles will spawn with their HSW upgrades installed.
- `random_colors` - if enabled, spawned DLC cars will receive a random paint job.
- `max_traffic_vehicles` - how many DLC cars may be in traffic at the same time (1-10, default is 3). Lower this if you get frame drops in dense traffic.
- `show_errors` - enable diagnostic notifications (unknown add-on classes, models that fail to load, traffic spawn errors). Off by default; turn it on before reporting a bug.

## Advanced settings

These live in the `[ADVANCED]` section of the same file. The defaults suit most setups - change them only if you have a reason to.

- `SpawnDistance` - how far ahead of the player DLC traffic is spawned, in metres (default 300). **If DLC cars seem to appear "on the other side of the map", lower this** (50-100 works well).
- `DespawnDistance` - how far from the player a DLC traffic car is removed, in metres (default 500). Keep it comfortably above `SpawnDistance`; around 100 pairs well with a `SpawnDistance` of 50.
- `ClearSpawnArea` - if enabled, existing traffic is cleared out of the spot before a DLC car is placed there. Makes spawns more reliable at the cost of occasionally removing a car in front of you.
- `addon_vehicles_in_traffic` - allow add-on vehicles from `NewVehiclesList.txt` to appear in **traffic** (default 0, off). Add-ons still spawn in parking lots either way. This is off by default because many add-on models ship without complete handling or seat data and can crash the game once the AI tries to drive them - enable it only if your add-ons are known to work.

# Notes and known behaviour

- **The mod is disabled during story missions.** This is intentional: spawning extra vehicles during scripted missions could break them or crash the game. Vehicles return a few seconds after the mission ends.
- **To hide the "Unique vehicle" blips**, set `blips` to 0 for parked cars and `traffic_cars_blips` to 0 for traffic cars.
- **A newer game version than the mod expects is fine.** The mod only warns when your game is genuinely *older* than the version it was built for; a newer game simply may contain vehicles the mod does not list yet.

# Reporting a problem

Before reporting a crash or a missing feature, please check the following - these account for most reports:

1. **Update ScriptHookV and ScriptHookVDotNet.** An outdated ScriptHookVDotNet is the single most common cause of the mod failing to load. Use a current [nightly](https://github.com/scripthookvdotnet/scripthookvdotnet-nightly/releases) build.
2. **Set `show_errors` to 1** in `AllMpVehiclesInSp.ini` and reproduce the problem, so the mod can tell you which model or setting is at fault.
3. **Include your `ScriptHookVDotNet.log` and `ScriptHookV.log`** (both in your main GTA V folder). A crash report without a log usually cannot be acted on.
4. **If you use add-on vehicles,** try setting `addon_vehicles_in_traffic` to 0 and see whether the problem goes away.
