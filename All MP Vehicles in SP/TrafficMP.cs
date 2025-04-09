
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
    int vehicles_spawned = 0;
    int lockdown = 0;
    int blip_color;
    int time_traffic_gen;
    int traffic_blip_config;
    int street_flag;
    Vehicle traffic_veh = null;
    Ped traffic_ped;
    Blip traffic_blip;
    List<Vehicle> veh_dlc_list = new List<Vehicle>();
    List<Ped> ped_dlc_list = new List<Ped>();
    ScriptSettings config;
    int debugging = 0;
    const int MAX_VEHICLES = 10;
    const float REMOVE_DISTANCE = 200.0f; // Дистанция удаления

    public TrafficMP()
    {
        Tick += OnTick;
        Aborted += OnAborted;

        Function.Call(Hash.REQUEST_IPL, "m23_1_garage");

        config = ScriptSettings.Load("Scripts\\AllMpVehiclesInSp.ini");
        blip_color = config.GetValue<int>("MAIN", "blip_color_traffic", 3);
        traffic_blip_config = config.GetValue<int>("MAIN", "traffic_cars_blips", 0);
        street_flag = config.GetValue<int>("MAIN", "spawn_traffic", 1);
        time_traffic_gen = config.GetValue<int>("MAIN", "time_traffic_gen", 8000);
        debugging = config.GetValue<int>("MAIN", "show_errors", 0);

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

    public static bool IsEmergencyVehicle(Vehicle vehicle)
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

    private bool IsDriverCop(Ped ped)
    {
        return Function.Call<int>(Hash.GET_PED_TYPE, ped) == 6;
    }

    public Vehicle FindOriginalVehicle()
    {
        Vehicle[] veh_list = World.GetNearbyVehicles(Game.Player.Character, 50.0f);

        foreach (Vehicle car in veh_list)
        {
            if (!car.IsOnScreen &&
                !veh_dlc_list.Contains(car) &&
                !IsEmergencyVehicle(car) &&
                car.Position.DistanceTo(Game.Player.Character.Position) > 30.0f &&
                Function.Call<int>(Hash.GET_ENTITY_POPULATION_TYPE, car) != 7 &&
                car.Driver != null &&
                car.Driver.IsAlive &&
                !IsDriverCop(car.Driver) &&
                Function.Call<bool>(Hash.IS_THIS_MODEL_A_CAR, car.Model.Hash)
                )
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
        if (!model.IsValid)
        {
            return null;
        }
        else
        {
            while (!model.IsLoaded) Script.Wait(0);

            veh = World.CreateVehicle(model, pos, heading);
            while (veh == null) Script.Wait(0);
            Function.Call(Hash.SET_ENTITY_COLLISION, veh, false, false);

            foreach (Vehicle v in World.GetAllVehicles())
            {
                if (v != veh && v.Position.DistanceTo(veh.Position) <= 2.0f)
                {
                    Ped p = v.Driver;
                    v.Delete();
                    p.Delete();

                }
            }
            Function.Call(Hash.SET_ENTITY_COLLISION, veh, true, true);
            return veh;
        }
    }

    public Ped SetDrive(Vehicle car, int ped_hash, int ped_type)
    {
        var model = new Model(ped_hash);
        model.Request(250);
        while (!model.IsLoaded)
        {
            Script.Wait(0);
        }

        if (model.IsInCdImage && model.IsValid)
        {
            Ped street_driver = Function.Call<Ped>(Hash.CREATE_PED_INSIDE_VEHICLE, car, ped_type, model, -1, false, true);
            Function.Call(Hash.SET_VEHICLE_ENGINE_ON, car, true, true, false);
            street_driver.Task.CruiseWithVehicle(car, 10.0f, DrivingStyle.Normal);
            car.Speed = 10.0f;
            return street_driver;
        }

        return null;
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
        //GTA.UI.Screen.ShowSubtitle("Автомобилей в трафике: " + veh_dlc_list.Count);

        if (vehicles_spawned == 1 && (Function.Call<bool>(Hash.GET_MISSION_FLAG) || Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING)))
        {
            foreach (Vehicle car in veh_dlc_list)
            {
                if (car.Exists())
                {
                    Ped ped = car.Driver;
                    if (ped != null && ped.Exists())
                    {
                        ped.Delete();
                    }
                    car.Delete();
                }
            }

            veh_dlc_list.Clear();
            vehicles_spawned = 0;
        }

        if (street_flag == 1 && !(Function.Call<bool>(Hash.GET_MISSION_FLAG) || Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING)))
        {
            if (lockdown == 0 || Game.GameTime > lockdown)
            {
                // Если машин >= MAX_VEHICLES, пробуем удалить дальнюю
                if (veh_dlc_list.Count >= MAX_VEHICLES)
                {
                    Vehicle farthestCar = null;
                    float maxDistance = REMOVE_DISTANCE;

                    foreach (Vehicle car in veh_dlc_list)
                    {
                        if (car.Exists())
                        {
                            float distance = car.Position.DistanceTo(Game.Player.Character.Position);
                            if (distance > maxDistance)
                            {
                                maxDistance = distance;
                                farthestCar = car;
                            }
                        }
                    }

                    // Если нашли машину дальше 200 метров — удаляем
                    if (farthestCar != null)
                    {
                        Ped ped = farthestCar.Driver;
                        if (ped != null && ped.Exists()) ped.Delete();
                        farthestCar.Delete();
                        veh_dlc_list.Remove(farthestCar);
                    }
                    else
                    {
                        return; // Если нет машин дальше 200м, новых не спавним
                    }
                }

                Vehicle temp_veh = null;
                while (temp_veh == null)
                {
                    Script.Wait(0);
                    temp_veh = FindOriginalVehicle();
                }

                Vector3 pos = temp_veh.Position;
                float heading = temp_veh.Heading;
                Ped driver = temp_veh.Driver;
                var ped_model = driver.Model.Hash;
                int type = Function.Call<int>(Hash.GET_PED_TYPE, driver);

                string model_name = SelectRandmDLC(temp_veh);

                if (traffic_blip != null && traffic_blip.Exists())
                {
                    traffic_blip.Delete();
                }

                traffic_veh = SpawnVehicle(model_name, pos, heading);
                if (traffic_veh == null) return;

                veh_dlc_list.Add(traffic_veh);
                traffic_ped = SetDrive(traffic_veh, ped_model, type);
                ped_dlc_list.Add(traffic_ped);

                if (traffic_blip_config == 1)
                {
                    traffic_blip = CreateMarkerAboveCar(traffic_veh);
                }

                vehicles_spawned = 1;

                foreach (Vehicle car in veh_dlc_list.ToArray())
                {
                    if (car.Exists())
                    {
                        if (car.Driver == Game.Player.Character)
                        {
                            veh_dlc_list.Remove(car);
                        }
                    }
                }

                lockdown = Game.GameTime + 8000;
            }
        }
    }
}