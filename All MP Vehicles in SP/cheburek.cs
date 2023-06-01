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
    private int spawned = 0;
    private int x = 0;
    private float distance = 150.0f;
    private Blip marker;

    private Vector3[] coords = new Vector3[1];
    private float[] angle = new float[1];
    private GTA.Vehicle car;
    private int all_coords;
    private VehicleHash[] models = new VehicleHash[3];

    public Cheburek()
    {
        coords[0] = new Vector3(1546.591f, 3781.791f, 33.06f); //26.557 //чебурек
        all_coords = 0;

        angle[0] = 26.557f;

        models[0] = VehicleHash.Cheburek;
        models[1] = VehicleHash.RatBike;
        models[2] = VehicleHash.SlamVan3;

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
                        var veh_model = new Model(models[rnd.Next(0, 3)]);
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

            //проверка, сидит ли игрок в заспавненном тс. если да, то ремув
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


            if (spawned == 1 && car != null) //удаление тс, если игрок покинул зону видимости 
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