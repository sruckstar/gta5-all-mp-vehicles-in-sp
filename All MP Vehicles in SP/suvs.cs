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
        coords[0] = new Vector3(-2340.76f, 296.197f, 168.467f); // 294.694
        coords[1] = new Vector3(629.014f, 196.173f, 96.128f); //73.644
        coords[2] = new Vector3(1150.161f, -991.569f, 44.528f); //184.693
        coords[3] = new Vector3(244.916f, -860.606f, 28.5f); //2545.340
        coords[4] = new Vector3(-340.099f, -876.452f, 30.071f); //342.657
        coords[5] = new Vector3(387.275f, -215.651f, 55.835f); //342.648
        coords[6] = new Vector3(-1234.11f, -1646.83f, 3.129f); //303.873
        coords[7] = new Vector3(-472.038f, 6034.981f, 30.341f); //48.131 //SUVs
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
            var position = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 0, 0));

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