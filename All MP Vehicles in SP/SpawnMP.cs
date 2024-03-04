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
    private int doors_config = 0;
    private int blip_config = 0;
    private int x = 0;
    private float[] angle = new float[1];
    private GTA.Vehicle car;
    private int tuning_flag;
    private int street_flag;
    private int street_blip;
    private int mod_plate;
    private int cooldown = 0;
    private Vehicle[] veh = new Vehicle[200];
    private Vehicle[] street_veh = new Vehicle[200];
    private List<Blip> marker = new List<Blip>();

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

    int i = 0;
    char symbol = '#';
    string[] lines = File.ReadAllLines("Scripts\\mp_blacklist.txt");
    List<string> blacklist_str = new List<string>();

    private List<string> models_arena = new List<string>() {
    "boxville5",
    "zr380",
    "cerberus",
    "cerberus2",
    "cerberus3",
    "deathbike",
    "slamvan4",
    "dominator4",
    "impaler2"
    };

    private List<string> models_boats = new List<string>() {
    "longfin",
    "toro"
    };

    private List<string> models_cemetery = new List<string>() {
    "tornado6",
    "btype2",
    "sanctus",
    "lurcher",
    "brigham"
    };

    private List<string> models_cheburek = new List<string>() {
    "cheburek",
    "ratbike",
    "slamvan3"
    };

    private List<string> models_cinema = new List<string>() {
    "scramjet",
    "vigilante",
    "voltic2",
    "toreador",
    "jb7002",
    "deluxo",
    "stromberg",
    "rrocket",
    "shotaro",
    "dune5"
    };

    private List<string> models_cluckin = new List<string>() {
    "benson2"
    };

    private List<string> models_compacts = new List<string>() {
    "brioso",
    "brioso2",
    "brioso3",
    "weevil",
    "club",
    "kanjo",
    "asbo",
    "issi3"
    };

    private List<string> models_coupes = new List<string>() {
    "kanjosj",
    "postlude",
    "previon",
    "windsor2",
    "windsor",
    "fr36"
    };

    private List<string> models_cycles = new List<string>() {
    "inductor",
    "inductor2",
    };

    private List<string> models_ghetto = new List<string>() {
    "peyote3",
    "retinue2",
    "dynasty",
    "cheburek",
    "fagaloa",
    "tornado5",
    "youga4",
    "gburrito2",
    "eudora",
    "greenwood",
    "voodoo",
    };

    private List<string> models_helicopter = new List<string>() {
    "conada",
    "havok",
    "volatus",
    "supervolito",
    "supervolito2",
    "swift2",
    };

    private List<string> models_humanlabs = new List<string>() {
    "brickade2",
    };

    private List<string> models_industrial = new List<string>() {
    "pounder2",
    "mule4",
    "phantom3",
    "hauler2",
    "phantom2",
    "mule3",
    "boxville4",
    };

    private List<string> models_karting = new List<string>() {
    "veto",
    "veto2",
    };

    private string vetir_model = "vetir";
    private string scarab_model = "scarab";
    private string terrorbyte_model = "terbyte";
    private string thruster_model = "thruster";
    private string khanjari_model = "khanjali";
    private string chernobog_model = "chernobog";
    private string barrage_model = "barrage";
    private string trailerLarge_model = "trailerlarge";
    private string halfTrack_model = "halftrack";
    private string apc_model = "apc";
    private string trailerSmall2_model = "trailersmall2";
    private string raiju_model = "raiju";
    private string streamer216_model = "streamer216";
    private string conada2_model = "conada2";

    private List<string> models_military_planes = new List<string>() {
    "alkonost",
    "strikeforce",
    "avenger",
    "volatol",
    "nokota",
    "seabreeze",
    "pyro",
    "mogul",
    "howard",
    "bombushka",
    "molotok",
    "tula",
    "rogue",
    "starling",
    "alphaz1",
    "hydra",
    };

    private List<string> models_military_helicopters = new List<string>() {
    "annihilator2",
    "akula",
    "hunter",
    "valkyrie",
    "savage",
    };

    private List<string> models_military_opressors = new List<string>() {
    "oppressor",
    "oppressor2",
    };

    private List<string> models_military_bikes = new List<string>() {
    "squaddie",
    "manchez2",
    "winky",
    "insurgent3",
    };

    private List<string> models_motorcycles = new List<string>() {
    "powersurge",
    "manchez3",
    "reever",
    "shinobi",
    "manchez2",
    "stryder",
    "fcr2",
    "fcr",
    "diablous",
    "diablous2",
    "esskey",
    "vortex",
    "daemon2",
    "zombiea",
    "zombieb",
    "wolfsbane",
    "nightblade",
    "manchez",
    "hakuchou2",
    "faggio",
    "faggio3",
    "defiler",
    "chimera",
    "avarus",
    "cliffhanger",
    "gargoyle",
    "bf400",
    "vindicator",
    "lectro",
    "enduro",
    };

    private List<string> models_muscle = new List<string>() {
    "tahoma",
    "tulip2",
    "weevil2",
    "vigero2",
    "ruiner4",
    "buffalo4",
    "dominator7",
    "dominator8",
    "gauntlet5",
    "manana2",
    "dukes3",
    "yosemite2",
    "peyote2",
    "gauntlet4",
    "gauntlet3",
    "vamos",
    "deviant",
    "tulip",
    "clique",
    "imperator",
    "impaler",
    "dominator3",
    "ellie",
    "hustler",
    "hermes",
    "yosemite",
    "tampa3",
    "ruiner2",
    "sabreGT2",
    "virgo2",
    "virgo3",
    "faction3",
    "tampa",
    "nightshade",
    "moonbeam2",
    "moonbeam",
    "faction2",
    "faction",
    "chino2",
    "buccaneer2",
    "coquette3",
    "chino",
    "vigero",
    "slamVan2",
    "clique2",
    "buffalo5",
    "vigero3",
    "dominator9",
    "impaler6",
    };

    private List<string> models_offroad = new List<string>() {
    "boor",
    "draugur",
    "patriot3",
    "verus",
    "yosemite3",
    "outlaw",
    "zhaba",
    "everon",
    "vagrant",
    "hellion",
    "caracara2",
    "brutus",
    "monster3",
    "bruiser",
    "freecrawler",
    "menacer",
    "caracara",
    "kamacho",
    "riata",
    "nightShark",
    "technical3",
    "dune3",
    "blazer5",
    "blazer4",
    "rallyTruck",
    "trophyTruck",
    "trophyTruck2",
    "brawler",
    "technical",
    "insurgent",
    "guardian",
    "l35",
    "ratel",
    "monstrociti",
    "terminus",
    };

    private List<string> models_openwheel = new List<string>() {
    "openwheel1",
    "openwheel2",
    "formula",
    "formula2",
    "raptor",
    };

    private List<string> models_beach = new List<string>() {
    "pbus2",
    };

    private List<string> models_planes = new List<string>() {
    "microlight",
    "nimbus",
    "luxor2",
    "velum2",
    };

    private List<string> models_police = new List<string>() {
    "riot2",
    "polgauntlet",
    "police5",
    };

    private List<string> models_sedans = new List<string>() {
    "rhinehart",
    "deity",
    "cinquemila",
    "tailgater2",
    "warrener2",
    "glendale2",
    "stafford",
    "schafter3",
    "schafter4",
    "limo2",
    "schafter5",
    "schafter6",
    "cog552",
    "cog55",
    "cognoscenti2",
    "cognoscenti",
    "primo2",
    "asterope2",
    "impaler5",
    };

    private List<string> models_slawmantruck = new List<string>() {
    "slamtruck",
    };

    private List<string> models_sportclassic = new List<string>() {
    "everon2",
    "panthere",
    "r300",
    "sentinel4",
    "tenf2",
    "tenf",
    "sm722",
    "omnisegt",
    "corsita",
    "comet7",
    "cypher",
    "sultan3",
    "growler",
    "vectre",
    "comet6",
    "remus",
    "jester4",
    "rt3000",
    "zr350",
    "euros",
    "futo2",
    "italirsx",
    "penumbra2",
    "coquette4",
    "sugoi",
    "vstr",
    "sultan2",
    "imorgon",
    "komoda",
    "jugular",
    "zion3",
    "locust",
    "nebula",
    "neo",
    "issi7",
    "drafter",
    "paragon",
    "paragon2",
    "schlagen",
    "italigto",
    "swinger",
    "jester3",
    "michelli",
    "flashgt",
    "gb200",
    "hotring",
    "comet5",
    "z190",
    "neon",
    "revolter",
    "gt500",
    "viseris",
    "savestra",
    "streiter",
    "sentinel3",
    "raiden",
    "pariah",
    "comet4",
    "rapidgt3",
    "retinue",
    "ardent",
    "torero",
    "cheetah2",
    "turismo2",
    "infernus2",
    "ruston",
    "specter2",
    "specter",
    "comet3",
    "elegy",
    "tampa2",
    "lynx",
    "tropos",
    "omnis",
    "seven70",
    "bestiagts",
    "mamba",
    "verlierer2",
    "schafter3",
    "schafter4",
    "feltzer3",
    "casco",
    "kuruma",
    "kuruma2",
    "stingertt",
    "gauntlet6",
    "coureur",
    };

    private List<string> models_submarine = new List<string>() {
    "avisa",
    };

    private List<string> models_supers = new List<string>() {
    "virtue",
    "entity3",
    "lm87",
    "torero2",
    "zeno",
    "ignus",
    "champion",
    "tigon",
    "furia",
    "zorrusso",
    "krieger",
    "emerus",
    "s80",
    "thrax",
    "deveste",
    "tyrant",
    "tezeract",
    "taipan",
    "entity2",
    "autarch",
    "sc1",
    "cyclone",
    "visione",
    "xa21",
    "vagner",
    "gp1",
    "italigtb",
    "italigtb2",
    "nero",
    "nero2",
    "tempesta",
    "penetrator",
    "tyrus",
    "le7b",
    "sheava",
    "pfister811",
    "prototipo",
    "reaper",
    "fmj",
    "sultanrs",
    "banshee2",
    "t20",
    "osiris",
    "turismo3",
    };

    private List<string> models_suvs = new List<string>() {
    "issi8",
    "granger2",
    "iwagen",
    "baller7",
    "astron",
    "jubilee",
    "seminole2",
    "landstalker2",
    "rebla",
    "novak",
    "toros",
    "stretch",
    "contender",
    "xls2",
    "baller3",
    "baller4",
    "baller5",
    "baller6",
    "vivanite",
    "aleutian",
    "cavalcade3",
    "baller8",
    "dorado",
    };

    private List<string> models_towtruck = new List<string>() {
    "towtruck4",
    };

    private List<string> models_tuners = new List<string>() {
    "kanjosj",
    "postlude",
    "previon",
    "cypher",
    "sultan3",
    "growler",
    "vectre",
    "dominator7",
    "comet6",
    "remus",
    "jester4",
    "tailgater2",
    "warrener2",
    "rt3000",
    "zr350",
    "dominator8",
    "euros",
    "futo2",
    "calico"
    };

    private List<string> models_valentine = new List<string>() {
    "btype3",
    };

    private List<string> models_vans = new List<string>() {
    "journey2",
    "surfer3",
    "youga3",
    "speedo4",
    "youga2",
    "rumpo3",
    "minivan2",
    "boxville6",
    };

    private List<string> models_wastelander = new List<string>() {
    "wastelander",
    };

    private List<string> models_weaponboats = new List<string>() {
    "dinghy5",
    "patrolboat",
    "tug",
    };

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
        street_flag = config.GetValue<int>("MAIN", "spawn_traffic", 1);
        street_blip = config.GetValue<int>("MAIN", "traffic_cars_blips", 0);
        mod_plate = config.GetValue<int>("MAIN", "new_license_plates", 0);

        char symbol = '#';
        string[] lines = File.ReadAllLines("Scripts\\mp_blacklist.txt");
        List<string> blacklist_str = new List<string>();

        foreach (string s in lines)
        {
            if (s.IndexOf(symbol) == -1)
                blacklist_str.Add(s);
        }

        foreach (string hash in blacklist_str)
        {
            if (models_arena.Contains(hash))
                models_arena.Remove(hash);

            if (models_beach.Contains(hash))
                models_beach.Remove(hash);

            if (models_boats.Contains(hash))
                models_boats.Remove(hash);

            if (models_cemetery.Contains(hash))
                models_cemetery.Remove(hash);

            if (models_cheburek.Contains(hash))
                models_cheburek.Remove(hash);

            if (models_cinema.Contains(hash))
                models_cinema.Remove(hash);

            if (models_cluckin.Contains(hash))
                models_cluckin.Remove(hash);

            if (models_compacts.Contains(hash))
                models_compacts.Remove(hash);

            if (models_coupes.Contains(hash))
                models_coupes.Remove(hash);

            if (models_cycles.Contains(hash))
                models_cycles.Remove(hash);

            if (models_ghetto.Contains(hash))
                models_ghetto.Remove(hash);

            if (models_helicopter.Contains(hash))
                models_helicopter.Remove(hash);

            if (models_humanlabs.Contains(hash))
                models_humanlabs.Remove(hash);

            if (models_industrial.Contains(hash))
                models_industrial.Remove(hash);

            if (models_karting.Contains(hash))
                models_karting.Remove(hash);

            if (models_military_bikes.Contains(hash))
                models_military_bikes.Remove(hash);

            if (models_military_helicopters.Contains(hash))
                models_military_helicopters.Remove(hash);

            if (models_military_opressors.Contains(hash))
                models_military_opressors.Remove(hash);

            if (models_military_planes.Contains(hash))
                models_military_planes.Remove(hash);

            if (models_motorcycles.Contains(hash))
                models_motorcycles.Remove(hash);

            if (models_muscle.Contains(hash))
                models_muscle.Remove(hash);

            if (models_offroad.Contains(hash))
                models_offroad.Remove(hash);

            if (models_openwheel.Contains(hash))
                models_openwheel.Remove(hash);

            if (models_planes.Contains(hash))
                models_planes.Remove(hash);

            if (models_police.Contains(hash))
                models_police.Remove(hash);

            if (models_sedans.Contains(hash))
                models_sedans.Remove(hash);

            if (models_slawmantruck.Contains(hash))
                models_slawmantruck.Remove(hash);

            if (models_sportclassic.Contains(hash))
                models_sportclassic.Remove(hash);

            if (models_submarine.Contains(hash))
                models_submarine.Remove(hash);

            if (models_supers.Contains(hash))
                models_supers.Remove(hash);

            if (models_suvs.Contains(hash))
                models_suvs.Remove(hash);

            if (models_towtruck.Contains(hash))
                models_towtruck.Remove(hash);

            if (models_tuners.Contains(hash))
                models_tuners.Remove(hash);

            if (models_valentine.Contains(hash))
                models_valentine.Remove(hash);

            if (models_vans.Contains(hash))
                models_vans.Remove(hash);

            if (models_wastelander.Contains(hash))
                models_wastelander.Remove(hash);

            if (models_weaponboats.Contains(hash))
                models_weaponboats.Remove(hash);
        }

        Tick += OnTick;
        Aborted += OnAborded;
    }

    public enum Nodetype
    {
        AnyRoad,
        Road,
        Offroad,
        Water
    }

    string GenerateVehicleModelName(int index_db, int type)
    {
        //0 - машина на парковку, 1 - машина для спавна в трафике
        string model_name = null;
        bool isEmpty;
        var random = new Random();
        switch (index_db)
        {
            case arena:
                isEmpty = !models_arena.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_arena[random.Next(models_arena.Count)];
                }
                break;

            case boats:
                isEmpty = !models_boats.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_boats[random.Next(models_boats.Count)];
                }
                break;

            case cemetery:
                isEmpty = !models_cemetery.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_cemetery[random.Next(models_cemetery.Count)];
                }
                break;

            case cheburek:
                isEmpty = !models_cheburek.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_cheburek[random.Next(models_cheburek.Count)];
                }
                break;

            case cinema:
                isEmpty = !models_cinema.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_cinema[random.Next(models_cinema.Count)];
                }
                break;

            case cluckin:
                isEmpty = !models_cluckin.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_cluckin[random.Next(models_cluckin.Count)];
                }
                break;

            case compacts_1:
            case compacts_2:
            case compacts_3:
            case compacts_4:
            case compacts_5:
            case compacts_6:
                isEmpty = !models_compacts.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_compacts[random.Next(models_compacts.Count)];
                }
                break;

            case coupes_1:
            case coupes_2:
            case coupes_3:
            case coupes_4:
            case coupes_5:
            case coupes_6:
            case coupes_7:
                isEmpty = !models_coupes.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_coupes[random.Next(models_coupes.Count)];
                }
                break;

            case cycles_1:
            case cycles_2:
            case cycles_3:
            case cycles_4:
            case cycles_5:
                isEmpty = !models_cycles.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_cycles[random.Next(models_cycles.Count)];
                }
                break;

            case ghetto_1:
            case ghetto_2:
            case ghetto_3:
            case ghetto_4:
            case ghetto_5:
                isEmpty = !models_ghetto.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_ghetto[random.Next(models_ghetto.Count)];
                }
                break;

            case helicopter:
                isEmpty = !models_helicopter.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_helicopter[random.Next(models_helicopter.Count)];
                }
                break;

            case humanlabs:
                isEmpty = !models_humanlabs.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_humanlabs[random.Next(models_humanlabs.Count)];
                }
                break;

            case industrial_1:
            case industrial_2:
            case industrial_3:
            case industrial_4:
                isEmpty = !models_industrial.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_industrial[random.Next(models_industrial.Count)];
                }
                break;

            case karting:
                isEmpty = !models_karting.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_karting[random.Next(models_karting.Count)];
                }
                break;

            case vetir:
                if (veh[index_db] == null && vetir_model != "Blocked")
                {
                    model_name = models_karting[random.Next(models_karting.Count)];
                }
                break;

            case scarab:
                if (veh[index_db] == null && scarab_model != "Blocked")
                {
                    model_name = scarab_model;
                }
                break;

            case terrorbyte:
                if (veh[index_db] == null && terrorbyte_model != "Blocked")
                {
                    model_name = terrorbyte_model;
                }
                break;

            case thruster:
                if (veh[index_db] == null && thruster_model != "Blocked")
                {
                    model_name = thruster_model;
                }
                break;

            case khanjari:
                if (veh[index_db] == null && khanjari_model != "Blocked")
                {
                    model_name = khanjari_model;
                }
                break;

            case chernobog:
                if (veh[index_db] == null && chernobog_model != "Blocked")
                {
                    model_name = chernobog_model;
                }
                break;

            case barrage:
                if (veh[index_db] == null && barrage_model != "Blocked")
                {
                    model_name = barrage_model;
                }
                break;

            case trailerLarge:
                if (veh[index_db] == null && trailerLarge_model != "Blocked")
                {
                    model_name = trailerLarge_model;
                }
                break;

            case halfTrack:
                if (veh[index_db] == null && halfTrack_model != "Blocked")
                {
                    model_name = halfTrack_model;
                }
                break;

            case apc:
                if (veh[index_db] == null && apc_model != "Blocked")
                {
                    model_name = apc_model;
                }
                break;

            case trailerSmall2:
                if (veh[index_db] == null && trailerSmall2_model != "Blocked")
                {
                    model_name = trailerSmall2_model;
                }
                break;

            case military_planes_1:
            case military_planes_2:
                isEmpty = !models_military_planes.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_military_planes[random.Next(models_military_planes.Count)];
                }
                break;

            case military_helicopters:
                isEmpty = !models_military_helicopters.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_military_helicopters[random.Next(models_military_helicopters.Count)];
                }
                break;

            case military_opressors:
                isEmpty = !models_military_opressors.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_military_opressors[random.Next(models_military_opressors.Count)];
                }
                break;

            case military_bikes:
                isEmpty = !models_military_bikes.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_military_bikes[random.Next(models_military_bikes.Count)];
                }
                break;

            case raiju:
                if (veh[index_db] == null && raiju_model != "Blocked")
                {
                    model_name = raiju_model;
                }
                break;

            case streamer216:
                if (veh[index_db] == null && streamer216_model != "Blocked")
                {
                    model_name = streamer216_model;
                }
                break;

            case conada2:
                if (veh[index_db] == null && conada2_model != "Blocked")
                {
                    model_name = conada2_model;
                }
                break;

            case motorcycles_1:
            case motorcycles_2:
            case motorcycles_3:
            case motorcycles_4:
            case motorcycles_5:
            case motorcycles_6:
                isEmpty = !models_motorcycles.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_motorcycles[random.Next(models_motorcycles.Count)];
                }
                break;

            case muscle_1:
            case muscle_2:
            case muscle_3:
            case muscle_4:
            case muscle_5:
            case muscle_6:
            case muscle_7:
                isEmpty = !models_muscle.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_muscle[random.Next(models_muscle.Count)];
                }
                break;

            case offroad_1:
            case offroad_2:
            case offroad_3:
            case offroad_4:
                isEmpty = !models_offroad.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_offroad[random.Next(models_offroad.Count)];
                }
                break;

            case openwheel:
                isEmpty = !models_openwheel.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_openwheel[random.Next(models_openwheel.Count)];
                }
                break;

            case beach:
                isEmpty = !models_beach.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_beach[random.Next(models_beach.Count)];
                }
                break;

            case planes:
                isEmpty = !models_planes.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_planes[random.Next(models_planes.Count)];
                }
                break;

            case police_1:
            case police_2:
            case police_3:
            case police_4:
            case police_5:
                isEmpty = !models_police.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_police[random.Next(models_police.Count)];
                }
                break;

            case sedans_1:
            case sedans_2:
            case sedans_3:
            case sedans_4:
            case sedans_5:
            case sedans_6:
            case sedans_7:
                isEmpty = !models_sedans.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_sedans[random.Next(models_sedans.Count)];
                }
                break;

            case slawmantruck:
                isEmpty = !models_slawmantruck.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_slawmantruck[random.Next(models_slawmantruck.Count)];
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
                isEmpty = !models_sportclassic.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_sportclassic[random.Next(models_sportclassic.Count)];
                }
                break;

            case submarine:
                isEmpty = !models_submarine.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_submarine[random.Next(models_submarine.Count)];
                }
                break;

            case supers_1:
            case supers_2:
            case supers_3:
            case supers_4:
            case supers_5:
            case supers_6:
            case supers_7:
                isEmpty = !models_supers.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_supers[random.Next(models_supers.Count)];
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
                isEmpty = !models_suvs.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_suvs[random.Next(models_suvs.Count)];
                }
                break;

            case towtruck_1:
            case towtruck_2:
            case towtruck_3:
            case towtruck_4:
            case towtruck_5:
                isEmpty = !models_towtruck.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_towtruck[random.Next(models_towtruck.Count)];
                }
                break;

            case tuners_1:
            case tuners_2:
            case tuners_3:
            case tuners_4:
            case tuners_5:
                isEmpty = !models_tuners.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_tuners[random.Next(models_tuners.Count)];
                }
                break;

            case valentine:
                isEmpty = !models_valentine.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_valentine[random.Next(models_valentine.Count)];
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
                isEmpty = !models_vans.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_vans[random.Next(models_vans.Count)];
                }
                break;

            case wastelander:
                isEmpty = !models_wastelander.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_wastelander[random.Next(models_wastelander.Count)];
                }
                break;

            case weaponboats:
                isEmpty = !models_weaponboats.Any();
                if ((veh[index_db] == null && !isEmpty) || type == 1)
                {
                    model_name = models_weaponboats[random.Next(models_weaponboats.Count)];
                }
                break;
        }
        return model_name;
    }

    void SetNumberPlate(Vehicle car, int mode, int index)
    {
        if (mode == 1)
        {
            Function.Call(Hash.SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX, car, index);
        }
    }

    public void SetSpawnLocation(Vehicle car, int min, int max)
    {
        car.PlaceOnGround(); 
        float AroundDistance = (float)GetRandomNumber(min, max);
        car.Position = World.GetNextPositionOnStreet(Game.Player.Character.Position.Around(AroundDistance)); 

        while (car.IsOnScreen)
        {
            Wait(10);
            AroundDistance += 5f;
            car.Position = World.GetNextPositionOnStreet(Game.Player.Character.Position.Around(AroundDistance));
        }

        OutputArgument outArgA = new OutputArgument();
        OutputArgument outArgB = new OutputArgument();

        if (Function.Call<bool>(Hash.GET_CLOSEST_VEHICLE_NODE_WITH_HEADING, car.Position.X, car.Position.Y, car.Position.Z, outArgA, outArgB, 1, 1077936128, 0))
        {
            car.Heading = outArgB.GetResult<float>();
        }

        car.IsVisible = true;
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
            return car;
        }

    }

    Blip CreateMarkerAboveCar(Vehicle car)
    {
        Blip mark = Function.Call<Blip>(GTA.Native.Hash.ADD_BLIP_FOR_ENTITY, car);
        Function.Call(GTA.Native.Hash.SET_BLIP_SPRITE, mark, 1);
        Function.Call(GTA.Native.Hash.SET_BLIP_COLOUR, mark, 3);
        Function.Call(GTA.Native.Hash.FLASH_MINIMAP_DISPLAY);
        mark.Name = "Unique vehicle";
        return mark;
    }

    private int GetRandomNumber(int min, int max)
    {
        var random = new Random();
        if (max <= 1)
            return 0;

        try
        {
            return random.Next(0, max);
        }
        catch
        {
            return 0;
        }
    }

    bool IsIndexCanSpawned(int type)
    {
        int[] veh_types = {
            arena,
            boats,
            cemetery,
            cinema,
            helicopter,
            humanlabs,
            vetir,
            scarab,
            terrorbyte,
            thruster,
            khanjari,
            chernobog,
            barrage,
            trailerLarge,
            halfTrack,
            apc,
            trailerSmall2,
            military_planes_1,
            military_planes_2,
            military_helicopters,
            military_opressors,
            military_bikes,
            raiju,
            streamer216,
            conada2,
            openwheel,
            beach,
            slawmantruck,
            submarine,
            valentine,
            wastelander,
            weaponboats,
        };
        bool notInArray = true;

        for (int i = 0; i < veh_types.Length && notInArray; i++)
        {
            if (type == veh_types[i])
            {
                notInArray = false;
            }
        }
        return notInArray;
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
        int index_db = 0;
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
                        if (blip_config == 1)
                        {
                            Blip mark = CreateMarkerAboveCar(veh[index_db]);
                            marker.Add(mark);
                        }
                    }
                }

                //spawn in traffic
                if ((Game.GameTime > cooldown + 10000 || cooldown == 0) && IsIndexCanSpawned(index_db))
                {
                    model_name = GenerateVehicleModelName(index_db, 1);
                    street_veh[index_db] = CreateNewVehicle(model_name, Vector3.Zero, 0.0f);
                    street_veh[index_db].IsVisible = false;
                    Ped street_driver = Function.Call<Ped>(Hash.CREATE_RANDOM_PED_AS_DRIVER, street_veh[index_db], true);
                    street_driver.AlwaysKeepTask = true;
                    SetSpawnLocation(street_veh[index_db], 50, 300);
                    street_veh[index_db].Speed = 10.0f;
                    street_driver.MarkAsNoLongerNeeded();
                    cooldown = Game.GameTime;
                }
            }
            index_db++;
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

        //Delete Vehicle
        index_db = 0;
        foreach (Vehicle car in street_veh)
        {
            if (car != null && car.Exists() && !Function.Call<bool>(Hash.IS_PED_IN_VEHICLE, Game.Player.Character, car, false) && Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, car.Position.X, car.Position.Y, car.Position.Z, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z, 0) > 400)
            {
                car.Delete();
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
            Game.Player.Character.Position = coords[debug_releport];
            if (coords.Count - 1 > debug_releport)
            {
                debug_releport++;
            }
            else
            {
                debug_releport = 0;
            }
        }
    }
}
