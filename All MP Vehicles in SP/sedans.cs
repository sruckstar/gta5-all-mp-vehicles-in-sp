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

public class Sedans : Script
{
    private int spawned = 0;
    private int x = 0;
    private float distance = 150.0f;
    private Blip marker;

    private Vector3[] coords = new Vector3[7];
    private float[] angle = new float[7];
    private GTA.Vehicle car;
    private int all_coords;
    private VehicleHash[] models = new VehicleHash[17];

    public Sedans()
    {
        coords[0] = new Vector3(1156.682f, -1474.15f, 33.693f); 
        coords[1] = new Vector3(-936.334f, -2692.07f, 15.611f); 
        coords[2] = new Vector3(-532.351f, -2134.22f, 4.992f); 
        coords[3] = new Vector3(-1528.44f, -427.05f, 34.447f); 
        coords[4] = new Vector3(642.042f, 587.747f, 127.911f); 
        coords[5] = new Vector3(-3138.86f, 1086.83f, 19.669f);
        coords[6] = new Vector3(-1144.0f, 2666.28f, 17.094f); 
        all_coords = 6;

        angle[0] = 146.244f;
        angle[1] = 141.262f;
        angle[2] = 47.597f;
        angle[3] = 138.808f;
        angle[4] = 33.185f;
        angle[4] = 33.185f;

        models[0] = VehicleHash.Rhinehart;
        models[1] = VehicleHash.Deity;
        models[2] = VehicleHash.Cinquemila;
        models[3] = VehicleHash.Tailgater2;
        models[4] = VehicleHash.Warrener2;
        models[5] = VehicleHash.Glendale2;
        models[6] = VehicleHash.Stafford;
        models[7] = VehicleHash.Schafter3;
        models[8] = VehicleHash.Schafter4;
        models[9] = VehicleHash.Limo2;
        models[10] = VehicleHash.Schafter5;
        models[11] = VehicleHash.Schafter6;
        models[12] = VehicleHash.Cog552;
        models[13] = VehicleHash.Cog55;
        models[14] = VehicleHash.Cognoscenti2;
        models[15] = VehicleHash.Cognoscenti;
        models[16] = VehicleHash.Primo2;

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
                        var veh_model = new Model(models[rnd.Next(0, 17)]);
                        veh_model.Request(500);
                        while (!veh_model.IsLoaded) Script.Wait(100);
                        car = World.CreateVehicle(veh_model, coords[i], angle[i]);
                        Function.Call(Hash.DECOR_SET_INT, car, "MPBitset", 0);
                        spawned = 1;
                        marker = GTA.Native.Function.Call<Blip>(GTA.Native.Hash.ADD_BLIP_FOR_ENTITY, car);
                        GTA.Native.Function.Call(GTA.Native.Hash.SET_BLIP_SPRITE, marker, 1);
                        GTA.Native.Function.Call(GTA.Native.Hash.SET_BLIP_COLOUR, marker, 3);
                        GTA.Native.Function.Call(GTA.Native.Hash.FLASH_MINIMAP_DISPLAY);
                        Function.Call(Hash.BEGIN_TEXT_COMMAND_SET_BLIP_NAME, "STRING");
                        Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "Unique vehicle");
                        Function.Call(Hash.END_TEXT_COMMAND_SET_BLIP_NAME, marker);
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
                    marker.Delete();
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
                    marker.Delete();
                }
            }
        }
    }
}