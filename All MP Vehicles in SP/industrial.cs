﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Native;
using GTA.Math;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

public class industrial : Script
{
    ScriptSettings config;
    private int doors_config = 0;
    private int blip_config = 0;
    private int spawned = 0;
    private int x = 0;
    private float distance = 150.0f;
    private Blip marker;

    private Vector3[] coords = new Vector3[4];
    private float[] angle = new float[4];
    private GTA.Vehicle car;
    private int all_coords;
    private VehicleHash[] models = new VehicleHash[7];

    public industrial()
    {
        config = ScriptSettings.Load("Scripts\\AllMpVehiclesInSp.ini");
        doors_config = config.GetValue<int>("MAIN", "doors", 1);
        blip_config = config.GetValue<int>("MAIN", "blips", 1);

        coords[0] = new Vector3(1566.097f, -1683.17f, 87.205f); 
        coords[1] = new Vector3(2673.478f, 1678.569f, 23.488f); 
        coords[2] = new Vector3(839.097f, 2202.196f, 50.46f);
        coords[3] = new Vector3(2717.772f, 1391.725f, 23.535f); 
        all_coords = 3;

        angle[0] = 14.900f;
        angle[1] = 270.297f;
        angle[2] = 245.553f;
        angle[3] = 26.547f;

        models[0] = VehicleHash.Pounder2;
        models[1] = VehicleHash.Mule4;
        models[2] = VehicleHash.Phantom3;
        models[3] = VehicleHash.Hauler2;
        models[4] = VehicleHash.Phantom2;
        models[5] = VehicleHash.Mule3;
        models[6] = VehicleHash.Boxville4;

        car = null;
        spawned = 0;
        Tick += OnTick;
    }

    void OnTick(object sender, EventArgs e)
    {
        {
            Vector3 fix_coords = new Vector3(0.0f, 0.0f, 0.0f);
            var position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));

            for (int i = 0; i <= all_coords; i++)
            {
                if (spawned == 0 && Function.Call<bool>(Hash.IS_PLAYER_SWITCH_IN_PROGRESS) == false)
                {
                    if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, coords[i].X, coords[i].Y, coords[i].Z, position.X, position.Y, position.Z, 0) < distance)
                    {
                        Random rnd = new Random();
                        var veh_model = new Model(models[rnd.Next(0, 7)]);
                        veh_model.Request(500);
                        while (!veh_model.IsLoaded) Script.Wait(100);
                        car = World.CreateVehicle(veh_model, coords[i], angle[i]);
                        Function.Call(Hash.DECOR_SET_INT, car, "MPBitset", 0);
                        spawned = 1;

                        if (blip_config == 1)
                        {
                            marker = GTA.Native.Function.Call<Blip>(GTA.Native.Hash.ADD_BLIP_FOR_ENTITY, car);
                            GTA.Native.Function.Call(GTA.Native.Hash.SET_BLIP_SPRITE, marker, 1);
                            GTA.Native.Function.Call(GTA.Native.Hash.SET_BLIP_COLOUR, marker, 3);
                            GTA.Native.Function.Call(GTA.Native.Hash.FLASH_MINIMAP_DISPLAY);
                            marker.Name = "Unique vehicle";
                        }

                        if (doors_config == 1)
                        {
                            GTA.Native.Function.Call(GTA.Native.Hash.SET_VEHICLE_DOORS_LOCKED, car, 7);
                        }

                        veh_model.MarkAsNoLongerNeeded();
                        x = i;
                        break;
                    }
                }
            }

            
            if (car != null)
            {
                if (GTA.Native.Function.Call<bool>(GTA.Native.Hash.IS_PED_IN_VEHICLE, Game.Player.Character, car, false))
                {
                    if (doors_config == 1)
                    {
                        GTA.Native.Function.Call(GTA.Native.Hash.SET_VEHICLE_ALARM, car, true);
                        GTA.Native.Function.Call(GTA.Native.Hash.START_VEHICLE_ALARM, car);
                    }

                    if (blip_config == 1)
                    {
                        marker.Delete();
                    }

                    car.MarkAsNoLongerNeeded();
                    car = null;
                    position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));
                }
            }

            if (car == null && spawned == 1)
            {
                position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));
                while (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, coords[x].X, coords[x].Y, coords[x].Z, position.X, position.Y, position.Z, 0) < distance)
                {
                    Script.Wait(100);
                    position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));
                }
                spawned = 0;
            }


            if (spawned == 1 && car != null) 
            {
                position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));
                if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, coords[x].X, coords[x].Y, coords[x].Z, position.X, position.Y, position.Z, 0) > distance)
                {
                    if (blip_config == 1)
                    {
                        marker.Delete();
                    }
                    car.Delete();
                    car = null;
                }
            }
        }
    }
}