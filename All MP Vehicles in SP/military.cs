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

public class Military : Script
{
    private int spawned = 0;
    private int x = 0;
    private float distance = 150.0f;
    private Blip marker;

    private Vector3[] coords = new Vector3[19];
    private float[] angle = new float[19];
    private GTA.Vehicle[] car = new GTA.Vehicle[19];
    private VehicleHash[] models = new VehicleHash[41];

    public Military()
    {
        coords[0] = new Vector3(-1922.443f, 3010.560f, 32.810f); 
        coords[1] = new Vector3(-1941.971f, 3025.726f, 32.810f);
        coords[2] = new Vector3(-1898.065f, 3046.221f, 32.811f);
        coords[3] = new Vector3(-1985.113f, 3044.164f, 32.810f);
        coords[4] = new Vector3(-1988.379f, 3029.776f, 32.810f); 
        coords[5] = new Vector3(-1985.841f, 3056.533f, 32.810f); 
        coords[6] = new Vector3(-1976.082f, 3079.133f, 32.810f); 
        coords[7] = new Vector3(-1987.650f, 3091.420f, 32.810f); 
        coords[8] = new Vector3(-2027.833f, 3078.663f, 32.810f); 
        coords[9] = new Vector3(-2045.754f, 3095.386f, 32.810f); 
        coords[10] = new Vector3(-2059.501f, 3077.011f, 32.810f);
        coords[11] = new Vector3(-1892.247f, 3082.933f, 32.810f); 
        coords[12] = new Vector3(-1934.867f, 3109.608f, 32.810f); 
        coords[13] = new Vector3(-1965.212f, 3101.532f, 32.810f); 
        coords[14] = new Vector3(-1907.528f, 3117.613f, 32.959f); 
        coords[15] = new Vector3(-1903.383f, 3115.676f, 32.810f);
        coords[16] = new Vector3(-2060.052f, 3146.352f, 32.8103f);
        coords[17] = new Vector3(-2084.384f, 3161.351f, 32.8103f);
        coords[18] = new Vector3(-2116.779f, 3166.792f, 32.8101f);

        angle[0] = 327.087f;
        angle[1] = 240.052f;
        angle[2] = 239.996f;
        angle[3] = 344.361f;
        angle[4] = 60.516f;
        angle[5] = 329.284f;
        angle[6] = 149.870f;
        angle[7] = 60.084f;
        angle[8] = 151.765f;
        angle[9] = 5.327f;
        angle[10] = 145.951f;
        angle[11] = 147.141f;
        angle[12] = 150.073f;
        angle[13] = 236.324f;
        angle[14] = 147.964f;
        angle[15] = 150.114f; 
        angle[16] = 239.2013f;
        angle[17] = 61.37347f;
        angle[18] = 237.5298f;

        models[0] = VehicleHash.Vetir;
        models[1] = VehicleHash.Scarab;
        models[2] = VehicleHash.Terrorbyte;
        models[3] = VehicleHash.Thruster;
        models[4] = VehicleHash.Khanjari;
        models[5] = VehicleHash.Chernobog;
        models[6] = VehicleHash.Barrage;
        models[7] = VehicleHash.TrailerLarge;
        models[8] = VehicleHash.HalfTrack;
        models[9] = VehicleHash.APC;
        models[10] = VehicleHash.TrailerSmall2;

        
        models[11] = VehicleHash.Alkonost;
        models[12] = VehicleHash.Strikeforce;
        models[13] = VehicleHash.Avenger;
        models[14] = VehicleHash.Volatol;
        models[15] = VehicleHash.Nokota;
        models[16] = VehicleHash.Seabreeze;
        models[17] = VehicleHash.Pyro;
        models[18] = VehicleHash.Mogul;
        models[19] = VehicleHash.Howard;
        models[20] = VehicleHash.Bombushka;
        models[21] = VehicleHash.Molotok;
        models[22] = VehicleHash.Tula;
        models[23] = VehicleHash.Rogue;
        models[24] = VehicleHash.Starling;
        models[25] = VehicleHash.AlphaZ1;
        models[26] = VehicleHash.Hydra;

        
        models[27] = VehicleHash.Annihilator2;
        models[28] = VehicleHash.Akula;
        models[29] = VehicleHash.Hunter;
        models[30] = VehicleHash.Valkyrie;
        models[31] = VehicleHash.Savage;

        
        models[32] = VehicleHash.Oppressor;
        models[33] = VehicleHash.Oppressor2;

        
        models[34] = VehicleHash.Squaddie;
        models[35] = VehicleHash.Manchez2;
        models[36] = VehicleHash.Winky;
        models[37] = VehicleHash.Insurgent3;
        models[38] = Function.Call<VehicleHash>(Hash.GET_HASH_KEY, "raiju");
        models[39] = Function.Call<VehicleHash>(Hash.GET_HASH_KEY, "streamer216");
        models[40] = Function.Call<VehicleHash>(Hash.GET_HASH_KEY, "conada2");

        spawned = 0;
        Tick += OnTick;
    }

    void OnTick(object sender, EventArgs e)
    {
        {
            Vector3 fix_coords = new Vector3(0.0f, 0.0f, 0.0f);
            var position = Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 0, 0));
            if (spawned == 0 && GTA.Native.Function.Call<bool>(GTA.Native.Hash.IS_ENTITY_IN_ZONE, Game.Player.Character, "ARMYB"))
            {
                for (int i = 0; i <= 10; i++)
                {
                    var veh_model = new Model(models[0]);
                    veh_model.Request(500);
                    while (!veh_model.IsLoaded) Script.Wait(100);
                    car[i] = World.CreateVehicle(models[i], coords[i], angle[i]);
                    Function.Call(Hash.DECOR_SET_INT, car[i], "MPBitset", 0);
                }
                Random rnd = new Random();
                car[11] = World.CreateVehicle(models[rnd.Next(11, 27)], coords[11], angle[11]);
                Function.Call(Hash.DECOR_SET_INT, car[11], "MPBitset", 0);

                car[12] = World.CreateVehicle(models[rnd.Next(11, 27)], coords[12], angle[12]);
                Function.Call(Hash.DECOR_SET_INT, car[12], "MPBitset", 0);

                car[13] = World.CreateVehicle(models[rnd.Next(27, 32)], coords[13], angle[13]);
                Function.Call(Hash.DECOR_SET_INT, car[13], "MPBitset", 0);

                car[14] = World.CreateVehicle(models[rnd.Next(32, 34)], coords[14], angle[14]);
                Function.Call(Hash.DECOR_SET_INT, car[14], "MPBitset", 0);

                car[15] = World.CreateVehicle(models[rnd.Next(34, 38)], coords[15], angle[15]);
                Function.Call(Hash.DECOR_SET_INT, car[15], "MPBitset", 0);

                car[16] = World.CreateVehicle(models[38], coords[16], angle[16]);
                Function.Call(Hash.DECOR_SET_INT, car[16], "MPBitset", 0);

                car[17] = World.CreateVehicle(models[39], coords[17], angle[17]);
                Function.Call(Hash.DECOR_SET_INT, car[17], "MPBitset", 0);

                car[18] = World.CreateVehicle(models[40], coords[18], angle[18]);
                Function.Call(Hash.DECOR_SET_INT, car[18], "MPBitset", 0);

                spawned = 1;
            }
            
            if (Function.Call<bool>(Hash.IS_ENTITY_IN_ZONE, Game.Player.Character, "ARMYB") == false && spawned == 1)
            {
                for (int i = 0; i <= 14; i++)
                {
                    if (GTA.Native.Function.Call<bool>(GTA.Native.Hash.IS_PED_IN_VEHICLE, Game.Player.Character, car[i], false))
                    {
                        car[i].MarkAsNoLongerNeeded();
                    }
                    else
                    {
                        car[i].Delete();
                    }
                }
                spawned = 0;
            }
        }
    }
}