using System;
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
using System.IO;

public class TrafficMP : Script
{
    int lockdown = 0;
    int blip_color;
    int time_traffic_gen;
    int traffic_blip_config;
    int street_flag;
    Vehicle traffic_veh = null;
    Ped traffic_ped;
    Blip traffic_blip;
    List<Vehicle> veh_dlc_list = new List<Vehicle>();
    ScriptSettings config;

    public TrafficMP()
    {
        Tick += OnTick;
        Aborted += OnAborted;

        config = ScriptSettings.Load("Scripts\\AllMpVehiclesInSp.ini");
        blip_color = config.GetValue<int>("MAIN", "blip_color_traffic", 3);
        traffic_blip_config = config.GetValue<int>("MAIN", "traffic_cars_blips", 0);
        street_flag = config.GetValue<int>("MAIN", "spawn_traffic", 1);
        time_traffic_gen = config.GetValue<int>("MAIN", "time_traffic_gen", 8000);

        if (time_traffic_gen < 3000)
        {
            time_traffic_gen = 3000;
        }
    }

    public string SelectRandmDLC(Vehicle car)
    {
        var lsSpawn = new Random();
        var mdSpawn = new Random();
        string model_name = null;
        int vclass = Function.Call<int>(Hash.GET_VEHICLE_CLASS, car);

        if (vclass > 13)
        {
            vclass = lsSpawn.Next(0, 13);
        }

        switch (vclass)
        {
            case 0:
                model_name = VehList.models_compacts[mdSpawn.Next(VehList.models_compacts.Count)];
                break;
            case 1:
                model_name = VehList.models_sedans[mdSpawn.Next(VehList.models_sedans.Count)];
                break;
            case 2:
                model_name = VehList.models_suvs[mdSpawn.Next(VehList.models_suvs.Count)];
                break;
            case 3:
                model_name = VehList.models_coupes[mdSpawn.Next(VehList.models_coupes.Count)];
                break;
            case 4:
                model_name = VehList.models_muscle[mdSpawn.Next(VehList.models_muscle.Count)];
                break;
            case 5:
                model_name = VehList.models_sportclassic[mdSpawn.Next(VehList.models_sportclassic.Count)];
                break;
            case 6:
                model_name = VehList.models_sportclassic[mdSpawn.Next(VehList.models_sportclassic.Count)];
                break;
            case 7:
                model_name = VehList.models_supers[mdSpawn.Next(VehList.models_supers.Count)];
                break;
            case 8:
                model_name = VehList.models_motorcycles[mdSpawn.Next(VehList.models_motorcycles.Count)];
                break;
            case 9:
                model_name = VehList.models_offroad[mdSpawn.Next(VehList.models_offroad.Count)];
                break;
            case 10:
                model_name = VehList.models_industrial[mdSpawn.Next(VehList.models_industrial.Count)];
                break;
            case 11:
                model_name = VehList.models_industrial[mdSpawn.Next(VehList.models_industrial.Count)];
                break;
            case 12:
                model_name = VehList.models_vans[mdSpawn.Next(VehList.models_vans.Count)];
                break;
            case 13:
                model_name = VehList.models_cycles[mdSpawn.Next(VehList.models_cycles.Count)];
                break;
        }

        return model_name;
    }

    public Vehicle FindOriginalVehicle()
    {
        Vehicle[] veh_list = World.GetNearbyVehicles(Game.Player.Character, 50.0f);

        foreach (Vehicle car in veh_list)
        {
            if (!car.IsOnScreen && car.Position.DistanceTo(Game.Player.Character.Position) > 30.0f && car.Speed > 0.0)
            {
                return car;
            }
        }
        return null;
    }

    public Vehicle SpawnVehicle(string modelString, Vector3 pos, float heading)
    {
        Vehicle veh = null;
        var rdVehicle = new Random();
        var model = new Model(modelString);
        model.Request(250);
        while (!model.IsLoaded)
        {
            Script.Wait(0);
        }

        if (model.IsInCdImage && model.IsValid)
        {
            veh = World.CreateVehicle(model, pos, heading);
            return veh;
        }
        return null;
    }

    public Ped SetDrive(Vehicle car)
    {
        Ped street_driver = Function.Call<Ped>(Hash.CREATE_RANDOM_PED_AS_DRIVER, car, true);
        Function.Call(Hash.SET_VEHICLE_ENGINE_ON, car, true, true, false);
        street_driver.Task.CruiseWithVehicle(car, 10.0f, DrivingStyle.Normal);
        car.Speed = 10.0f;
        return street_driver;
    }

    Blip CreateMarkerAboveCar(Vehicle car)
    {
        Blip mark = Function.Call<Blip>(GTA.Native.Hash.ADD_BLIP_FOR_ENTITY, car);
        Function.Call(GTA.Native.Hash.SET_BLIP_SPRITE, mark, 1);
        Function.Call(GTA.Native.Hash.SET_BLIP_COLOUR, mark, blip_color);
        Function.Call(GTA.Native.Hash.FLASH_MINIMAP_DISPLAY);
        mark.Name = "Unique vehicle";
        return mark;
    }

    private void OnAborted(object sender, EventArgs e)
    {
        if (traffic_blip != null && traffic_blip.Exists())
        {
            traffic_blip.Delete();
        }
    }

    private void OnTick(object sender, EventArgs e)
    {
        if (street_flag == 1 && !Function.Call<bool>(Hash.GET_MISSION_FLAG))
        {
            if (lockdown == 0 || Game.GameTime > lockdown)
            {
                Vehicle temp_veh = null;
                while (temp_veh == null)
                {
                    Script.Wait(0);
                    temp_veh = FindOriginalVehicle();

                }
                Vector3 pos = temp_veh.Position;
                float heading = temp_veh.Heading;

                string model_name = SelectRandmDLC(temp_veh);

                if (traffic_blip != null && traffic_blip.Exists())
                {
                    traffic_blip.Delete();
                }

                traffic_veh = null;
                while (traffic_veh == null)
                {
                    Script.Wait(0);
                    traffic_veh = SpawnVehicle(model_name, pos, heading);
                }
                veh_dlc_list.Add(traffic_veh);
                traffic_ped = SetDrive(traffic_veh);

                if (traffic_blip_config == 1)
                {
                    traffic_blip = CreateMarkerAboveCar(traffic_veh);
                }

                foreach (Vehicle car in veh_dlc_list)
                {
                    if (car.Exists())
                    {
                        if (car.Position.DistanceTo(Game.Player.Character.Position) > 150.0f || car.IsSeatFree(VehicleSeat.Driver))
                        {
                            Ped ped = car.Driver;
                            if (ped != null && ped.Exists())
                            {
                                ped.Delete();
                            }
                            car.Delete();
                        }
                    }
                }

                lockdown = Game.GameTime + 8000;
            }
        }
    }
}