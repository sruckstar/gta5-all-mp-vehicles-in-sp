using System;
using System.Collections.Generic;
using GTA;
using GTA.Native;
using GTA.Math;
using System.Windows.Forms;

public class ReplacePoliceCars : Script
{
    private readonly Random _random = new Random();
    private ScriptSettings config;
    private int street_flag;

    public ReplacePoliceCars()
    {
        Tick += OnTick;
        Interval = 1000;

        config = ScriptSettings.Load("Scripts\\AllMpVehiclesInSp.ini");
        street_flag = config.GetValue<int>("MAIN", "spawn_traffic", 1);
    }

    private void OnTick(object sender, EventArgs e)
    {
        if (street_flag == 1)
        {
            var policeVehicles = World.GetNearbyVehicles(Game.Player.Character.Position, 200f, new Model[] { VehicleHash.Police, VehicleHash.Police2, VehicleHash.Police3, VehicleHash.Police4 });

            foreach (var vehicle in policeVehicles)
            {
                if (IsPoliceVehicle(vehicle) && vehicle.Position.DistanceTo(Game.Player.Character.Position) > 100.0f && !vehicle.IsOnScreen)
                {
                    if (_random.NextDouble() < 0.1)
                    {
                        ReplaceVehicle(vehicle, 1);
                    }
                    else
                    {
                        ReplaceVehicle(vehicle, 0);
                    }
                }
            }
        }
    }

    private bool IsPoliceVehicle(Vehicle vehicle)
    {
        return vehicle.Model == VehicleHash.Police ||
               vehicle.Model == VehicleHash.Police2 ||
               vehicle.Model == VehicleHash.Police3 ||
               vehicle.Model == VehicleHash.Police4 ||
               vehicle.Model == VehicleHash.FBI ||
               vehicle.Model == VehicleHash.FBI2 ||
               vehicle.Model == VehicleHash.Policeb ||
               vehicle.Model == VehicleHash.Sheriff ||
               vehicle.Model == VehicleHash.Sheriff2 ||
               vehicle.Model == VehicleHash.Lguard ||
               vehicle.Model == VehicleHash.Ambulance ||
               vehicle.Model == VehicleHash.FireTruck;
    }

    private void SetLivery(Vehicle police)
    {
        Random rnd = new Random();

        if (police.Model.Hash == new Model(VehList.models_police_traffic[0]).Hash)
        {
            string zone = Function.Call<string>(Hash.GET_NAME_OF_ZONE, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z);

            switch (zone)
            {
                case "RGLEN":
                case "Richman":
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_KIT, police, 0);
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD, police, 48, 5, false);
                    break;

                case "AIRP":
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_KIT, police, 0);
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD, police, 48, 6, false);
                    break;

                case "ROCKF":
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_KIT, police, 0);
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD, police, 48, 7, false);
                    break;

                case "DELBE":
                case "DELPE":
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_KIT, police, 0);
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD, police, 48, 8, false);
                    break;

                case "ZQ_UAR":
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_KIT, police, 0);
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD, police, 48, 9, false);
                    break;

                case "ZP_ORT":
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_KIT, police, 0);
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD, police, 48, 10, false);
                    break;

                case "GRAPES":
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_KIT, police, 0);
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD, police, 48, 11, false);
                    break;

                case "PALETO":
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_KIT, police, 0);
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD, police, 48, 12, false);
                    break;

                default:
                    int map_z = Function.Call<int>(Hash.GET_HASH_OF_MAP_AREA_AT_COORDS, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z);
                    if (map_z == -289320599)
                    {
                        Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_KIT, police, 0);
                        Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD, police, 48, rnd.Next(0, 4), false);
                    }
                    else
                    {
                        Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_KIT, police, 0);
                        Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD, police, 48, 4, false);
                    }
                    break;
            }
        }
        else if (police.Model.Hash == new Model("police5").Hash)
        {
            string zone = Function.Call<string>(Hash.GET_NAME_OF_ZONE, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z);

            switch (zone)
            {
                case "RGLEN":
                case "Richman":
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_KIT, police, 0);
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD, police, 48, 9, false);
                    break;

                case "AIRP":
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_KIT, police, 0);
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD, police, 48, 10, false);
                    break;

                case "ROCKF":
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_KIT, police, 0);
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD, police, 48, 11, false);
                    break;

                case "DELBE":
                case "DELPE":
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_KIT, police, 0);
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD, police, 48, 12, false);
                    break;

                case "ZQ_UAR":
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_KIT, police, 0);
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD, police, 48, 13, false);
                    break;

                case "ZP_ORT":
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_KIT, police, 0);
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD, police, 48, 14, false);
                    break;

                case "GRAPES":
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_KIT, police, 0);
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD, police, 48, 15, false);
                    break;

                case "PALETO":
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_KIT, police, 0);
                    Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD, police, 48, 16, false);
                    break;

                default:
                    int map_z = Function.Call<int>(Hash.GET_HASH_OF_MAP_AREA_AT_COORDS, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z);
                    if (map_z == -289320599)
                    {
                        Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_KIT, police, 0);
                        Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD, police, 48, rnd.Next(0, 8), false);
                    }
                    else
                    {
                        Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD_KIT, police, 0);
                        Function.Call(GTA.Native.Hash.SET_VEHICLE_MOD, police, 48, 8, false);
                    }
                    break;
            }
        }
    }

    private void ReplaceVehicle(Vehicle oldVehicle, int mode)
    {
        var oldPosition = oldVehicle.Position;
        var oldHeading = oldVehicle.Heading;
        var oldSirenActive = oldVehicle.IsSirenActive;
        var oldEngineRunning = oldVehicle.IsEngineRunning;
        var mdSpawn = new Random();
        Vehicle newVehicle;

        if (mode == 1)
        {
            newVehicle = World.CreateVehicle(new Model(VehList.models_police_traffic[mdSpawn.Next(VehList.models_police_traffic.Count)]), oldPosition, oldHeading);
        }
        else
        {
            newVehicle = World.CreateVehicle(new Model("police5"), oldPosition, oldHeading);
        }
        while (newVehicle == null) Script.Wait(0);
        SetLivery(newVehicle);
        Function.Call(Hash.SET_ENTITY_COLLISION, newVehicle, false, false);

        newVehicle.IsSirenActive = oldSirenActive;
        newVehicle.IsEngineRunning = oldEngineRunning;

        for (int i = -1; i < oldVehicle.PassengerCount; i++)
        {
            Ped ped = oldVehicle.GetPedOnSeat((VehicleSeat)i);
            if (ped != null)
            {
                ped.SetIntoVehicle(newVehicle, (VehicleSeat)i);
            }
        }

        oldVehicle.MarkAsNoLongerNeeded();
        oldVehicle.Delete();
        Function.Call(Hash.SET_ENTITY_COLLISION, newVehicle, true, true);
    }
}
