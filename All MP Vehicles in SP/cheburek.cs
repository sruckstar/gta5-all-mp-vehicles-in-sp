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

public class Cheburek : Script
{
    ScriptSettings config;
    private int doors_config = 0;
    private int blip_config = 0;
    private int tuning_flag = 0;
    private int[] mode_type = new int[5];
    private int spawned = 0;
    private int x = 0;
    private float distance = 150.0f;
    private Blip marker;
    private Blip street_marker;

    private int street_flag = 1;
    private int street_spawned = 0;
    private float street_angle = 0.0f;
    private float street_speed = 40.0f;
    private Ped street_driver;
    private Vehicle street_car;
    private Vector3 street_coords = Vector3.Zero;
    private int street_blip = 0;

    private static int cars_number = 3; // To add a new vehicle, change this number

    private Vector3[] coords = new Vector3[1];
    private float[] angle = new float[1];
    private GTA.Vehicle car;
    private int all_coords;
    private VehicleHash[] models = new VehicleHash[cars_number];

    public Cheburek()
    {
        config = ScriptSettings.Load("Scripts\\AllMpVehiclesInSp.ini");
        doors_config = config.GetValue<int>("MAIN", "doors", 1);
        blip_config = config.GetValue<int>("MAIN", "blips", 1);
        tuning_flag = config.GetValue<int>("MAIN", "tuning", 1);
        street_flag = config.GetValue<int>("MAIN", "spawn_traffic", 1);
        street_blip = config.GetValue<int>("MAIN", "traffic_cars_blips", 0);
        coords[0] = new Vector3(1546.591f, 3781.791f, 33.06f);
        all_coords = 0;

        angle[0] = 26.557f;

        models[0] = VehicleHash.Cheburek;
        models[1] = VehicleHash.RatBike;
        models[2] = VehicleHash.SlamVan3;

        car = null;
        spawned = 0;
        Tick += OnTick;
    }

    public enum Nodetype
    {
        AnyRoad,
        Road,
        Offroad,
        Water
    }

    void GenerateSpawnPos(Vector3 desiredPos, Nodetype roadtype)
    {
        bool ForceOffroad = false;
        OutputArgument outArgA = new OutputArgument();
        OutputArgument outArgB = new OutputArgument();
        int NodeNumber = 1;
        int type = 0;

        if (roadtype == Nodetype.AnyRoad)
        {
            type = 1;
        }
        if (roadtype == Nodetype.Road)
        {
            type = 0;
        }
        if (roadtype == Nodetype.Offroad)
        {
            type = 1;
            ForceOffroad = true;
        }
        if (roadtype == Nodetype.Water)
        {
            type = 3;
        }

        int NodeID = Function.Call<int>(Hash.GET_NTH_CLOSEST_VEHICLE_NODE_ID_WITH_HEADING, desiredPos.X, desiredPos.Y, desiredPos.Z, NodeNumber, outArgA, outArgB, type, 300f, 300f);
        if (ForceOffroad)
        {
            while (!Function.Call<bool>(Hash.GET_VEHICLE_NODE_IS_SWITCHED_OFF, NodeID) && NodeNumber < 500)
            {
                NodeNumber++;
                NodeID = Function.Call<int>(Hash.GET_NTH_CLOSEST_VEHICLE_NODE_ID_WITH_HEADING, desiredPos.X, desiredPos.Y, desiredPos.Z, NodeNumber, outArgA, outArgB, type, 300f, 300f);
            }
        }

        Function.Call(Hash.GET_VEHICLE_NODE_POSITION, NodeID, outArgA);
        street_coords = outArgA.GetResult<Vector3>();
        street_angle = outArgB.GetResult<float>();
        Random rnd = new Random();
        var veh_model = new Model(models[rnd.Next(0, cars_number)]);
        veh_model.Request(500);
        while (!veh_model.IsLoaded) Script.Wait(100);
        street_car = World.CreateVehicle(veh_model, street_coords, street_angle);
        Function.Call(Hash.DECOR_SET_INT, car, "MPBitset", 0);
        street_spawned = 1;

        street_driver = Function.Call<Ped>(Hash.CREATE_RANDOM_PED_AS_DRIVER, street_car, true);
        street_driver.Task.CruiseWithVehicle(street_car, street_speed, DrivingStyle.Normal);

        if (street_blip == 1)
        {
            street_marker = GTA.Native.Function.Call<Blip>(GTA.Native.Hash.ADD_BLIP_FOR_ENTITY, street_car);
            GTA.Native.Function.Call(GTA.Native.Hash.SET_BLIP_SPRITE, street_marker, 1);
            GTA.Native.Function.Call(GTA.Native.Hash.SET_BLIP_COLOUR, street_marker, 3);
            GTA.Native.Function.Call(GTA.Native.Hash.FLASH_MINIMAP_DISPLAY);
            street_marker.Name = "Unique vehicle";
        }
    }

    void OnAborded(object sender, EventArgs e)
    {
        //Delete blips
        if (marker != null && marker.Exists())
        {
            marker.Delete();
        }
        if (street_marker != null && street_marker.Exists())
        {
            street_marker.Delete();
        }
        //Delete peds
        if (street_driver != null && street_driver.Exists())
        {
            street_driver.Delete();
        }
        //Delete_cars
        if (car != null && car.Exists() && !Function.Call<bool>(Hash.IS_PED_SITTING_IN_VEHICLE, Game.Player.Character, car))
        {
            car.Delete();
        }
        if (street_car != null && street_car.Exists() && !Function.Call<bool>(Hash.IS_PED_SITTING_IN_VEHICLE, Game.Player.Character, street_car))
        {
            street_car.Delete();
        }
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
                        var veh_model = new Model(models[rnd.Next(0, cars_number)]);
                        veh_model.Request(500);
                        while (!veh_model.IsLoaded) Script.Wait(100);
                        car = World.CreateVehicle(veh_model, coords[i], angle[i]);
                        Function.Call(Hash.DECOR_SET_INT, car, "MPBitset", 0);
                        spawned = 1;

                        if (street_flag == 1 && street_spawned == 0)
                        {
                            GenerateSpawnPos(coords[i], Nodetype.Road);
                            street_spawned = 1;
                        }

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

                        if (tuning_flag == 1)
                        {
                            rnd = new Random();
                            int num;
                            int modindex;
                            for (int a = 0; a <= 3; a++)
                            {
                                mode_type[a] = rnd.Next(0, 17);
                                num = Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, car, mode_type[a]);
                                if (num != -1)
                                {
                                    modindex = rnd.Next(0, num + 1);
                                    Function.Call(Hash.SET_VEHICLE_MOD, car, mode_type[a], modindex, true);
                                }
                            }
                            if (Function.Call<bool>(Hash.IS_THIS_MODEL_A_BIKE, veh_model))
                            {
                                num = Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, car, 24);
                                modindex = rnd.Next(0, num + 1);
                                Function.Call(Hash.SET_VEHICLE_MOD, car, 24, modindex, true);
                            }
                            else
                            {
                                num = Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, car, 23);
                                modindex = rnd.Next(0, num + 1);
                                Function.Call(Hash.SET_VEHICLE_MOD, car, 23, modindex, true);
                            }
                            int choose = rnd.Next(1, 3);
                            if (choose == 1)
                            {
                                num = Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, car, 48);
                                if (num != -1)
                                {
                                    modindex = rnd.Next(0, num + 1);
                                    Function.Call(Hash.SET_VEHICLE_MOD, car, 48, modindex, true);
                                }
                            }
                            else
                            {
                                modindex = rnd.Next(0, 7);
                                num = Function.Call<int>(Hash.GET_NUM_MOD_COLORS, 6, true);
                                int color_1 = rnd.Next(0, num + 1);
                                int color_2 = rnd.Next(0, num + 1);
                                Function.Call(Hash.SET_VEHICLE_MOD_COLOR_1, car, modindex, color_1, 0);
                                Function.Call(Hash.SET_VEHICLE_MOD_COLOR_2, car, modindex, color_2, 0);
                            }
                        }

                        veh_model.MarkAsNoLongerNeeded();
                        x = i;
                        break;
                    }
                }
            }

            //If the player gets in the car, we clear it from memory
            if (car != null)
            {
                if (GTA.Native.Function.Call<bool>(GTA.Native.Hash.IS_PED_IN_VEHICLE, Game.Player.Character, car, false))
                {
                    if (blip_config == 1)
                    {
                        marker.Delete();
                    }
                    car.MarkAsNoLongerNeeded();
                    car = null;
                    position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));
                }
            }
            //Similarly for the car from traffic
            if (street_car != null)
            {
                if (GTA.Native.Function.Call<bool>(GTA.Native.Hash.IS_PED_IN_VEHICLE, Game.Player.Character, street_car, false))
                {
                    if (blip_config == 1)
                    {
                        marker.Delete();
                    }
                    street_car.MarkAsNoLongerNeeded();
                    street_car = null;
                    position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));
                }
            }
            //If the car is deleted and the player has left the stream zone, you can spawn a new car.
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
            //Position and old_position coordinates are used for cars from traffic. 
            //old_position has static coordinates, position changes every 100 ms.
            if (street_car == null && street_spawned == 1)
            {
                position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));
                Vector3 old_position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));
                while (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, old_position.X, old_position.Y, old_position.Z, position.X, position.Y, position.Z, 0) < distance)
                {
                    Script.Wait(100);
                    position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));
                }
                street_spawned = 0;
            }
            //If the car exists but the player is far away, delete the car
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
            //Similarly for the car from traffic
            if (street_spawned == 1 && street_car != null)
            {
                position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));
                if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, street_car.Position.X, street_car.Position.Y, street_car.Position.Z, position.X, position.Y, position.Z, 0) > 500)
                {
                    if (marker != null)
                    {
                        marker.Delete();
                    }

                    if (street_driver != null)
                    {
                        street_driver.Delete();
                    }

                    street_car.Delete();
                    street_car = null;
                }
            }
        }
    }
}