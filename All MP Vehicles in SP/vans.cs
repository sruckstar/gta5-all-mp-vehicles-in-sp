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

public class vans : Script
{
    private int spawned = 0;
    private int x = 0;
    private float distance = 100.0f;
    private Blip marker;

    private Vector3[] coords = new Vector3[13];
    private float[] angle = new float[13];
    private GTA.Vehicle car;
    private int all_coords;
    private VehicleHash[] models = new VehicleHash[7];

    public vans()
    {
        coords[0] = new Vector3(140.945f, 6606.513f, 30.845f); //фургон 0.239
        coords[1] = new Vector3(1362.672f, 1178.352f, 111.609f); //фургон 359.306
        coords[2] = new Vector3(2593.022f, 364.349f, 107.457f); //174.745 фургон
        coords[3] = new Vector3(2002.724f, 3769.429f, 31.181f); //298.783 //фургон
        coords[4] = new Vector3(-771.927f, 5566.46f, 32.486f); //271.230 //фургон
        coords[5] = new Vector3(1697.817f, 6414.365f, 31.73f); //247.870 фургон 
        coords[6] = new Vector3(1700.445f, 4937.267f, 41.078f); //147.276 //фургон
        coords[7] = new Vector3(-1804.77f, 804.137f, 137.514f); //223.747 //фургон
        coords[8] = new Vector3(756.539f, 2525.957f, 72.161f); //270.240 //фургон
        coords[9] = new Vector3(1205.454f, 2658.357f, 36.824f); //223.627 //фургон
        coords[10] = new Vector3(-165.839f, 6454.25f, 30.495f); //фургон 225.116
        coords[11] = new Vector3(-2221.14f, 4232.757f, 46.132f); //225.108 фургон
        coords[12] = new Vector3(-2555.51f, 2322.827f, 32.06f); //273.837 //фургон
        all_coords = 12;

        angle[0] = 0.239f;
        angle[1] = 359.306f;
        angle[2] = 174.745f;
        angle[3] = 298.783f;
        angle[4] = 271.230f;
        angle[5] = 247.870f;
        angle[6] = 147.276f;
        angle[7] = 223.747f;
        angle[8] = 270.240f;
        angle[9] = 223.627f;
        angle[10] = 225.116f;
        angle[11] = 225.108f;
        angle[12] = 273.837f;

        models[0] = VehicleHash.Journey2;
        models[1] = VehicleHash.Surfer3;
        models[2] = VehicleHash.Youga3;
        models[3] = VehicleHash.Speedo4;
        models[4] = VehicleHash.Youga2;
        models[5] = VehicleHash.Rumpo3;
        models[6] = VehicleHash.Minivan2;

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
                        var veh_model = new Model(models[rnd.Next(0, 7)]);
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