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

public class VehList
{
    public static List<string> models_arena = new List<string>() {
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

    public static List<string> models_boats = new List<string>() {
    "longfin",
    "toro"
    };

    public static List<string> models_cemetery = new List<string>() {
    "tornado6",
    "btype2",
    "sanctus",
    "lurcher",
    "brigham"
    };

    public static List<string> models_cheburek = new List<string>() {
    "cheburek",
    "ratbike",
    "slamvan3"
    };

    public static List<string> models_cinema = new List<string>() {
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

    public static List<string> models_cluckin = new List<string>() {
    "benson2"
    };

    public static List<string> models_compacts = new List<string>() {
    "brioso",
    "brioso2",
    "brioso3",
    "weevil",
    "club",
    "kanjo",
    "asbo",
    "issi3"
    };

    public static List<string> models_coupes = new List<string>() {
    "kanjosj",
    "postlude",
    "previon",
    "windsor2",
    "windsor",
    "fr36"
    };

    public static List<string> models_cycles = new List<string>() {
    "inductor",
    "inductor2",
    };

    public static List<string> models_ghetto = new List<string>() {
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

    public static List<string> models_helicopter = new List<string>() {
    "conada",
    "havok",
    "volatus",
    "supervolito",
    "supervolito2",
    "swift2",
    };

    public static List<string> models_humanlabs = new List<string>() {
    "brickade2",
    };

    public static List<string> models_industrial = new List<string>() {
    "pounder2",
    "mule4",
    "phantom3",
    "hauler2",
    "phantom2",
    "mule3",
    "boxville4",
    };

    public static List<string> models_karting = new List<string>() {
    "veto",
    "veto2",
    };

    public static string vetir_model = "vetir";
    public static string scarab_model = "scarab";
    public static string terrorbyte_model = "terbyte";
    public static string thruster_model = "thruster";
    public static string khanjari_model = "khanjali";
    public static string chernobog_model = "chernobog";
    public static string barrage_model = "barrage";
    public static string trailerLarge_model = "trailerlarge";
    public static string halfTrack_model = "halftrack";
    public static string apc_model = "apc";
    public static string trailerSmall2_model = "trailersmall2";
    public static string raiju_model = "raiju";
    public static string streamer216_model = "streamer216";
    public static string conada2_model = "conada2";

    public static List<string> models_military_planes = new List<string>() {
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

    public static List<string> models_military_helicopters = new List<string>() {
    "annihilator2",
    "akula",
    "hunter",
    "valkyrie",
    "savage",
    };

    public static List<string> models_military_opressors = new List<string>() {
    "oppressor",
    "oppressor2",
    };

    public static List<string> models_military_bikes = new List<string>() {
    "squaddie",
    "manchez2",
    "winky",
    "insurgent3",
    };

    public static List<string> models_motorcycles = new List<string>() {
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

    public static List<string> models_muscle = new List<string>() {
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

    public static List<string> models_offroad = new List<string>() {
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

    public static List<string> models_openwheel = new List<string>() {
    "openwheel1",
    "openwheel2",
    "formula",
    "formula2",
    "raptor",
    };

    public static List<string> models_beach = new List<string>() {
    "pbus2",
    };

    public static List<string> models_planes = new List<string>() {
    "microlight",
    "nimbus",
    "luxor2",
    "velum2",
    };

    public static List<string> models_police = new List<string>() {
    "riot2",
    "polgauntlet",
    "police5",
    };

    public static List<string> models_sedans = new List<string>() {
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

    public static List<string> models_slawmantruck = new List<string>() {
    "slamtruck",
    };

    public static List<string> models_sportclassic = new List<string>() {
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

    public static List<string> models_submarine = new List<string>() {
    "avisa",
    };

    public static List<string> models_supers = new List<string>() {
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

    public static List<string> models_suvs = new List<string>() {
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

    public static List<string> models_towtruck = new List<string>() {
    "towtruck4",
    };

    public static List<string> models_tuners = new List<string>() {
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

    public static List<string> models_valentine = new List<string>() {
    "btype3",
    };

    public static List<string> models_vans = new List<string>() {
    "journey2",
    "surfer3",
    "youga3",
    "speedo4",
    "youga2",
    "rumpo3",
    "minivan2",
    "boxville6",
    };

    public static List<string> models_wastelander = new List<string>() {
    "wastelander",
    };

    public static List<string> models_weaponboats = new List<string>() {
    "dinghy5",
    "patrolboat",
    "tug",
    };
}