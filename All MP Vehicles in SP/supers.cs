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

public class supers : Script
{
    ScriptSettings config;
    private int doors_config = 0;
    private int blip_config = 0;
    private int spawned = 0;
    private int x = 0;
    private float distance = 100.0f;
    private Blip marker;

    private Vector3[] coords = new Vector3[7];
    private float[] angle = new float[7];
    private GTA.Vehicle car;
    private VehicleHash[] models = new VehicleHash[43];

    public supers()
    {
        config = ScriptSettings.Load("Scripts\\AllMpVehiclesInSp.ini");
        doors_config = config.GetValue<int>("MAIN", "doors", 1);
        blip_config = config.GetValue<int>("MAIN", "blips", 1);

        coords[0] = new Vector3(-1297.2f, 252.495f, 61.813f); 
        coords[1] = new Vector3(-345.267f, 662.299f, 168.587f); 
        coords[2] = new Vector3(-72.605f, 902.579f, 234.631f); 
        coords[3] = new Vector3(-1451.92f, 533.495f, 118.177f); 
        coords[4] = new Vector3(-1979.25f, 586.078f, 116.479f); 
        coords[5] = new Vector3(-1873.6f, -343.933f, 48.26f);
        coords[6] = new Vector3(443.542f, 253.197f, 102.21f); 
        angle[0] = 181.166f;
        angle[1] = 171.211f;
        angle[2] = 291.351f;
        angle[3] = 73.674f;
        angle[4] = 185.087f;
        angle[5] = 225.300f;
        angle[6] = 245.845f;

        models[0] = VehicleHash.Virtue;
        models[1] = VehicleHash.EntityMT;
        models[2] = VehicleHash.LM87;
        models[3] = VehicleHash.Torero2;
        models[4] = VehicleHash.Zeno;
        models[5] = VehicleHash.Ignus;
        models[6] = VehicleHash.Champion;
        models[7] = VehicleHash.Tigon;
        models[8] = VehicleHash.Furia;
        models[9] = VehicleHash.Zorrusso;
        models[10] = VehicleHash.Krieger;
        models[11] = VehicleHash.Emerus;
        models[12] = VehicleHash.S80;
        models[13] = VehicleHash.Thrax;
        models[14] = VehicleHash.Deveste;
        models[15] = VehicleHash.Tyrant;
        models[16] = VehicleHash.Tezeract;
        models[17] = VehicleHash.Taipan;
        models[18] = VehicleHash.EntityXXR;
        models[19] = VehicleHash.Autarch;
        models[20] = VehicleHash.SC1;
        models[21] = VehicleHash.Cyclone;
        models[22] = VehicleHash.Visione;
        models[23] = VehicleHash.XA21;
        models[24] = VehicleHash.Vagner;
        models[25] = VehicleHash.GP1;
        models[26] = VehicleHash.ItaliGTB;
        models[27] = VehicleHash.ItaliGTB2;
        models[28] = VehicleHash.Nero2;
        models[29] = VehicleHash.Nero;
        models[30] = VehicleHash.Tempesta;
        models[31] = VehicleHash.Penetrator;
        models[32] = VehicleHash.Tyrus;
        models[33] = VehicleHash.RE7B;
        models[34] = VehicleHash.Sheava;
        models[35] = VehicleHash.Pfister811;
        models[36] = VehicleHash.Prototipo;
        models[37] = VehicleHash.Reaper;
        models[38] = VehicleHash.FMJ;
        models[39] = VehicleHash.SultanRS;
        models[40] = VehicleHash.Banshee2;
        models[41] = VehicleHash.T20;
        models[42] = VehicleHash.Osiris;

        car = null;
        spawned = 0;
        Tick += OnTick;
    }

    void OnTick(object sender, EventArgs e)
    {
        {
            Vector3 fix_coords = new Vector3(0.0f, 0.0f, 0.0f);
            var position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));

            for (int i = 0; i <= 6; i++)
            {
                if (spawned == 0 && Function.Call<bool>(Hash.IS_PLAYER_SWITCH_IN_PROGRESS) == false)
                {
                    if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, coords[i].X, coords[i].Y, coords[i].Z, position.X, position.Y, position.Z, 0) < distance)
                    {
                        Random rnd = new Random();
                        var veh_model = new Model(models[rnd.Next(0, 43)]);
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