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

public class Coupes : Script
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

    private Vector3[] coords = new Vector3[7];
    private float[] angle = new float[7];
    private GTA.Vehicle car;
    private int all_coords;
    private VehicleHash[] models = new VehicleHash[5];

    public Coupes()
    {
        config = ScriptSettings.Load("Scripts\\AllMpVehiclesInSp.ini");
        doors_config = config.GetValue<int>("MAIN", "doors", 1);
        blip_config = config.GetValue<int>("MAIN", "blips", 1);
        tuning_flag = config.GetValue<int>("MAIN", "tuning", 1);

        coords[0] = new Vector3(-74.575f, -619.874f, 35.173f); 
        coords[1] = new Vector3(283.769f, -342.644f, 43.92f); 
        coords[2] = new Vector3(-1044.02f, -2608.02f, 19.775f); 
        coords[3] = new Vector3(-801.566f, -1313.92f, 4.0f); 
        coords[4] = new Vector3(-972.578f, -1464.27f, 4.013f); 
        coords[5] = new Vector3(1309.942f, -530.154f, 70.312f); 
        coords[6] = new Vector3(339.481f, 159.143f, 102.146f); 
        all_coords = 6;

        angle[0] = 341.667f;
        angle[1] = 66.978f;
        angle[2] = 66.226f;
        angle[3] = 169.408f;
        angle[4] = 294.730f;
        angle[5] = 341.133f;
        angle[6] = 71.345f;

        models[0] = VehicleHash.KanjoSJ;
        models[1] = VehicleHash.Postlude;
        models[2] = VehicleHash.Previon;
        models[3] = VehicleHash.Windsor2;
        models[4] = VehicleHash.Windsor;

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
                        var veh_model = new Model(models[rnd.Next(0, 5)]);
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