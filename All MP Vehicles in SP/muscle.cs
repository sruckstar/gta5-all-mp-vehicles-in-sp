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

public class Muscle : Script
{
    private int spawned = 0;
    private int x = 0;
    private float distance = 150.0f;
    private Blip marker;

    private Vector3[] coords = new Vector3[7];
    private float[] angle = new float[7];
    private GTA.Vehicle car;
    private int all_coords;
    private VehicleHash[] models = new VehicleHash[44];

    public Muscle()
    {
        coords[0] = new Vector3(-604.458f, -1218.29f, 13.507f); 
        coords[1] = new Vector3(28.385f, -1707.34f, 28.298f); 
        coords[2] = new Vector3(-329.7f, -700.958f, 31.912f); 
        coords[3] = new Vector3(238.339f, -35.01f, 68.728f);
        coords[4] = new Vector3(393.61f, -649.557f, 27.5f);
        coords[5] = new Vector3(124.231f, -1472.5f, 28.142f); 
        coords[6] = new Vector3(185.595f, -1016.01f, 28.3f); 
        all_coords = 6;

        angle[0] = 146.244f;
        angle[1] = 141.262f;
        angle[2] = 47.597f;
        angle[3] = 138.808f;
        angle[4] = 33.185f;
        angle[4] = 33.185f;

        models[0] = VehicleHash.Tahoma;
        models[1] = VehicleHash.Tulip2;
        models[2] = VehicleHash.Weevil2;
        models[3] = VehicleHash.Vigero2;
        models[4] = VehicleHash.Ruiner4;
        models[5] = VehicleHash.Buffalo4;
        models[6] = VehicleHash.Dominator7;
        models[7] = VehicleHash.Dominator8;
        models[8] = VehicleHash.Gauntlet5;
        models[9] = VehicleHash.Manana2;
        models[10] = VehicleHash.Dukes3;
        models[11] = VehicleHash.Yosemite2;
        models[12] = VehicleHash.Peyote2;
        models[13] = VehicleHash.Gauntlet4;
        models[14] = VehicleHash.Gauntlet3;
        models[15] = VehicleHash.Vamos;
        models[16] = VehicleHash.Deviant;
        models[17] = VehicleHash.Tulip;
        models[18] = VehicleHash.Clique;
        models[19] = VehicleHash.Imperator;
        models[20] = VehicleHash.Impaler;
        models[21] = VehicleHash.Dominator3;
        models[22] = VehicleHash.Ellie;
        models[23] = VehicleHash.Hustler;
        models[24] = VehicleHash.Hermes;
        models[25] = VehicleHash.Yosemite;
        models[26] = VehicleHash.Tampa3;
        models[27] = VehicleHash.Ruiner2;
        models[28] = VehicleHash.SabreGT2;
        models[29] = VehicleHash.Virgo2;
        models[30] = VehicleHash.Virgo3;
        models[31] = VehicleHash.Faction3;
        models[32] = VehicleHash.Tampa;
        models[33] = VehicleHash.Nightshade;
        models[34] = VehicleHash.Moonbeam2;
        models[35] = VehicleHash.Moonbeam;
        models[36] = VehicleHash.Faction2;
        models[37] = VehicleHash.Faction;
        models[38] = VehicleHash.Chino2;
        models[39] = VehicleHash.Buccaneer2;
        models[40] = VehicleHash.Coquette3;
        models[41] = VehicleHash.Chino;
        models[42] = VehicleHash.Vigero;
        models[43] = VehicleHash.SlamVan2;

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
                        var veh_model = new Model(models[rnd.Next(0, 44)]);
                        veh_model.Request(500);
                        while (!veh_model.IsLoaded) Script.Wait(100);
                        car = World.CreateVehicle(veh_model, coords[i], angle[i]);
                        Function.Call(Hash.DECOR_SET_INT, car, "MPBitset", 0);
                        spawned = 1;
                        marker = GTA.Native.Function.Call<Blip>(GTA.Native.Hash.ADD_BLIP_FOR_ENTITY, car);
                        GTA.Native.Function.Call(GTA.Native.Hash.SET_BLIP_SPRITE, marker, 1);
                        GTA.Native.Function.Call(GTA.Native.Hash.SET_BLIP_COLOUR, marker, 3);
                        GTA.Native.Function.Call(GTA.Native.Hash.FLASH_MINIMAP_DISPLAY);
                        Function.Call(Hash._0xF9113A30DE5C6670, "STRING");
                        Function.Call(Hash._ADD_TEXT_COMPONENT_STRING, "Unique vehicle");
                        Function.Call(Hash._0xBC38B49BCB83BC9B, marker);
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
                    marker.Remove();
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
                    car.Delete();
                    car = null;
                    marker.Remove();
                }
            }
        }
    }
}