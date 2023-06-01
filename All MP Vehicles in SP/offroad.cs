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

public class Offroad : Script
{
    private int spawned = 0;
    private int x = 0;
    private float distance = 150.0f;
    private Blip marker;

    private Vector3[] coords = new Vector3[4];
    private float[] angle = new float[4];
    private GTA.Vehicle car;
    private int all_coords;
    private VehicleHash[] models = new VehicleHash[31];

    public Offroad()
    {
        coords[0] = new Vector3(386.663f, 2640.138f, 43.493f);
        coords[1] = new Vector3(1991.707f, 3078.063f, 46.016f);
        coords[2] = new Vector3(1977.207f, 3837.1f, 30.997f); 
        coords[3] = new Vector3(1350.173f, 3601.249f, 33.899f); 
        all_coords = 3;

        angle[0] = 126.963f;
        angle[1] = 318.133f;
        angle[2] = 355.228f;
        angle[3] = 148.566f;

        models[0] = VehicleHash.Boor;
        models[1] = VehicleHash.Draugur;
        models[2] = VehicleHash.Patriot3;
        models[3] = VehicleHash.Verus;
        models[4] = VehicleHash.Yosemite3;
        models[5] = VehicleHash.Outlaw;
        models[6] = VehicleHash.Zhaba;
        models[7] = VehicleHash.Everon;
        models[8] = VehicleHash.Vagrant;
        models[9] = VehicleHash.Hellion;
        models[10] = VehicleHash.Caracara2;
        models[11] = VehicleHash.Brutus;
        models[12] = VehicleHash.Monster3;
        models[13] = VehicleHash.Bruiser;
        models[14] = VehicleHash.Freecrawler;
        models[15] = VehicleHash.Menacer;
        models[16] = VehicleHash.Caracara;
        models[17] = VehicleHash.Kamacho;
        models[18] = VehicleHash.Riata;
        models[19] = VehicleHash.NightShark;
        models[20] = VehicleHash.Technical3;
        models[21] = VehicleHash.Dune3;
        models[22] = VehicleHash.Blazer5;
        models[23] = VehicleHash.Blazer4;
        models[24] = VehicleHash.RallyTruck;
        models[25] = VehicleHash.TrophyTruck;
        models[26] = VehicleHash.TrophyTruck2;
        models[27] = VehicleHash.Brawler;
        models[28] = VehicleHash.Technical;
        models[29] = VehicleHash.Insurgent;
        models[30] = VehicleHash.Guardian;

        car = null;
        spawned = 0;
        Tick += OnTick;
    }

    void OnTick(object sender, EventArgs e)
    {
        {
            Vector3 fix_coords = new Vector3(0.0f, 0.0f, 0.0f);
            var position = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 0, 0));

            for (int i = 0; i <= all_coords; i++)
            {
                if (spawned == 0 && Function.Call<bool>(Hash.IS_PLAYER_SWITCH_IN_PROGRESS) == false)
                {
                    if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, coords[i].X, coords[i].Y, coords[i].Z, position.X, position.Y, position.Z, 0) < distance)
                    {
                        Random rnd = new Random();
                        var veh_model = new Model(models[rnd.Next(0, 8)]);
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
                    position = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 0, 0));
                }
            }

            if (car == null && spawned == 1)
            {
                position = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 0, 0));
                while (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, coords[x].X, coords[x].Y, coords[x].Z, position.X, position.Y, position.Z, 0) < distance)
                {
                    Script.Wait(100);
                    position = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 0, 0));
                }
                spawned = 0;
            }


            if (spawned == 1 && car != null) 
            {
                position = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 0, 0));
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