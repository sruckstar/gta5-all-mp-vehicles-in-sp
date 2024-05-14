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
using System.IO;

public class SpawnMP : Script
{
    ScriptSettings config;
    int vehicles_spawned;
    private int doors_config = 0;
    private int blip_config = 0;
    private int[] mode_type = new int[5];
    private int traffic_blip_config = 0;
    private float[] angle = new float[1];
    private GTA.Vehicle car;
    private int tuning_flag;
    private int street_flag;
    private int blip_color;
    private int mod_plate;
    private int cooldown = 0;
    private int plate_id = -1;
    private Vehicle[] veh = new Vehicle[200];
    private Vehicle[] street_veh = new Vehicle[200];
    private List<Blip> marker = new List<Blip>();
    Blip traffic_marker;

    //Coords
    private const int arena = 0;
    private const int boats = 1;
    private const int cemetery = 2;
    private const int cheburek = 3;
    private const int cinema = 4;
    private const int cluckin = 5;
    private const int compacts_1 = 6;
    private const int compacts_2 = 7;
    private const int compacts_3 = 8;
    private const int compacts_4 = 9;
    private const int compacts_5 = 10;
    private const int compacts_6 = 11;
    private const int coupes_1 = 12;
    private const int coupes_2 = 13;
    private const int coupes_3 = 14;
    private const int coupes_4 = 15;
    private const int coupes_5 = 16;
    private const int coupes_6 = 17;
    private const int coupes_7 = 18;
    private const int cycles_1 = 19;
    private const int cycles_2 = 20;
    private const int cycles_3 = 21;
    private const int cycles_4 = 22;
    private const int cycles_5 = 23;
    private const int ghetto_1 = 24;
    private const int ghetto_2 = 25;
    private const int ghetto_3 = 26;
    private const int ghetto_4 = 27;
    private const int ghetto_5 = 28;
    private const int helicopter = 29;
    private const int humanlabs = 30;
    private const int industrial_1 = 31;
    private const int industrial_2 = 32;
    private const int industrial_3 = 33;
    private const int industrial_4 = 34;
    private const int karting = 35;

    private const int vetir = 36;
    private const int scarab = 37;
    private const int terrorbyte = 38;
    private const int thruster = 39;
    private const int khanjari = 40;
    private const int chernobog = 41;
    private const int barrage = 42;
    private const int trailerLarge = 43;
    private const int halfTrack = 44;
    private const int apc = 45;
    private const int trailerSmall2 = 46;


    private const int military_planes_1 = 47;
    private const int military_planes_2 = 48;
    private const int military_helicopters = 49;
    private const int military_opressors = 50;
    private const int military_bikes = 51;
    private const int raiju = 52;
    private const int streamer216 = 53;
    private const int conada2 = 54;

    private const int motorcycles_1 = 55;
    private const int motorcycles_2 = 56;
    private const int motorcycles_3 = 57;
    private const int motorcycles_4 = 58;
    private const int motorcycles_5 = 59;
    private const int motorcycles_6 = 60;
    private const int muscle_1 = 61;
    private const int muscle_2 = 62;
    private const int muscle_3 = 63;
    private const int muscle_4 = 64;
    private const int muscle_5 = 65;
    private const int muscle_6 = 66;
    private const int muscle_7 = 67;
    private const int offroad_1 = 68;
    private const int offroad_2 = 69;
    private const int offroad_3 = 70;
    private const int offroad_4 = 71;
    private const int openwheel = 72;
    private const int beach = 73;
    private const int planes = 74;
    private const int police_1 = 75;
    private const int police_2 = 76;
    private const int police_3 = 77;
    private const int police_4 = 78;
    private const int police_5 = 79;
    private const int sedans_1 = 80;
    private const int sedans_2 = 81;
    private const int sedans_3 = 82;
    private const int sedans_4 = 83;
    private const int sedans_5 = 84;
    private const int sedans_6 = 85;
    private const int sedans_7 = 86;
    private const int slawmantruck = 87;
    private const int sportclassic_1 = 88;
    private const int sportclassic_2 = 89;
    private const int sportclassic_3 = 90;
    private const int sportclassic_4 = 91;
    private const int sportclassic_5 = 92;
    private const int sportclassic_6 = 93;
    private const int sportclassic_7 = 94;
    private const int sportclassic_8 = 95;
    private const int submarine = 96;
    private const int supers_1 = 97;
    private const int supers_2 = 98;
    private const int supers_3 = 99;
    private const int supers_4 = 100;
    private const int supers_5 = 101;
    private const int supers_6 = 102;
    private const int supers_7 = 103;
    private const int suvs_1 = 104;
    private const int suvs_2 = 105;
    private const int suvs_3 = 106;
    private const int suvs_4 = 107;
    private const int suvs_5 = 108;
    private const int suvs_6 = 109;
    private const int suvs_7 = 110;
    private const int suvs_8 = 111;
    private const int towtruck_1 = 112;
    private const int towtruck_2 = 113;
    private const int towtruck_3 = 114;
    private const int towtruck_4 = 115;
    private const int towtruck_5 = 116;
    private const int tuners_1 = 117;
    private const int tuners_2 = 118;
    private const int tuners_3 = 119;
    private const int tuners_4 = 120;
    private const int tuners_5 = 121;
    private const int valentine = 122;
    private const int vans_1 = 123;
    private const int vans_2 = 124;
    private const int vans_3 = 125;
    private const int vans_4 = 126;
    private const int vans_5 = 127;
    private const int vans_6 = 128;
    private const int vans_7 = 129;
    private const int vans_8 = 130;
    private const int vans_9 = 131;
    private const int vans_10 = 132;
    private const int vans_11 = 133;
    private const int vans_12 = 134;
    private const int vans_13 = 135;
    private const int wastelander = 136;
    private const int weaponboats = 137;

    private int debug_releport = arena;

    private List<Vector3> coords = new List<Vector3>()
    {
        new Vector3(-236.7005f, -2061.008f, 27.48775f),
        new Vector3(-926.119f, -1478.350f, -0.474f),
        new Vector3(-1640.42f, -202.879f, 54.146f),
        new Vector3(1546.591f, 3781.791f, 33.06f),
        new Vector3(-1084.873f, -477.591f, 36.2069f),
        new Vector3(-19.4496f, 6321.813f, 31.22966f),
        new Vector3(-1407.751f, -589.1447f, 29.65687f),
        new Vector3(-817.325f, -1201.59f, 5.935f),
        new Vector3(-489.2397f, -596.5908f, 30.56949f),
        new Vector3(870.7411f, -75.28734f, 78.10686f),
        new Vector3(110.261f, -714.605f, 32.133f),
        new Vector3(-220.102f, -590.273f, 33.264f),
        new Vector3(-74.575f, -619.874f, 35.173f),
        new Vector3(283.769f, -342.644f, 43.92f),
        new Vector3(-1044.02f, -2608.02f, 19.775f),
        new Vector3(-801.566f, -1313.92f, 4.0f),
        new Vector3(-972.578f, -1464.27f, 4.013f),
        new Vector3(1309.942f, -530.154f, 70.312f),
        new Vector3(339.481f, 159.143f, 102.146f),
        new Vector3(-1374.766f, -1399.443f, 6.142528f),
        new Vector3(-941.4034f, -792.0335f, 15.95103f),
        new Vector3(274.6519f, -194.8017f, 61.57079f),
        new Vector3(698.6535f, -1197.893f, 24.39086f),
        new Vector3(840.2529f, -257.3479f, 65.66613f),
        new Vector3(-229.587f, -1483.44f, 30.352f),
        new Vector3(-22.296f, -1851.58f, 24.108f),
        new Vector3(321.798f, -1948.14f, 23.627f),
        new Vector3(455.602f, -1695.26f, 28.289f),
        new Vector3(1228.548f, -1605.65f, 50.736f),
        new Vector3(-979.378f, -2996.868f, 13.945f),
        new Vector3(3511.653f, 3783.877f, 28.925f),
        new Vector3(1566.097f, -1683.17f, 87.205f),
        new Vector3(2673.478f, 1678.569f, 23.488f),
        new Vector3(839.097f, 2202.196f, 50.46f),
        new Vector3(2717.772f, 1391.725f, 23.535f),
        new Vector3(-1530.63f, -993.47f, 12.017f),
        new Vector3(-1922.443f, 3010.560f, 32.810f),
        new Vector3(-1941.971f, 3025.726f, 32.810f),
        new Vector3(-1898.065f, 3046.221f, 32.811f),
        new Vector3(-1985.113f, 3044.164f, 32.810f),
        new Vector3(-1988.379f, 3029.776f, 32.810f),
        new Vector3(-1985.841f, 3056.533f, 32.810f),
        new Vector3(-1976.082f, 3079.133f, 32.810f),
        new Vector3(-1987.650f, 3091.420f, 32.810f),
        new Vector3(-2027.833f, 3078.663f, 32.810f),
        new Vector3(-2045.754f, 3095.386f, 32.810f),
        new Vector3(-2059.501f, 3077.011f, 32.810f),
        new Vector3(-1892.247f, 3082.933f, 32.810f),
        new Vector3(-1934.867f, 3109.608f, 32.810f),
        new Vector3(-1965.212f, 3101.532f, 32.810f),
        new Vector3(-1907.528f, 3117.613f, 32.959f),
        new Vector3(-1903.383f, 3115.676f, 32.810f),
        new Vector3(-2060.052f, 3146.352f, 32.8103f),
        new Vector3(-2084.384f, 3161.351f, 32.8103f),
        new Vector3(-2116.779f, 3166.792f, 32.8101f),
        new Vector3(-2316.357f, 280.0749f, 168.9348f),
        new Vector3(-3036.57f, 105.31f, 10.593f),
        new Vector3(-3072.296f, 657.9456f, 10.53257f),
        new Vector3(-1535.044f, 890.5871f, 181.3348f),
        new Vector3(231.9765f, 1161.922f, 224.9349f),
        new Vector3(-582.6653f, -859.2297f, 25.49919f),
        new Vector3(-604.9778f, -1218.401f, 13.92473f),
        new Vector3(31.46499f, -1706.062f, 28.6591f),
        new Vector3(-329.9433f, -700.7843f, 32.33982f),
        new Vector3(238.2489f, -34.84402f, 69.18212f),
        new Vector3(393.4623f, -649.7198f, 27.92926f),
        new Vector3(124.0182f, -1472.58f, 28.6794f),
        new Vector3(185.595f, -1016.01f, 28.3f),
        new Vector3(392.6896f, 2641.558f, 44.07256f),
        new Vector3(1991.201f, 3076.069f, 46.79815f),
        new Vector3(1977.402f, 3835.433f, 31.59359f),
        new Vector3(1350.489f, 3605.351f, 34.47185f),
        new Vector3(1122.086f, 267.125f, 79.856f),
        new Vector3(-1513.889f, -1253.183f, 2.433f),
        new Vector3(-961.005f, -2963.593f, 13.945f),
        new Vector3(-449.017f, 6052.354f, 31.341f),
        new Vector3(1867.271f, 3696.303f, 33.606f),
        new Vector3(626.4047f, 27.50228f, 87.9091f),
        new Vector3(-1051.572f, -867.256f, 5.129f),
        new Vector3(375.766f, -1612.061f, 29.292f),
        new Vector3(1156.74f, -1474.257f, 33.9701f),
        new Vector3(-936.2781f, -2692.023f, 16.11801f),
        new Vector3(-532.5765f, -2133.869f, 5.491799f),
        new Vector3(-1528.733f, -427.0032f, 35.01511f),
        new Vector3(642.1031f, 587.9972f, 128.4254f),
        new Vector3(-3139.044f, 1086.714f, 20.23225f),
        new Vector3(-1144.189f, 2666.219f, 17.47463f),
        new Vector3(452.709f, -1020.140f, 28.379f),
        new Vector3(-1114.1f, 479.205f, 81.161f),
        new Vector3(-160.8898f, 275.334f, 92.95601f),
        new Vector3(-504.323f, 424.21f, 96.287f),
        new Vector3(-1405.12f, 81.983f, 52.099f),
        new Vector3(-1299.92f, -228.464f, 59.654f),
        new Vector3(-1334.63f, -1008.97f, 6.867f),
        new Vector3(-187.144f, -175.854f, 42.624f),
        new Vector3(-1886.25f, 2016.572f, 139.951f),
        new Vector3(-1616.693f, 5270.917f, -0.298f),
        new Vector3(-1297.2f, 252.495f, 61.813f),
        new Vector3(-345.267f, 662.299f, 168.587f),
        new Vector3(-72.605f, 902.579f, 234.631f),
        new Vector3(-1451.92f, 533.495f, 118.177f),
        new Vector3(-1979.25f, 586.078f, 116.479f),
        new Vector3(-1873.6f, -343.933f, 48.26f),
        new Vector3(443.542f, 253.197f, 102.21f),
        new Vector3(-2340.907f, 295.8933f, 169.1187f),
        new Vector3(627.8824f, 196.4409f, 96.67142f),
        new Vector3(1147.651f, -985.0583f, 45.4853f),
        new Vector3(243.1413f, -861.0181f, 28.94244f),
        new Vector3(-340.161f, -876.799f, 30.90968f),
        new Vector3(388.3879f, -215.6955f, 56.76986f),
        new Vector3(-1235.388f, -1647.45f, 3.512795f),
        new Vector3(-472.0576f, 6034.684f, 30.74616f),
        new Vector3(-198.5697f, 6273.029f, 31.48925f),
        new Vector3(2502.232f, 4080.495f, 38.63095f),
        new Vector3(1203.418f, -1262.387f, 35.22676f),
        new Vector3(-71.37413f, -1339.442f, 29.25686f),
        new Vector3(-464.9293f, -1718.74f, 18.66934f),
        new Vector3(934.148f, -1812.94f, 29.812f),
        new Vector3(246.847f, -1162.08f, 28.16f),
        new Vector3(1136.156f, -773.997f, 56.632f),
        new Vector3(1028.898f, -2405.95f, 28.494f),
        new Vector3(-552.673f, 309.154f, 82.191f),
        new Vector3(-762.865f, -38.192f, 37.687f),
        new Vector3(140.945f, 6606.513f, 30.845f),
        new Vector3(1362.672f, 1178.352f, 111.609f),
        new Vector3(2593.022f, 364.349f, 107.457f),
        new Vector3(2002.724f, 3769.429f, 31.181f),
        new Vector3(-771.927f, 5566.46f, 32.486f),
        new Vector3(1697.817f, 6414.365f, 31.73f),
        new Vector3(1700.445f, 4937.267f, 41.078f),
        new Vector3(-1804.77f, 804.137f, 137.514f),
        new Vector3(756.539f, 2525.957f, 72.161f),
        new Vector3(1205.454f, 2658.357f, 36.824f),
        new Vector3(-165.839f, 6454.25f, 30.495f),
        new Vector3(-2221.14f, 4232.757f, 46.132f),
        new Vector3(-2555.51f, 2322.827f, 32.06f),
        new Vector3(1111.018f, 2221.073f, 50.140f),
        new Vector3(-3092.066f, 3465.729f, -0.474f),
};

    private List<float> heading = new List<float>()
    {
        36.65652f,
        12.163f,
        338.279f,
        26.557f,
        27.92156f,
        30.12479f,
        298.6727f,
        318.133f,
        358.1453f,
        147.4842f,
        341.667f,
        341.667f,
        341.667f,
        66.978f,
        66.226f,
        169.408f,
        294.730f,
        341.133f,
        71.345f,
        352.5828f,
        1.6991f,
        255.056f,
        270.8829f,
        99.87038f,
        146.244f,
        141.262f,
        47.597f,
        138.808f,
        33.185f,
        331.180f,
        166.594f,
        14.900f,
        270.297f,
        245.553f,
        1.832709f,
        254.258f,
        327.087f,
        240.052f,
        239.996f,
        344.361f,
        60.516f,
        329.284f,
        149.870f,
        60.084f,
        151.765f,
        5.327f,
        145.951f,
        147.141f,
        150.073f,
        236.324f,
        147.964f,
        150.114f,
        239.2013f,
        61.37347f,
        237.5298f,
        201.5139f,
        141.262f,
        311.2028f,
        19.50508f,
        98.82303f,
        358.6906f,
        133.0528f,
        23.36283f,
        88.68892f,
        340.165f,
        90.89349f,
        321.0109f,
        33.185f,
        205.5221f,
        58.8943f,
        297.7102f,
        16.77524f,
        294.684f,
        276.757f,
        147.589f,
        35.312f,
        29.408f,
        198.4456f,
        240.586f,
        230.537f,
        268.8033f,
        241.2007f,
        353.1183f,
        48.3741f,
        160.515f,
        260.5882f,
        130.5594f,
        270.200f,
        171.220f,
        176.2266f,
        313.167f,
        53.145f,
        126.968f,
        118.474f,
        160.257f,
        174.739f,
        304.052f,
        181.166f,
        171.211f,
        291.351f,
        73.674f,
        185.087f,
        225.300f,
        245.845f,
        294.0081f,
        70.38502f,
        182.3871f,
        248.5342f,
        347.7794f,
        341.7853f,
        124.5176f,
        43.40757f,
        313.6799f,
        68.68851f,
        178.5483f,
        89.01824f,
        244.1471f,
        88.712f,
        180.390f,
        269.604f,
        170.017f,
        260.340f,
        115.427f,
        0.239f,
        359.306f,
        174.745f,
        298.783f,
        271.230f,
        247.870f,
        147.276f,
        223.747f,
        270.240f,
        223.627f,
        225.116f,
        225.108f,
        273.837f,
        273.390f,
        47.552f,
};

    public SpawnMP()
    {
        config = ScriptSettings.Load("Scripts\\AllMpVehiclesInSp.ini");
        doors_config = config.GetValue<int>("MAIN", "doors", 1);
        blip_config = config.GetValue<int>("MAIN", "blips", 1);
        tuning_flag = config.GetValue<int>("MAIN", "tuning", 1);
        mod_plate = config.GetValue<int>("MAIN", "new_license_plates", 0);
        blip_color = config.GetValue<int>("MAIN", "blip_color", 3);

        char symbol = '#';
        string[] lines = File.ReadAllLines("Scripts\\mp_blacklist.txt");
        string[] lines_addon = File.ReadAllLines("Scripts\\NewVehiclesList.txt");
        List<string> blacklist_str = new List<string>();
        List<string> new_list_str = new List<string>();

        foreach (string s in lines)
        {
            if (s.IndexOf(symbol) == -1)
                blacklist_str.Add(s);
        }

        foreach (string s in lines_addon)
        {
            if (s.IndexOf(symbol) == -1 && s.Length > 0)
            {
                new_list_str.Add(s);
            }
        }

        foreach (string line in new_list_str)
        {
            string[] veh_data = line.Split(',');
            try
            {
                AddCustomVehicle(veh_data[0], veh_data[1]);
            }
            catch
            {
                GTA.UI.Notification.Show("Error in loading the vehicle Add-On. Check if the entries in NewVehiclesList.txt are correct and try again.");
            }
        }
        
        foreach (string hash in blacklist_str)
        {
            if (VehList.models_arena.Contains(hash))
                VehList.models_arena.Remove(hash);

            if (VehList.models_beach.Contains(hash))
                VehList.models_beach.Remove(hash);

            if (VehList.models_boats.Contains(hash))
                VehList.models_boats.Remove(hash);

            if (VehList.models_cemetery.Contains(hash))
                VehList.models_cemetery.Remove(hash);

            if (VehList.models_cheburek.Contains(hash))
                VehList.models_cheburek.Remove(hash);

            if (VehList.models_cinema.Contains(hash))
                VehList.models_cinema.Remove(hash);

            if (VehList.models_cluckin.Contains(hash))
                VehList.models_cluckin.Remove(hash);

            if (VehList.models_compacts.Contains(hash))
                VehList.models_compacts.Remove(hash);

            if (VehList.models_coupes.Contains(hash))
                VehList.models_coupes.Remove(hash);

            if (VehList.models_cycles.Contains(hash))
                VehList.models_cycles.Remove(hash);

            if (VehList.models_ghetto.Contains(hash))
                VehList.models_ghetto.Remove(hash);

            if (VehList.models_helicopter.Contains(hash))
                VehList.models_helicopter.Remove(hash);

            if (VehList.models_humanlabs.Contains(hash))
                VehList.models_humanlabs.Remove(hash);

            if (VehList.models_industrial.Contains(hash))
                VehList.models_industrial.Remove(hash);

            if (VehList.models_karting.Contains(hash))
                VehList.models_karting.Remove(hash);

            if (VehList.models_military_bikes.Contains(hash))
                VehList.models_military_bikes.Remove(hash);

            if (VehList.models_military_helicopters.Contains(hash))
                VehList.models_military_helicopters.Remove(hash);

            if (VehList.models_military_opressors.Contains(hash))
                VehList.models_military_opressors.Remove(hash);

            if (VehList.models_military_planes.Contains(hash))
                VehList.models_military_planes.Remove(hash);

            if (VehList.models_motorcycles.Contains(hash))
                VehList.models_motorcycles.Remove(hash);

            if (VehList.models_muscle.Contains(hash))
                VehList.models_muscle.Remove(hash);

            if (VehList.models_offroad.Contains(hash))
                VehList.models_offroad.Remove(hash);

            if (VehList.models_openwheel.Contains(hash))
                VehList.models_openwheel.Remove(hash);

            if (VehList.models_planes.Contains(hash))
                VehList.models_planes.Remove(hash);

            if (VehList.models_police.Contains(hash))
                VehList.models_police.Remove(hash);

            if (VehList.models_sedans.Contains(hash))
                VehList.models_sedans.Remove(hash);

            if (VehList.models_slawmantruck.Contains(hash))
                VehList.models_slawmantruck.Remove(hash);

            if (VehList.models_sportclassic.Contains(hash))
                VehList.models_sportclassic.Remove(hash);

            if (VehList.models_submarine.Contains(hash))
                VehList.models_submarine.Remove(hash);

            if (VehList.models_supers.Contains(hash))
                VehList.models_supers.Remove(hash);

            if (VehList.models_suvs.Contains(hash))
                VehList.models_suvs.Remove(hash);

            if (VehList.models_towtruck.Contains(hash))
                VehList.models_towtruck.Remove(hash);

            if (VehList.models_tuners.Contains(hash))
                VehList.models_tuners.Remove(hash);

            if (VehList.models_valentine.Contains(hash))
                VehList.models_valentine.Remove(hash);

            if (VehList.models_vans.Contains(hash))
                VehList.models_vans.Remove(hash);

            if (VehList.models_wastelander.Contains(hash))
                VehList.models_wastelander.Remove(hash);

            if (VehList.models_weaponboats.Contains(hash))
                VehList.models_weaponboats.Remove(hash);
        }

       

        Tick += OnTick;
        KeyUp += onkeyup;
        Aborted += OnAborded;
    }

    public enum Nodetype
    {
        AnyRoad,
        Road,
        Offroad,
        Water
    }

    void AddCustomVehicle(string Model, string Class)
    {
        switch(Class)
        {
            case "boats":
                VehList.models_boats.Add(Model);
                break;

            case "commercial":
                VehList.models_industrial.Add(Model);
                break;

            case "compacts":
                VehList.models_compacts.Add(Model);
                break;

            case "coupes":
                VehList.models_coupes.Add(Model);
                break;

            case "cycles":
                VehList.models_cycles.Add(Model);
                break;

            case "emergency":
                VehList.models_industrial.Add(Model);
                break;

            case "helicopters":
                VehList.models_helicopter.Add(Model);
                break;

            case "industrial":
                VehList.models_industrial.Add(Model);
                break;

            case "karting":
                VehList.models_karting.Add(Model);
                break;

            case "motorcycles":
                VehList.models_motorcycles.Add(Model);
                break;

            case "muscle":
                VehList.models_muscle.Add(Model);
                break;

            case "openwheel":
                VehList.models_openwheel.Add(Model);
                break;

            case "offroad":
                VehList.models_offroad.Add(Model);
                break;

            case "planes":
                VehList.models_planes.Add(Model);
                break;

            case "sedans":
                VehList.models_sedans.Add(Model);
                break;

            case "service":
                VehList.models_industrial.Add(Model);
                break;

            case "sports":
                VehList.models_sportclassic.Add(Model);
                break;

            case "sportsclassics":
                VehList.models_sportclassic.Add(Model);
                break;

            case "super":
                VehList.models_supers.Add(Model);
                break;

            case "suvs":
                VehList.models_suvs.Add(Model);
                break;

            case "vans":
                VehList.models_vans.Add(Model);
                break;
        }
    }

    string GenerateVehicleModelName(int index_db, int type)
    {
        string model_name = null;
        bool isEmpty;
        var random = new Random();
        switch (index_db)
        {
            case arena:
                isEmpty = !VehList.models_arena.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_arena[random.Next(VehList.models_arena.Count)];
                    plate_id = 10;
                }
                break;

            case boats:
                isEmpty = !VehList.models_boats.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_boats[random.Next(VehList.models_boats.Count)];
                }
                break;

            case cemetery:
                isEmpty = !VehList.models_cemetery.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_cemetery[random.Next(VehList.models_cemetery.Count)];
                }
                break;

            case cheburek:
                isEmpty = !VehList.models_cheburek.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_cheburek[random.Next(VehList.models_cheburek.Count)];
                    plate_id = 8;
                }
                break;

            case cinema:
                isEmpty = !VehList.models_cinema.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_cinema[random.Next(VehList.models_cinema.Count)];
                    plate_id = 6;
                }
                break;

            case cluckin:
                isEmpty = !VehList.models_cluckin.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_cluckin[random.Next(VehList.models_cluckin.Count)];
                    plate_id = 6;
                }
                break;

            case compacts_1:
            case compacts_2:
            case compacts_3:
            case compacts_4:
            case compacts_5:
            case compacts_6:
                isEmpty = !VehList.models_compacts.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_compacts[random.Next(VehList.models_compacts.Count)];
                }
                break;

            case coupes_1:
            case coupes_2:
            case coupes_3:
            case coupes_4:
            case coupes_5:
            case coupes_6:
            case coupes_7:
                isEmpty = !VehList.models_coupes.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_coupes[random.Next(VehList.models_coupes.Count)];
                }
                break;

            case cycles_1:
            case cycles_2:
            case cycles_3:
            case cycles_4:
            case cycles_5:
                isEmpty = !VehList.models_cycles.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_cycles[random.Next(VehList.models_cycles.Count)];
                }
                break;

            case ghetto_1:
            case ghetto_2:
            case ghetto_3:
            case ghetto_4:
            case ghetto_5:
                isEmpty = !VehList.models_ghetto.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_ghetto[random.Next(VehList.models_ghetto.Count)];
                }
                break;

            case helicopter:
                isEmpty = !VehList.models_helicopter.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_helicopter[random.Next(VehList.models_helicopter.Count)];
                }
                break;

            case humanlabs:
                isEmpty = !VehList.models_humanlabs.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_humanlabs[random.Next(VehList.models_humanlabs.Count)];
                }
                break;

            case industrial_1:
            case industrial_2:
            case industrial_3:
            case industrial_4:
                isEmpty = !VehList.models_industrial.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_industrial[random.Next(VehList.models_industrial.Count)];
                }
                break;

            case karting:
                isEmpty = !VehList.models_karting.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_karting[random.Next(VehList.models_karting.Count)];
                }
                break;

            case vetir:
                if (veh[index_db] == null && VehList.vetir_model != "Blocked")
                {
                    model_name = VehList.models_karting[random.Next(VehList.models_karting.Count)];
                }
                break;

            case scarab:
                if (veh[index_db] == null && VehList.scarab_model != "Blocked")
                {
                    model_name = VehList.scarab_model;
                }
                break;

            case terrorbyte:
                if (veh[index_db] == null && VehList.terrorbyte_model != "Blocked")
                {
                    model_name = VehList.terrorbyte_model;
                }
                break;

            case thruster:
                if (veh[index_db] == null && VehList.thruster_model != "Blocked")
                {
                    model_name = VehList.thruster_model;
                }
                break;

            case khanjari:
                if (veh[index_db] == null && VehList.khanjari_model != "Blocked")
                {
                    model_name = VehList.khanjari_model;
                }
                break;

            case chernobog:
                if (veh[index_db] == null && VehList.chernobog_model != "Blocked")
                {
                    model_name = VehList.chernobog_model;
                }
                break;

            case barrage:
                if (veh[index_db] == null && VehList.barrage_model != "Blocked")
                {
                    model_name = VehList.barrage_model;
                }
                break;

            case trailerLarge:
                if (veh[index_db] == null && VehList.trailerLarge_model != "Blocked")
                {
                    model_name = VehList.trailerLarge_model;
                }
                break;

            case halfTrack:
                if (veh[index_db] == null && VehList.halfTrack_model != "Blocked")
                {
                    model_name = VehList.halfTrack_model;
                }
                break;

            case apc:
                if (veh[index_db] == null && VehList.apc_model != "Blocked")
                {
                    model_name = VehList.apc_model;
                }
                break;

            case trailerSmall2:
                if (veh[index_db] == null && VehList.trailerSmall2_model != "Blocked")
                {
                    model_name = VehList.trailerSmall2_model;
                }
                break;

            case military_planes_1:
            case military_planes_2:
                isEmpty = !VehList.models_military_planes.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_military_planes[random.Next(VehList.models_military_planes.Count)];
                }
                break;

            case military_helicopters:
                isEmpty = !VehList.models_military_helicopters.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_military_helicopters[random.Next(VehList.models_military_helicopters.Count)];
                }
                break;

            case military_opressors:
                isEmpty = !VehList.models_military_opressors.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_military_opressors[random.Next(VehList.models_military_opressors.Count)];
                }
                break;

            case military_bikes:
                isEmpty = !VehList.models_military_bikes.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_military_bikes[random.Next(VehList.models_military_bikes.Count)];
                }
                break;

            case raiju:
                if (veh[index_db] == null && VehList.raiju_model != "Blocked")
                {
                    model_name = VehList.raiju_model;
                }
                break;

            case streamer216:
                if (veh[index_db] == null && VehList.streamer216_model != "Blocked")
                {
                    model_name = VehList.streamer216_model;
                }
                break;

            case conada2:
                if (veh[index_db] == null && VehList.conada2_model != "Blocked")
                {
                    model_name = VehList.conada2_model;
                }
                break;

            case motorcycles_1:
            case motorcycles_2:
            case motorcycles_3:
            case motorcycles_4:
            case motorcycles_5:
            case motorcycles_6:
                isEmpty = !VehList.models_motorcycles.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_motorcycles[random.Next(VehList.models_motorcycles.Count)];
                }
                break;

            case muscle_1:
            case muscle_2:
            case muscle_3:
            case muscle_4:
            case muscle_5:
            case muscle_6:
            case muscle_7:
                isEmpty = !VehList.models_muscle.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_muscle[random.Next(VehList.models_muscle.Count)];
                }
                break;

            case offroad_1:
            case offroad_2:
            case offroad_3:
            case offroad_4:
                isEmpty = !VehList.models_offroad.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_offroad[random.Next(VehList.models_offroad.Count)];
                }
                break;

            case openwheel:
                isEmpty = !VehList.models_openwheel.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_openwheel[random.Next(VehList.models_openwheel.Count)];
                }
                break;

            case beach:
                isEmpty = !VehList.models_beach.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_beach[random.Next(VehList.models_beach.Count)];
                }
                break;

            case planes:
                isEmpty = !VehList.models_planes.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_planes[random.Next(VehList.models_planes.Count)];
                }
                break;

            case police_1:
            case police_2:
            case police_3:
            case police_4:
            case police_5:
                isEmpty = !VehList.models_police.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_police[random.Next(VehList.models_police.Count)];
                }
                break;

            case sedans_1:
            case sedans_2:
            case sedans_3:
            case sedans_4:
            case sedans_5:
            case sedans_6:
            case sedans_7:
                isEmpty = !VehList.models_sedans.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_sedans[random.Next(VehList.models_sedans.Count)];
                }
                break;

            case slawmantruck:
                isEmpty = !VehList.models_slawmantruck.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_slawmantruck[random.Next(VehList.models_slawmantruck.Count)];
                }
                break;

            case sportclassic_1:
            case sportclassic_2:
            case sportclassic_3:
            case sportclassic_4:
            case sportclassic_5:
            case sportclassic_6:
            case sportclassic_7:
            case sportclassic_8:
                isEmpty = !VehList.models_sportclassic.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_sportclassic[random.Next(VehList.models_sportclassic.Count)];
                    plate_id = 7;
                }
                break;

            case submarine:
                isEmpty = !VehList.models_submarine.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_submarine[random.Next(VehList.models_submarine.Count)];
                }
                break;

            case supers_1:
            case supers_2:
            case supers_3:
            case supers_4:
            case supers_5:
            case supers_6:
            case supers_7:
                isEmpty = !VehList.models_supers.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_supers[random.Next(VehList.models_supers.Count)];
                }
                break;

            case suvs_1:
            case suvs_2:
            case suvs_3:
            case suvs_4:
            case suvs_5:
            case suvs_6:
            case suvs_7:
            case suvs_8:
                isEmpty = !VehList.models_suvs.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_suvs[random.Next(VehList.models_suvs.Count)];
                }
                break;

            case towtruck_1:
            case towtruck_2:
            case towtruck_3:
            case towtruck_4:
            case towtruck_5:
                isEmpty = !VehList.models_towtruck.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_towtruck[random.Next(VehList.models_towtruck.Count)];
                }
                break;

            case tuners_1:
            case tuners_2:
            case tuners_3:
            case tuners_4:
            case tuners_5:
                isEmpty = !VehList.models_tuners.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_tuners[random.Next(VehList.models_tuners.Count)];
                }
                break;

            case valentine:
                isEmpty = !VehList.models_valentine.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_valentine[random.Next(VehList.models_valentine.Count)];
                }
                break;

            case vans_1:
            case vans_2:
            case vans_3:
            case vans_4:
            case vans_5:
            case vans_6:
            case vans_7:
            case vans_8:
            case vans_9:
            case vans_10:
            case vans_11:
            case vans_12:
            case vans_13:
                isEmpty = !VehList.models_vans.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_vans[random.Next(VehList.models_vans.Count)];
                }
                break;

            case wastelander:
                isEmpty = !VehList.models_wastelander.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_wastelander[random.Next(VehList.models_wastelander.Count)];
                }
                break;

            case weaponboats:
                isEmpty = !VehList.models_weaponboats.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = VehList.models_weaponboats[random.Next(VehList.models_weaponboats.Count)];
                }
                break;
        }
        return model_name;
    }

    void SetNumberPlate(Vehicle car, int mode, int index)
    {
        if (mode == 1 && plate_id != -1)
        {
            Function.Call(Hash.SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX, car, index);
        }
    }

    Vehicle CreateNewVehicle(string hash, Vector3 pos, float heading)
    {
        var veh_model = new Model(hash);
        veh_model.Request(500);
        if (!veh_model.IsValid)
        {
            GTA.UI.Notification.Show($"{hash} is invalid model! Please add this model to mp_blacklist.txt");
            return null;
        }
        else
        {
            while (!veh_model.IsLoaded) Script.Wait(100);
            car = World.CreateVehicle(veh_model, pos, heading);

            if (doors_config == 1)
            {
                GTA.Native.Function.Call(GTA.Native.Hash.SET_VEHICLE_DOORS_LOCKED, car, 7);

            }

            if (tuning_flag == 1)
            {
                Random rnd = new Random();
                int num;
                int modindex;
                for (int a = 0; a <= 3; a++)
                {
                    mode_type[a] = rnd.Next(0, 17);
                    num = Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, car, mode_type[a]);
                    if (num != -1)
                    {
                        modindex = rnd.Next(0, num + 1);
                        Function.Call(Hash.SET_VEHICLE_MOD, car, mode_type[a], modindex, true);
                    }
                }
                if (Function.Call<bool>(Hash.IS_THIS_MODEL_A_BIKE, veh_model))
                {
                    num = Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, car, 24);
                    modindex = rnd.Next(0, num + 1);
                    Function.Call(Hash.SET_VEHICLE_MOD, car, 24, modindex, true);
                }
                else
                {
                    num = Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, car, 23);
                    modindex = rnd.Next(0, num + 1);
                    Function.Call(Hash.SET_VEHICLE_MOD, car, 23, modindex, true);
                }
                int choose = rnd.Next(1, 3);
                if (choose == 1)
                {
                    num = Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, car, 48);
                    if (num != -1)
                    {
                        modindex = rnd.Next(0, num + 1);
                        Function.Call(Hash.SET_VEHICLE_MOD, car, 48, modindex, true);
                    }
                }
                else
                {
                    modindex = rnd.Next(0, 7);
                    num = Function.Call<int>(Hash.GET_NUM_MOD_COLORS, 6, true);
                    int color_1 = rnd.Next(0, num + 1);
                    int color_2 = rnd.Next(0, num + 1);
                    Function.Call(Hash.SET_VEHICLE_MOD_COLOR_1, car, modindex, color_1, 0);
                    Function.Call(Hash.SET_VEHICLE_MOD_COLOR_2, car, modindex, color_2, 0);
                }
            }

            vehicles_spawned = 1;

            return car;
        }

    }

    Blip CreateMarkerAboveCar(Vehicle car)
    {
        Blip mark = Function.Call<Blip>(GTA.Native.Hash.ADD_BLIP_FOR_ENTITY, car);
        Function.Call(GTA.Native.Hash.SET_BLIP_SPRITE, mark, 1);
        Function.Call(GTA.Native.Hash.SET_BLIP_COLOUR, mark, blip_color);
        Function.Call(GTA.Native.Hash.FLASH_MINIMAP_DISPLAY);
        mark.Name = "Unique vehicle";
        return mark;
    }

    void OnAborded(object sender, EventArgs e)
    {
        foreach (Blip mark in marker)
        {
            if (mark != null && mark.Exists())
                mark.Delete();
        }

        foreach (Vehicle car in veh)
        {
            if (car != null && car.Exists())
                car.Delete();
        }
    }

    void OnTick(object sender, EventArgs e)
    {

        if (vehicles_spawned == 1 && Function.Call<bool>(Hash.GET_MISSION_FLAG))
        {
            foreach (Vehicle car in veh)
            {
                if (car.Exists())
                {
                    car.Delete();
                }
            }

            vehicles_spawned = 0;
        }


        int index_db = 0;

        //Create vehicles (ON_MISSION = 0)
        if (!Function.Call<bool>(Hash.GET_MISSION_FLAG))
        {
            foreach (Vector3 veh_coords in coords)
            {
                var position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));
                if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, veh_coords.X, veh_coords.Y, veh_coords.Z, position.X, position.Y, position.Z, 0) < 300)
                {
                    //spawn in parking lots
                    string model_name = GenerateVehicleModelName(index_db, 0);
                    if (model_name != null)
                    {
                        veh[index_db] = CreateNewVehicle(model_name, coords[index_db], heading[index_db]);
                        if (veh[index_db] != null)
                        {
                            SetNumberPlate(veh[index_db], mod_plate, plate_id);
                            plate_id = -1;
                            if (blip_config == 1)
                            {
                                Blip mark = CreateMarkerAboveCar(veh[index_db]);
                                marker.Add(mark);
                            }
                        }
                    }
                }
                index_db++;
            }
        }

        //Player in car
        index_db = 0;
        foreach (Vehicle car in veh)
        {
            if (car != null && car.Exists() && Function.Call<bool>(Hash.IS_PED_IN_VEHICLE, Game.Player.Character, car, false))
            {
                Blip mark = Function.Call<Blip>(Hash.GET_BLIP_FROM_ENTITY, car);
                if (mark != null && mark.Exists())
                    mark.Delete();
                car.MarkAsNoLongerNeeded();
            }
            index_db++;
        }
        index_db = 0;
        foreach (Vehicle car in street_veh)
        {
            if (car != null && car.Exists() && Function.Call<bool>(Hash.IS_PED_IN_VEHICLE, Game.Player.Character, car, false))
            {
                car.MarkAsNoLongerNeeded();
                street_veh[index_db] = null;
            }
            index_db++;
        }

        index_db = 0;
        foreach (Vector3 veh_coords in coords)
        {
            var position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 0, 0));
            if (Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, veh_coords.X, veh_coords.Y, veh_coords.Z, position.X, position.Y, position.Z, 0) > 300)
            {
                if (veh[index_db] != null && veh[index_db].Exists() && !Function.Call<bool>(Hash.IS_PED_SITTING_IN_VEHICLE, Game.Player.Character, veh[index_db]))
                {
                    Blip mark = Function.Call<Blip>(Hash.GET_BLIP_FROM_ENTITY, veh[index_db]);
                    if (mark != null && mark.Exists())
                        mark.Delete();
                    veh[index_db].Delete();
                }
                veh[index_db] = null;
            }
            index_db++;
        }
    }

    private void onkeyup(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.N)
        {
            var veh_model = new Model("gstghell1");
            veh_model.Request(500);
            if (!veh_model.IsValid)
            {
                GTA.UI.Notification.Show("gstghell1 is invalid model! Please add this model to mp_blacklist.txt");
            }
            else
            {
                var position = Game.Player.Character.GetOffsetPosition(new Vector3(0, 5, 0));
                var heading = Game.Player.Character.Heading - 90;
                var vehicle = World.CreateVehicle(veh_model, position, heading);
            }
        }
    }
}
