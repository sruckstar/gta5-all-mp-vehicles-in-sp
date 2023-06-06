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

public class Suvs : Script
{
    ScriptSettings config;
    private int doors_config = 0;
    private int blip_config = 0;
    private int spawned = 0;
    private int x = 0;
    private float distance = 150.0f;
    private Blip marker;

    private Vector3[] coords = new Vector3[8];
    private float[] angle = new float[8];
    private GTA.Vehicle car;
    private int all_coords;
    private VehicleHash[] models = new VehicleHash[18];

    public Suvs()
    {
        config = ScriptSettings.Load("Scripts\\AllMpVehiclesInSp.ini");
        doors_config = config.GetValue<int>("MAIN", "doors", 1);
        blip_config = config.GetValue<int>("MAIN", "blips", 1);

        coords[0] = new Vector3(-2340.76f, 296.197f, 168.467f);
        coords[1] = new Vector3(629.014f, 196.173f, 96.128f);
        coords[2] = new Vector3(1150.161f, -991.569f, 44.528f);
        coords[3] = new Vector3(244.916f, -860.606f, 28.5f); 
        coords[4] = new Vector3(-340.099f, -876.452f, 30.071f);
        coords[5] = new Vector3(387.275f, -215.651f, 55.835f); 
        coords[6] = new Vector3(-1234.11f, -1646.83f, 3.129f); 
        coords[7] = new Vector3(-472.038f, 6034.981f, 30.341f); 
        all_coords = 7;

        angle[0] = 146.244f;
        angle[1] = 141.262f;
        angle[2] = 47.597f;
        angle[3] = 138.808f;
        angle[4] = 33.185f;
        angle[4] = 33.185f;

        models[0] = VehicleHash.Issi8;
        models[1] = VehicleHash.Granger2;
        models[2] = VehicleHash.IWagen;
        models[3] = VehicleHash.Baller7;
        models[4] = VehicleHash.Astron;
        models[5] = VehicleHash.Jubilee;
        models[6] = VehicleHash.Seminole2;
        models[7] = VehicleHash.Landstalker2;
        models[8] = VehicleHash.Rebla;
        models[9] = VehicleHash.Novak;
        models[10] = VehicleHash.Toros;
        models[11] = VehicleHash.Stretch;
        models[12] = VehicleHash.Contender;
        models[13] = VehicleHash.XLS2;
        models[14] = VehicleHash.Baller3;
        models[15] = VehicleHash.Baller4;
        models[16] = VehicleHash.Baller5;
        models[17] = VehicleHash.Baller6;

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
                        var veh_model = new Model(models[rnd.Next(0, 18)]);
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