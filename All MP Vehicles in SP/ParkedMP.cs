using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTA;
using GTA.Native;
using GTA.Math;
using System.Drawing;
using System.Reflection;
using System.IO;

public static class Notifier
{
    private static MethodInfo _method;
    private static bool _resolved;

    public static void Show(string message)
    {
        try
        {
            if (!_resolved)
            {
                _resolved = true;
                Type t = typeof(GTA.UI.Screen).Assembly.GetType("GTA.UI.Notification");
                if (t != null)
                {
                    _method = t.GetMethod("Show", new[] { typeof(string), typeof(bool) })
                           ?? t.GetMethod("Show", new[] { typeof(string) })
                           ?? t.GetMethod("PostTicker", new[] { typeof(string), typeof(bool), typeof(bool) })
                           ?? t.GetMethod("PostTicker", new[] { typeof(string), typeof(bool) });
                }
            }

            if (_method != null)
            {
                var ps = _method.GetParameters();
                object[] args = new object[ps.Length];
                args[0] = message;
                for (int i = 1; i < ps.Length; i++) args[i] = false;
                _method.Invoke(null, args);
            }
        }
        catch
        {
            
        }
    }
}

public class SpawnMP : Script
{
    ScriptSettings config;
    private int doors_config = 0;
    private int blip_config = 0;
    private int tuning_flag;
    private int tuning_hsw_flag;
    private int random_colors_flag; 
    private int blip_color;
    private int mod_plate;
    private int plate_id = -1;
    private bool IsHSW = false;
    private Vehicle[] veh = new Vehicle[200];
    private bool[] _claimed = new bool[200];
    // Blips tracked per slot rather than in a flat list, so a blip can be removed even
    // when its vehicle was destroyed by the game and GET_BLIP_FROM_ENTITY no longer works.
    private Blip[] _slotBlip = new Blip[200];
    private int debugging = 0;
    private int _canSpawn = 1;
    private string mod_version = "1.73";

    private bool _wasRestrictedState = false;
    private int _resumeSpawnTime = 0;

    private const int MissionGraceMs = 5000;

    private const int MaxSpawnAttemptsPerTick = 1;

    private const float SpawnRadius = 300f;

    // The 152-coord sweep only needs redoing when the player has actually moved, or
    // periodically as a safety net. Running it every tick was a measurable hitch.
    private const float RescanMoveDistance = 25f;
    private const int RescanIntervalMs = 1000;
    private Vector3 _lastScanPosition = Vector3.Zero;
    private int _nextScanTime = 0;
    private bool _forceRescan = true;

    // Model.Request blocks the script thread. Cap the wait per tick and remember models
    // that never stream in so a bad add-on cannot stall every tick forever.
    private const int ModelRequestTimeoutMs = 250;
    private const int ModelRequestMaxWaitMs = 5000;
    private readonly Dictionary<string, int> _modelWaitMs = new Dictionary<string, int>();
    private readonly HashSet<string> _badModels = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

    private static readonly Random _rnd = new Random();

    private const int arena = 0;
    private const int boats = 1;
    private const int cemetery = 2;
    private const int cheburek = 3;
    private const int cinema = 4;
    private const int cluckin = 5;
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
    private const int raiju = 52;
    private const int streamer216 = 53;
    private const int conada2 = 54;
    private const int hsw = 149;

    private readonly List<string>[] _listByIndex = new List<string>[152];
    private readonly Dictionary<int, Func<string>> _singleByIndex = new Dictionary<int, Func<string>>();
    private readonly Dictionary<int, int> _plateByIndex = new Dictionary<int, int>();

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
        new Vector3(486.359f, -948.2272f, 26.64442f), 
        new Vector3(127.562f, 15.10451f, 68.00917f),
        new Vector3(-61.72556f, 6499.053f, 30.99122f),
        new Vector3(120.8748f, -1709.281f, 28.58102f),
        new Vector3(-1420.277f, -655.5345f, 28.17369f),
        new Vector3(541.7154f, 97.22734f, 95.95358f),
        new Vector3(714.0365f, -981.6344f, 23.54063f),
        new Vector3(722.5751f, -981.2658f, 23.4088f),
        new Vector3(1705.741f, 3271.236f, 41.56281f),
        new Vector3(2140.588f, 4816.544f, 41.05009f),
        new Vector3(-2078.637f, 2931.623f, 33.99109f),
        new Vector3(792.5626f, -1862.284f, 28.52566f),
        new Vector3(-744.5989f, -1467.786f, 5.675299f),
        new Vector3(-1864.326f, 3225.93f, 32.17207f),

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
        90.42657f,
        -110.5593f,
        139.6896f,
        48.76577f,
        -54.15135f,
        113.416f,
        -31.42274f,
        60.22583f,
        -179.1466f,
        115.5491f,
        58.04224f,
        167.2133f,
        -40.04208f,
        327.426f,

};

    public SpawnMP()
    {
        try
        {
            Initialize();
        }
        catch (Exception ex)
        {
            _canSpawn = 0;
            VehList.lists_ready = true;   // don't leave TrafficMP waiting on a failed init
            Notifier.Show("~r~All MP Vehicles in SP:~w~ init error: " + ex.Message);
        }

        Tick += OnTick;
        Aborted += OnAborded;
    }

    private void Initialize()
    {
        string onlineVersion = Function.Call<string>(Hash.GET_ONLINE_VERSION);

        // Compare numerically and only warn when the game is genuinely OLDER. A plain
        // string mismatch also fired on newer games, which is where the "update your mod"
        // reports came from - a newer game is fine, it just may add vehicles we don't list.
        if (IsGameOlderThan(onlineVersion, mod_version))
        {
            Notifier.Show("~r~WARNING:\n~s~Your version of the game is out of date. ~g~All MP Vehicles in SP ~s~will not be able to load vehicles from new updates.\n\nRequired Game Version:\n" + mod_version + "\nYour Game Version: " + onlineVersion);
        }

        config = ScriptSettings.Load("Scripts\\AllMpVehiclesInSp.ini");
        _canSpawn = config.GetValue<int>("MAIN", "parking_lots_spawn", 1);
        doors_config = config.GetValue<int>("MAIN", "doors", -1);
        blip_config = config.GetValue<int>("MAIN", "blips", -1);
        tuning_flag = config.GetValue<int>("MAIN", "tuning", -1);
        tuning_hsw_flag = config.GetValue<int>("MAIN", "tuning_hsw", -1);
        mod_plate = config.GetValue<int>("MAIN", "new_license_plates", -1);
        blip_color = config.GetValue<int>("MAIN", "blip_color", -1);
        random_colors_flag = config.GetValue<int>("MAIN", "random_colors", 0);
        debugging = config.GetValue<int>("MAIN", "show_errors", 0);

        if (doors_config == -1) { doors_config = 1; config.SetValue<int>("MAIN", "doors", 1); }
        if (blip_config == -1) { blip_config = 1; config.SetValue<int>("MAIN", "blips", 1); }
        if (tuning_flag == -1) { tuning_flag = 1; config.SetValue<int>("MAIN", "tuning", 1); }
        if (tuning_hsw_flag == -1) { tuning_hsw_flag = 1; config.SetValue<int>("MAIN", "tuning_hsw", 1); }
        if (mod_plate == -1) { mod_plate = 0; config.SetValue<int>("MAIN", "new_license_plates", 0); }
        if (blip_color == -1) { blip_color = 3; config.SetValue<int>("MAIN", "blip_color", 3); }
        config.SetValue<int>("MAIN", "random_colors", random_colors_flag);

        config.Save();

        BuildIndexMaps();

        try
        {
            LoadCustomVehicles();
            ApplyBlacklist();
        }
        finally
        {
            // TrafficMP blocks on this flag; never leave it unset or traffic stays dead.
            VehList.lists_ready = true;
        }
    }

    /// <summary>
    /// Compares dotted version strings ("1.73", "1.7.3") component by component.
    /// Returns true only when <paramref name="actual"/> is strictly older than
    /// <paramref name="required"/>; an unparseable version is treated as up to date.
    /// </summary>
    private static bool IsGameOlderThan(string actual, string required)
    {
        if (string.IsNullOrEmpty(actual) || string.IsNullOrEmpty(required)) return false;
        if (actual == required) return false;

        string[] a = actual.Split('.');
        string[] r = required.Split('.');
        int len = Math.Max(a.Length, r.Length);

        for (int i = 0; i < len; i++)
        {
            int av = 0, rv = 0;

            if (i < a.Length && !int.TryParse(a[i], out av)) return false;
            if (i < r.Length && !int.TryParse(r[i], out rv)) return false;

            if (av < rv) return true;
            if (av > rv) return false;
        }

        return false;
    }

    private void LoadCustomVehicles()
    {
        if (!File.Exists("Scripts\\NewVehiclesList.txt")) return;

        char symbol = '#';
        string[] lines_addon = File.ReadAllLines("Scripts\\NewVehiclesList.txt");

        foreach (string s in lines_addon)
        {
            if (s.IndexOf(symbol) != -1 || s.Trim().Length == 0) continue;

            string[] veh_data = s.Split(',');
            try
            {
                AddCustomVehicle(veh_data[0].Trim(), veh_data[1].Trim());
            }
            catch
            {
                Notifier.Show("Error in loading the vehicle Add-On. Check if the entries in NewVehiclesList.txt are correct and try again.");
            }
        }
    }

    private void ApplyBlacklist()
    {
        if (!File.Exists("Scripts\\mp_blacklist.txt")) return;

        char symbol = '#';
        string[] lines = File.ReadAllLines("Scripts\\mp_blacklist.txt");

        FieldInfo[] fields = typeof(VehList).GetFields(BindingFlags.Public | BindingFlags.Static);

        foreach (string raw in lines)
        {
            if (raw.IndexOf(symbol) != -1) continue;
            string hash = raw.Trim();
            if (hash.Length == 0) continue;

            foreach (var field in fields)
            {
                if (field.FieldType == typeof(List<string>))
                {
                    List<string> list = (List<string>)field.GetValue(null);
                    if (list != null) list.RemoveAll(m => m == hash);
                }
                else if (field.FieldType == typeof(string))
                {
                    string value = (string)field.GetValue(null);
                    if (value == hash) field.SetValue(null, "Blocked");
                }
            }

            if (hash == "vivanite2") TrafficMP.disableTaxiFlag = 1;
        }
    }

    private void BuildIndexMaps()
    {
        Map(VehList.models_arena, 0);
        Map(VehList.models_boats, 1);
        Map(VehList.models_cemetery, 2);
        Map(VehList.models_cheburek, 3);
        Map(VehList.models_cinema, 4);
        Map(VehList.models_cluckin, 5);
        MapRange(VehList.models_compacts, 6, 11);
        MapRange(VehList.models_coupes, 12, 18);
        MapRange(VehList.models_cycles, 19, 23);
        MapRange(VehList.models_ghetto, 24, 28);
        Map(VehList.models_helicopter, 29);
        Map(VehList.models_humanlabs, 30);
        MapRange(VehList.models_industrial, 31, 34);
        Map(VehList.models_karting, 35);
        MapRange(VehList.models_military_planes, 47, 48);
        Map(VehList.models_military_helicopters, 49);
        Map(VehList.models_military_opressors, 50);
        Map(VehList.models_military_bikes, 51);
        MapRange(VehList.models_motorcycles, 55, 60);
        MapRange(VehList.models_muscle, 61, 67);
        MapRange(VehList.models_offroad, 68, 71);
        Map(VehList.models_openwheel, 72);
        Map(VehList.models_beach, 73);
        Map(VehList.models_planes, 74);
        MapRange(VehList.models_police, 75, 79);
        MapRange(VehList.models_sedans, 80, 86);
        Map(VehList.models_slawmantruck, 87);
        MapRange(VehList.models_sportclassic, 88, 95);
        Map(VehList.models_submarine, 96);
        MapRange(VehList.models_supers, 97, 103);
        MapRange(VehList.models_suvs, 104, 111);
        MapRange(VehList.models_towtruck, 112, 116);
        MapRange(VehList.models_tuners, 117, 121);
        Map(VehList.models_valentine, 122);
        MapRange(VehList.models_vans, 123, 135);
        Map(VehList.models_wastelander, 136);
        Map(VehList.models_weaponboats, 137);
        MapRange(VehList.models_enforcement, 138, 142);
        Map(VehList.models_pizza, 143);
        Map(VehList.models_bros_1, 144);
        Map(VehList.models_bros_2, 145);
        Map(VehList.models_plane_sandy, 146);
        Map(VehList.models_heli_sandy, 147);
        Map(VehList.models_titan2, 148);
        Map(VehList.models_hsw, 149);
        Map(VehList.models_higgins, 150);
        Map(VehList.models_ignus2, 151);

        _singleByIndex[vetir] = () => VehList.vetir_model;
        _singleByIndex[scarab] = () => VehList.scarab_model;
        _singleByIndex[terrorbyte] = () => VehList.terrorbyte_model;
        _singleByIndex[thruster] = () => VehList.thruster_model;
        _singleByIndex[khanjari] = () => VehList.khanjari_model;
        _singleByIndex[chernobog] = () => VehList.chernobog_model;
        _singleByIndex[barrage] = () => VehList.barrage_model;
        _singleByIndex[trailerLarge] = () => VehList.trailerLarge_model;
        _singleByIndex[halfTrack] = () => VehList.halfTrack_model;
        _singleByIndex[apc] = () => VehList.apc_model;
        _singleByIndex[trailerSmall2] = () => VehList.trailerSmall2_model;
        _singleByIndex[raiju] = () => VehList.raiju_model;
        _singleByIndex[streamer216] = () => VehList.streamer216_model;
        _singleByIndex[conada2] = () => VehList.conada2_model;

        _plateByIndex[arena] = 10;
        _plateByIndex[cheburek] = 8;
        _plateByIndex[cinema] = 6;
        _plateByIndex[cluckin] = 6;
        for (int i = 88; i <= 95; i++) _plateByIndex[i] = 7;
    }

    private void Map(List<string> list, int index)
    {
        _listByIndex[index] = list;
    }

    private void MapRange(List<string> list, int from, int to)
    {
        for (int i = from; i <= to; i++) _listByIndex[i] = list;
    }

    void AddCustomVehicle(string Model, string Class)
    {
        switch (Class)
        {
            case "boats": VehList.models_boats.Add(Model); break;
            case "commercial": VehList.models_industrial.Add(Model); break;
            case "compacts": VehList.models_compacts.Add(Model); break;
            case "coupes": VehList.models_coupes.Add(Model); break;
            case "cycles": VehList.models_cycles.Add(Model); break;
            case "emergency": VehList.models_industrial.Add(Model); break;
            case "helicopters": VehList.models_helicopter.Add(Model); break;
            case "industrial": VehList.models_industrial.Add(Model); break;
            case "karting": VehList.models_karting.Add(Model); break;
            case "motorcycles": VehList.models_motorcycles.Add(Model); break;
            case "muscle": VehList.models_muscle.Add(Model); break;
            case "openwheel": VehList.models_openwheel.Add(Model); break;
            case "offroad": VehList.models_offroad.Add(Model); break;
            case "planes": VehList.models_planes.Add(Model); break;
            case "sedans": VehList.models_sedans.Add(Model); break;
            case "service": VehList.models_industrial.Add(Model); break;
            case "sports": VehList.models_sportclassic.Add(Model); break;
            case "sportsclassics": VehList.models_sportclassic.Add(Model); break;
            case "super": VehList.models_supers.Add(Model); break;
            case "suvs": VehList.models_suvs.Add(Model); break;
            case "vans": VehList.models_vans.Add(Model); break;

            default:
                if (debugging == 1)
                    Notifier.Show("~y~NewVehiclesList.txt:~w~ unknown class \"" + Class + "\" for \"" + Model + "\"");
                return;
        }

        VehList.addon_models.Add(Model);
    }

    string GenerateVehicleModelName(int index_db)
    {
        plate_id = -1;
        IsHSW = false;

        Func<string> single;
        if (_singleByIndex.TryGetValue(index_db, out single))
        {
            string m = single();
            if (string.IsNullOrEmpty(m) || m == "Blocked") return null;
            return m;
        }

        if (index_db < 0 || index_db >= _listByIndex.Length) return null;

        List<string> list = _listByIndex[index_db];
        if (list == null || list.Count == 0) return null;

        int plate;
        if (_plateByIndex.TryGetValue(index_db, out plate)) plate_id = plate;
        if (index_db == hsw) IsHSW = true;

        return list[_rnd.Next(list.Count)];
    }

    void SetNumberPlate(Vehicle car, int mode, int index)
    {
        if (mode == 1 && index != -1)
        {
            Function.Call(Hash.SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX, car, index);
        }
    }

    Vehicle CreateNewVehicle(string hash, Vector3 pos, float heading)
    {
        if (_badModels.Contains(hash)) return null;

        var veh_model = new Model(hash);

        if (!veh_model.IsValid || !veh_model.IsInCdImage)
        {
            _badModels.Add(hash);
            return null;
        }

        // A single Request(2000) froze the script thread for up to two seconds, which is
        // what users saw as a periodic stutter. Wait a short slice per tick instead and
        // resume on the next one; give up on the model once the total wait is unreasonable.
        if (!veh_model.IsLoaded && !veh_model.Request(ModelRequestTimeoutMs))
        {
            int waited;
            _modelWaitMs.TryGetValue(hash, out waited);
            waited += ModelRequestTimeoutMs;

            if (waited >= ModelRequestMaxWaitMs)
            {
                _modelWaitMs.Remove(hash);
                _badModels.Add(hash);
                if (debugging == 1)
                    Notifier.Show("~y~All MP Vehicles in SP:~w~ model \"" + hash + "\" failed to stream in.");
            }
            else
            {
                _modelWaitMs[hash] = waited;
            }

            veh_model.MarkAsNoLongerNeeded();
            return null;
        }

        _modelWaitMs.Remove(hash);

        Vehicle newCar = World.CreateVehicle(veh_model, pos, heading);
        veh_model.MarkAsNoLongerNeeded();

        if (newCar == null || !newCar.Exists()) return null;

        if (doors_config == 1)
        {
            Function.Call(Hash.SET_VEHICLE_DOORS_LOCKED, newCar, 7);
        }

        if (random_colors_flag == 1)
        {
            Function.Call(Hash.SET_VEHICLE_COLOURS, newCar, _rnd.Next(0, 160), _rnd.Next(0, 160));
        }

        if (tuning_flag == 1)
        {
            Function.Call(Hash.SET_VEHICLE_MOD_KIT, newCar, 0);
            int num;
            int modindex;

            int choose = _rnd.Next(1, 3);
            if (choose == 1)
            {
                num = Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, newCar, 48);
                if (num != -1)
                {
                    modindex = _rnd.Next(0, num + 1);
                    Function.Call(Hash.SET_VEHICLE_MOD, newCar, 48, modindex, true);
                }
            }
            else
            {
                modindex = _rnd.Next(0, 7);
                num = Function.Call<int>(Hash.GET_NUM_MOD_COLORS, 6, true);
                int color_1 = _rnd.Next(0, num + 1);
                int color_2 = _rnd.Next(0, num + 1);
                Function.Call(Hash.SET_VEHICLE_MOD_COLOR_1, newCar, modindex, color_1, 0);
                Function.Call(Hash.SET_VEHICLE_MOD_COLOR_2, newCar, modindex, color_2, 0);
            }
        }

        if (tuning_hsw_flag == 1 && IsHSW)
        {
            Function.Call(Hash.SET_VEHICLE_MOD, newCar, 36, 0, 0); //HSW Base
            Function.Call(Hash.SET_VEHICLE_MOD, newCar, 34, 2, 0); //HSW Turbo

            int livery = _rnd.Next(1, 3);
            int mods = Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, newCar, 48) - livery;
            Function.Call(Hash.SET_VEHICLE_MOD, newCar, 48, mods, 0); //HSW Livery

            IsHSW = false;
        }

        return newCar;
    }

    Blip CreateMarkerAboveCar(Vehicle car)
    {
        Blip mark = Function.Call<Blip>(Hash.ADD_BLIP_FOR_ENTITY, car);
        Function.Call(Hash.SET_BLIP_SPRITE, mark, 1);
        Function.Call(Hash.SET_BLIP_COLOUR, mark, blip_color);
        mark.Name = "Unique vehicle";
        return mark;
    }

    /// <summary>
    /// Removes the blip belonging to a slot. Works from the tracked handle, so it still
    /// cleans up when the vehicle itself has already been destroyed by the game -
    /// GET_BLIP_FROM_ENTITY needs a live entity and used to leave those blips behind.
    /// </summary>
    void ReleaseSlotBlip(int index)
    {
        if (index < 0 || index >= _slotBlip.Length) return;

        Blip mark = _slotBlip[index];
        _slotBlip[index] = null;

        if (mark != null && mark.Exists()) mark.Delete();
    }

    void DeleteBlipOf(Vehicle car)
    {
        if (car == null || !car.Exists()) return;

        Blip mark = Function.Call<Blip>(Hash.GET_BLIP_FROM_ENTITY, car);
        if (mark != null && mark.Exists()) mark.Delete();
    }

    private bool IsRestrictedGameState()
    {
        if (Game.IsLoading) return true;
        if (Function.Call<bool>(Hash.GET_MISSION_FLAG)) return true;
        if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING)) return true;
        if (Function.Call<bool>(Hash.IS_PLAYER_SWITCH_IN_PROGRESS)) return true;

        Ped p = Game.Player.Character;
        if (p == null || !p.Exists() || p.IsDead) return true;

        return false;
    }

    private void CleanupAllSpawned()
    {
        Ped player = Game.Player.Character;

        for (int i = 0; i < veh.Length; i++)
        {
            Vehicle car = veh[i];
            if (car == null)
            {
                ReleaseSlotBlip(i);   // slot may still hold a blip from a car the game removed
                continue;
            }

            ReleaseSlotBlip(i);

            if (car.Exists())
            {
                DeleteBlipOf(car);

                if (player != null && player.Exists() && player.IsInVehicle(car))
                {
                    car.MarkAsNoLongerNeeded();
                    _claimed[i] = true;
                }
                else
                {
                    car.Delete();
                }
            }

            veh[i] = null;
        }

        _forceRescan = true;
    }

    void OnAborded(object sender, EventArgs e)
    {
        for (int i = 0; i < _slotBlip.Length; i++) ReleaseSlotBlip(i);

        foreach (Vehicle car in veh)
        {
            if (car != null && car.Exists())
            {
                DeleteBlipOf(car);
                car.Delete();
            }
        }
    }

    void OnTick(object sender, EventArgs e)
    {
        if (_canSpawn == 0)
            return;

        if (IsRestrictedGameState())
        {
            if (!_wasRestrictedState)
            {
                CleanupAllSpawned();
                _wasRestrictedState = true;
            }
            return;
        }

        if (_wasRestrictedState)
        {
            _wasRestrictedState = false;
            _resumeSpawnTime = Game.GameTime + MissionGraceMs;
        }

        if (Game.GameTime < _resumeSpawnTime) return;

        Ped player = Game.Player.Character;
        Vector3 position = player.Position;

        // Runs every tick: the player entering one of our cars has to be noticed at once,
        // otherwise its blip lingers and we may delete a car under the player.
        for (int i = 0; i < veh.Length; i++)
        {
            Vehicle entered = veh[i];
            if (entered != null && entered.Exists() && player.IsInVehicle(entered))
            {
                ReleaseSlotBlip(i);
                DeleteBlipOf(entered);
                entered.MarkAsNoLongerNeeded();
                veh[i] = null;
                _claimed[i] = true;
                _forceRescan = true;
            }
        }

        // Sweeping all 152 parking spots on every tick was pure overhead: the result only
        // changes when the player moves. Re-scan on movement, on a slow timer, or when a
        // spawn/despawn has just changed the picture.
        bool scanNow = _forceRescan
                       || Game.GameTime >= _nextScanTime
                       || position.DistanceTo(_lastScanPosition) >= RescanMoveDistance;

        if (!scanNow) return;

        _forceRescan = false;
        _lastScanPosition = position;
        _nextScanTime = Game.GameTime + RescanIntervalMs;

        int spawnAttemptsThisTick = 0;

        for (int i = 0; i < coords.Count; i++)
        {
            if (spawnAttemptsThisTick >= MaxSpawnAttemptsPerTick) break;
            if (veh[i] != null || _claimed[i]) continue;

            Vector3 veh_coords = coords[i];
            if (position.DistanceTo(veh_coords) >= SpawnRadius) continue;

            string model_name = GenerateVehicleModelName(i);
            if (model_name == null) continue;

            spawnAttemptsThisTick++;

            Vehicle newCar = CreateNewVehicle(model_name, veh_coords, heading[i]);
            if (newCar == null) continue;

            veh[i] = newCar;
            // Keep scanning on the next tick so a cluster of empty spots fills promptly
            // instead of one spot per rescan interval.
            _forceRescan = true;
            SetNumberPlate(newCar, mod_plate, plate_id);
            plate_id = -1;

            if (blip_config == 1)
                _slotBlip[i] = CreateMarkerAboveCar(newCar);

            //Optional mods (livery, colors, etc.)
            switch (model_name)
            {
                case "police3":
                    newCar.Mods.CustomPrimaryColor = Color.White;
                    newCar.Mods.CustomSecondaryColor = Color.Black;
                    Function.Call(Hash.SET_VEHICLE_MOD_KIT, newCar, 0);
                    Function.Call(Hash.SET_VEHICLE_MOD, newCar, 48, 0, false);
                    break;

                case "brickade2":
                    newCar.Mods.CustomPrimaryColor = Color.Black;
                    newCar.Mods.CustomSecondaryColor = Color.Black;
                    Function.Call(Hash.SET_VEHICLE_MOD_KIT, newCar, 0);
                    Function.Call(Hash.SET_VEHICLE_MOD, newCar, 48, 5, false);
                    break;
            }
        }

        for (int i = 0; i < coords.Count; i++)
        {
            Vehicle car = veh[i];

            // A car destroyed by the game (blown up, cleaned up by the engine, wrecked in
            // a mission) leaves its slot pointing at a dead handle and its blip orphaned -
            // that is the "blip with no vehicle" users reported. Reclaim the slot wherever
            // the vehicle is gone, not only when we are the ones deleting it.
            if (car != null && !car.Exists())
            {
                ReleaseSlotBlip(i);
                veh[i] = null;
                _forceRescan = true;
                continue;
            }

            if (position.DistanceTo(coords[i]) <= SpawnRadius) continue;

            if (car != null)
            {
                DeleteBlipOf(car);
                ReleaseSlotBlip(i);
                car.Delete();
                veh[i] = null;
                _forceRescan = true;
            }

            _claimed[i] = false;
        }
    }
}
