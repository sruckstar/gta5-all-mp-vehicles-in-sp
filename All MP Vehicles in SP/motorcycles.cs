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

public class Motorcycles : Script
{
    ScriptSettings config;
    private int doors_config = 0;
    private int blip_config = 0;
    private int spawned = 0;
    private int x = 0;
    private float distance = 150.0f;
    private Blip marker;

    private Vector3[] coords = new Vector3[6];
    private float[] angle = new float[6];
    private GTA.Vehicle car;
    private int all_coords;
    private VehicleHash[] models = new VehicleHash[30];

    public Motorcycles()
    {
        config = ScriptSettings.Load("Scripts\\AllMpVehiclesInSp.ini");
        doors_config = config.GetValue<int>("MAIN", "doors", 1);
        blip_config = config.GetValue<int>("MAIN", "blips", 1);

        coords[0] = new Vector3(-2316.49f, 280.86f, 168.467f);
        coords[1] = new Vector3(-3036.57f, 105.31f, 10.593f); 
        coords[2] = new Vector3(-3071.87f, 658.171f, 9.918f); 
        coords[3] = new Vector3(-1534.83f, 889.731f, 180.803f);
        coords[4] = new Vector3(231.935f, 1162.313f, 224.464f); 
        coords[5] = new Vector3(-582.454f, -859.433f, 25.034f); 
        all_coords = 5;

        angle[0] = 146.244f;
        angle[1] = 141.262f;
        angle[2] = 47.597f;
        angle[3] = 138.808f;
        angle[4] = 33.185f;
        angle[5] = 33.185f;

        models[0] = VehicleHash.Powersurge;
        models[1] = VehicleHash.Manchez3;
        models[2] = VehicleHash.Reever;
        models[3] = VehicleHash.Shinobi;
        models[4] = VehicleHash.Manchez2;
        models[5] = VehicleHash.Stryder;
        models[6] = VehicleHash.FCR2;
        models[7] = VehicleHash.FCR;
        models[8] = VehicleHash.Diablous2;
        models[9] = VehicleHash.Diablous;
        models[10] = VehicleHash.Esskey;
        models[11] = VehicleHash.Vortex;
        models[12] = VehicleHash.Daemon2;
        models[13] = VehicleHash.ZombieB;
        models[14] = VehicleHash.ZombieA;
        models[15] = VehicleHash.Wolfsbane;
        models[16] = VehicleHash.Nightblade;
        models[17] = VehicleHash.Manchez;
        models[18] = VehicleHash.Hakuchou2;
        models[19] = VehicleHash.Faggio;
        models[20] = VehicleHash.Faggio3;
        models[21] = VehicleHash.Defiler;
        models[22] = VehicleHash.Chimera;
        models[23] = VehicleHash.Avarus;
        models[24] = VehicleHash.Cliffhanger;
        models[25] = VehicleHash.Gargoyle;
        models[26] = VehicleHash.BF400;
        models[27] = VehicleHash.Vindicator;
        models[28] = VehicleHash.Lectro;
        models[29] = VehicleHash.Enduro;

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
                        var veh_model = new Model(models[rnd.Next(0, 29)]);
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