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

public class sportclassic : Script
{
    ScriptSettings config;
    private int doors_config = 0;
    private int blip_config = 0;
    private int tuning_flag = 0;
    private int[] mode_type = new int[5];
    private int spawned = 0;
    private int x = 0;
    private int all_coords;
    private float distance = 150.0f;
    private Blip marker;

    private Vector3[] coords = new Vector3[8];
    private float[] angle = new float[8];
    private GTA.Vehicle car;
    private VehicleHash[] models = new VehicleHash[84];

    public sportclassic()
    {
        config = ScriptSettings.Load("Scripts\\AllMpVehiclesInSp.ini");
        doors_config = config.GetValue<int>("MAIN", "doors", 1);
        blip_config = config.GetValue<int>("MAIN", "blips", 1);
        tuning_flag = config.GetValue<int>("MAIN", "tuning", 1);

        coords[0] = new Vector3(-1114.1f, 479.205f, 81.161f); 
        coords[1] = new Vector3(-161.232f, 274.911f, 92.534f);
        coords[2] = new Vector3(-504.323f, 424.21f, 96.287f); 
        coords[3] = new Vector3(-1405.12f, 81.983f, 52.099f); 
        coords[4] = new Vector3(-1299.92f, -228.464f, 59.654f);
        coords[5] = new Vector3(-1334.63f, -1008.97f, 6.867f);
        coords[6] = new Vector3(-187.144f, -175.854f, 42.624f); 
        coords[7] = new Vector3(-1886.25f, 2016.572f, 139.951f); 
        all_coords = 7;

        angle[0] = 171.220f;
        angle[1] = 272.080f;
        angle[2] = 313.167f;
        angle[3] = 53.145f;
        angle[4] = 126.968f;
        angle[5] = 118.474f;
        angle[6] = 160.257f;
        angle[7] = 174.739f;

        models[0] = VehicleHash.Everon2;
        models[1] = VehicleHash.Panthere;
        models[2] = VehicleHash.R300;
        models[3] = VehicleHash.Sentinel4;
        models[4] = VehicleHash.TenF2;
        models[5] = VehicleHash.TenF;
        models[6] = VehicleHash.SM722;
        models[7] = VehicleHash.OmniseGT;
        models[8] = VehicleHash.Corsita;
        models[9] = VehicleHash.Comet7;
        models[10] = VehicleHash.Cypher;
        models[11] = VehicleHash.Sultan3;
        models[12] = VehicleHash.Growler;
        models[13] = VehicleHash.Vectre;
        models[14] = VehicleHash.Comet6;
        models[15] = VehicleHash.Remus;
        models[16] = VehicleHash.Jester4;
        models[17] = VehicleHash.RT3000;
        models[18] = VehicleHash.ZR350;
        models[19] = VehicleHash.Euros;
        models[20] = VehicleHash.Futo2;
        models[21] = VehicleHash.ItaliRSX;
        models[22] = VehicleHash.Penumbra2;
        models[23] = VehicleHash.Coquette4;
        models[24] = VehicleHash.Sugoi;
        models[25] = VehicleHash.VStr;
        models[26] = VehicleHash.Sultan2;
        models[27] = VehicleHash.Imorgon;
        models[28] = VehicleHash.Komoda;
        models[29] = VehicleHash.Jugular;
        models[30] = VehicleHash.Zion3;
        models[31] = VehicleHash.Locust;
        models[32] = VehicleHash.Nebula;
        models[33] = VehicleHash.Neo;
        models[34] = VehicleHash.Issi7;
        models[35] = VehicleHash.Drafter;
        models[36] = VehicleHash.Paragon;
        models[37] = VehicleHash.Paragon2;
        models[38] = VehicleHash.Schlagen;
        models[39] = VehicleHash.ItaliGTO;
        models[40] = VehicleHash.Swinger;
        models[41] = VehicleHash.Jester3;
        models[42] = VehicleHash.Michelli;
        models[43] = VehicleHash.FlashGT;
        models[44] = VehicleHash.GB200;
        models[45] = VehicleHash.HotringSabre;
        models[46] = VehicleHash.Comet5;
        models[47] = VehicleHash.Z190;
        models[48] = VehicleHash.Neon;
        models[49] = VehicleHash.Revolter;
        models[50] = VehicleHash.GT500;
        models[51] = VehicleHash.Viseris;
        models[52] = VehicleHash.Savestra;
        models[53] = VehicleHash.Streiter;
        models[54] = VehicleHash.Sentinel3;
        models[55] = VehicleHash.Raiden;
        models[56] = VehicleHash.Pariah;
        models[57] = VehicleHash.Comet4;
        models[58] = VehicleHash.RapidGT3;
        models[59] = VehicleHash.Retinue;
        models[60] = VehicleHash.Ardent;
        models[61] = VehicleHash.Torero;
        models[62] = VehicleHash.Cheetah2;
        models[63] = VehicleHash.Turismo2;
        models[64] = VehicleHash.Infernus2;
        models[65] = VehicleHash.Ruston;
        models[66] = VehicleHash.Specter2;
        models[67] = VehicleHash.Specter;
        models[68] = VehicleHash.Comet3;
        models[69] = VehicleHash.Elegy;
        models[70] = VehicleHash.Tampa2;
        models[71] = VehicleHash.Lynx;
        models[72] = VehicleHash.Tropos;
        models[73] = VehicleHash.Omnis;
        models[74] = VehicleHash.Seven70;
        models[75] = VehicleHash.BestiaGTS;
        models[76] = VehicleHash.Mamba;
        models[77] = VehicleHash.Verlierer2;
        models[78] = VehicleHash.Schafter3;
        models[79] = VehicleHash.Schafter4;
        models[80] = VehicleHash.Feltzer3;
        models[81] = VehicleHash.Casco;
        models[82] = VehicleHash.Kuruma;
        models[83] = VehicleHash.Kuruma2;
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
                        var veh_model = new Model(models[rnd.Next(0, 84)]);
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
                            for (int a = 0; i <= 3; i++)
                            {
                                mode_type[i] = rnd.Next(0, 17);
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
                if (GTA.Native.Function.Call<bool>(GTA.Native.Hash.IS_PED_IN_VEHICLE, Game.Player.Character, car, true))
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