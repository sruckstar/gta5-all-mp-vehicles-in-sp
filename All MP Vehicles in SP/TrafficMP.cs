using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Linq; 
using GTA;
using GTA.Math;
using GTA.Native;

public class TrafficMP : Script
{
    private Vehicle _spawnedVehicle;
    private Ped _driver;
    private Blip _trafficBlip;

    private int _nextSpawnTime = 0;
    private int _nextSearchTime = 0;

    private float SpawnDistance = 300.0f;
    private float DespawnDistance = 500.0f;
    private int RespawnDelayMs = 3000;
    private const int SearchIntervalMs = 500;

    private int _blipColor;
    private int _trafficBlipConfig;
    private int _streetFlag;

    public static int disableTaxiFlag;

    private int _spawnSequenceIndex = 0;
    private Random _rnd = new Random();

    private HashSet<int> _dlcModelHashes = new HashSet<int>();
    private int _lastPlayerVehicleHandle = 0;

    // Список кодов богатых районов
    private HashSet<string> _richZones = new HashSet<string>
    {
        "RICHM",  // Richman
        "RGLEN",  // Richman Glen
        "ROCKF",  // Rockford Hills
        "VINE",   // Vinewood
        "DTVINE", // Downtown Vinewood
        "WVINE",  // West Vinewood
        "CHIL",   // Vinewood Hills
        "PBLUFF", // Pacific Bluffs
        "GOLF",   // GWC and Golfing Society
        "MORN",   // Morningwood
        "OBSERV"  // Galileo Observatory
    };

    public TrafficMP()
    {
        Tick += OnTick;
        Aborted += OnAborted;

        ScriptSettings config = ScriptSettings.Load("Scripts\\AllMpVehiclesInSp.ini");
        _blipColor = config.GetValue<int>("MAIN", "blip_color_traffic", 3);
        _trafficBlipConfig = config.GetValue<int>("MAIN", "traffic_cars_blips", 0);
        _streetFlag = config.GetValue<int>("MAIN", "spawn_traffic", 1);
        RespawnDelayMs = config.GetValue<int>("MAIN", "time_traffic_gen", 3000);

        SpawnDistance = config.GetValue<float>("ADVANCED", "SpawnDistance", 300.0f);
        DespawnDistance = config.GetValue<float>("ADVANCED", "DespawnDistance", 500.0f);

        BuildDlcCache();
    }

    private void BuildDlcCache()
    {
        try
        {
            FieldInfo[] fields = typeof(VehList).GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var field in fields)
            {
                if (field.FieldType == typeof(List<string>))
                {
                    List<string> list = (List<string>)field.GetValue(null);
                    if (list != null)
                        foreach (string modelName in list) _dlcModelHashes.Add(Game.GenerateHash(modelName));
                }
                else if (field.FieldType == typeof(string))
                {
                    string modelName = (string)field.GetValue(null);
                    if (!string.IsNullOrEmpty(modelName)) _dlcModelHashes.Add(Game.GenerateHash(modelName));
                }
            }
        }
        catch (Exception ex)
        {
            GTA.UI.Notification.Show($"~r~TrafficMP Cache Error:~w~ {ex.Message}");
        }
    }

    private void OnTick(object sender, EventArgs e)
    {
        if (_streetFlag == 0) return;

        Ped player = Game.Player.Character;

        // Логика DLC авто
        if (player.IsInVehicle())
        {
            Vehicle playerVeh = player.CurrentVehicle;

            if (playerVeh.Handle != _lastPlayerVehicleHandle)
            {
                if (IsDlcVehicle(playerVeh))
                {
                    if (_spawnedVehicle != null || (_trafficBlip != null && _trafficBlip.Exists()))
                    {
                        ReleaseCurrentGhost();
                        _nextSpawnTime = Game.GameTime + RespawnDelayMs;
                    }
                    else
                    {
                        _nextSpawnTime = Game.GameTime + RespawnDelayMs;
                    }
                }
                _lastPlayerVehicleHandle = playerVeh.Handle;
            }
        }
        else
        {
            _lastPlayerVehicleHandle = 0;
        }

        // Логика удаления по дистанции
        if (_spawnedVehicle != null && _spawnedVehicle.Exists())
        {
            if (player.Position.DistanceTo(_spawnedVehicle.Position) > DespawnDistance)
            {
                CleanUp();
                _nextSpawnTime = Game.GameTime + RespawnDelayMs;
            }
        }
        // Логика спавна
        else if (Game.GameTime >= _nextSpawnTime)
        {
            if (Game.GameTime >= _nextSearchTime)
            {
                AttemptSpawnOptimized();
                _nextSearchTime = Game.GameTime + SearchIntervalMs;
            }
        }
    }

    private bool IsDlcVehicle(Vehicle v)
    {
        return _dlcModelHashes.Contains(v.Model.Hash);
    }

    private void ReleaseCurrentGhost()
    {
        if (_trafficBlip != null && _trafficBlip.Exists()) _trafficBlip.Delete();
        _trafficBlip = null;

        if (_driver != null && _driver.Exists())
        {
            _driver.MarkAsNoLongerNeeded();
            _driver = null;
        }

        if (_spawnedVehicle != null && _spawnedVehicle.Exists())
        {
            _spawnedVehicle.MarkAsNoLongerNeeded();
            _spawnedVehicle = null;
        }

        _spawnedVehicle = null;
        _driver = null;
    }

    private void AttemptSpawnOptimized()
    {
        Ped playerPed = Game.Player.Character;
        Vector3 playerPos = playerPed.Position;
        Vector3 playerForward = playerPed.ForwardVector;

        Vector3 targetSearchPos = playerPos + (playerForward * SpawnDistance);

        OutputArgument outPos = new OutputArgument();
        OutputArgument outHead = new OutputArgument();

        Function.Call(Hash.GET_CLOSEST_VEHICLE_NODE_WITH_HEADING,
            targetSearchPos.X, targetSearchPos.Y, targetSearchPos.Z,
            outPos, outHead, 1, 3.0f, 0);

        Vector3 spawnNodePos = outPos.GetResult<Vector3>();
        float spawnNodeHeading = outHead.GetResult<float>();

        if (spawnNodePos != Vector3.Zero)
        {
            if (playerPos.DistanceTo(spawnNodePos) > 60.0f)
            {
                try
                {
                    // Передаем позицию спавна для проверки зоны
                    string modelToSpawn = GetNextModelName(spawnNodePos);
                    SpawnGhostCar(spawnNodePos, spawnNodeHeading, modelToSpawn);
                }
                catch (Exception ex)
                {
                    GTA.UI.Notification.Show($"TrafficMP Spawn Error: {ex.Message}");
                }
            }
        }
    }

    // Обновлен: принимает позицию для проверки зоны
    private string GetNextModelName(Vector3 spawnPos)
    {
        if (_spawnSequenceIndex == 0)
        {
            if (VehList.models_latest.Count > 0)
                return VehList.models_latest[_rnd.Next(VehList.models_latest.Count)];
            else
                return "vivanite2";
        }
        else if (_spawnSequenceIndex == 1)
        {
            if (disableTaxiFlag == 1)
                return GetModelFromContext(spawnPos);
            else
                return "vivanite2";
        }
        else
        {
            return GetModelFromContext(spawnPos);
        }
    }

    private void AdvanceSequence()
    {
        if (_spawnSequenceIndex == 0) _spawnSequenceIndex = 1;
        else if (_spawnSequenceIndex == 1) _spawnSequenceIndex = 2;
        else if (_spawnSequenceIndex == 2) _spawnSequenceIndex = 3;
        else if (_spawnSequenceIndex == 3) _spawnSequenceIndex = 4;
        else if (_spawnSequenceIndex == 4) _spawnSequenceIndex = 5;
        else if (_spawnSequenceIndex == 5) _spawnSequenceIndex = 6;
        else if (_spawnSequenceIndex == 6) _spawnSequenceIndex = 7;
        else if (_spawnSequenceIndex == 7) _spawnSequenceIndex = 8;
        else if (_spawnSequenceIndex == 8) _spawnSequenceIndex = 9;
        else if (_spawnSequenceIndex == 9) _spawnSequenceIndex = 1;
    }

    private string GetModelFromContext(Vector3 position)
    {
        // 1. Определяем зону в точке спавна
        string zoneName = Function.Call<string>(Hash.GET_NAME_OF_ZONE, position.X, position.Y, position.Z);
        bool isRichZone = _richZones.Contains(zoneName);

        int vclass = 0;

        if (isRichZone)
        {
            // БОГАТАЯ ЗОНА: Только Super (7) или SportClassic (5, 6)
            int[] richClasses = { 5, 6, 7 };
            vclass = richClasses[_rnd.Next(richClasses.Length)];
        }
        else
        {
            // ОБЫЧНАЯ ЗОНА: Исключаем 5, 6, 7

            // Пытаемся найти машину рядом с игроком
            Vehicle[] nearbyVehs = World.GetNearbyVehicles(Game.Player.Character.Position, 100.0f);
            bool foundValid = false;

            if (nearbyVehs.Length > 0)
            {
                Vehicle randomNeighbor = nearbyVehs[_rnd.Next(nearbyVehs.Length)];
                if (randomNeighbor.Exists())
                {
                    int c = (int)randomNeighbor.ClassType;
                    // Если класс машины допустим для бедной зоны (не 5,6,7 и валиден)
                    if (c != 5 && c != 6 && c != 7 && c <= 13)
                    {
                        vclass = c;
                        foundValid = true;
                    }
                }
            }

            // Если рядом ничего нет или попалась богатая машина, генерируем случайный "бедный" класс
            if (!foundValid)
            {
                // Допустимые классы для обычных зон
                int[] poorClasses = { 0, 1, 2, 3, 4, 8, 9, 10, 11, 12, 13 };
                vclass = poorClasses[_rnd.Next(poorClasses.Length)];
            }
        }

        string model_name = "vivanite2";

        switch (vclass)
        {
            case 0: model_name = GetRandomModel(VehList.models_compacts); break;
            case 1: model_name = GetRandomModel(VehList.models_sedans); break;
            case 2: model_name = GetRandomModel(VehList.models_suvs); break;
            case 3: model_name = GetRandomModel(VehList.models_coupes); break;
            case 4: model_name = GetRandomModel(VehList.models_muscle); break;
            case 5: model_name = GetRandomModel(VehList.models_sportclassic); break;
            case 6: model_name = GetRandomModel(VehList.models_sportclassic); break;
            case 7: model_name = GetRandomModel(VehList.models_supers); break;
            case 8: model_name = GetRandomModel(VehList.models_motorcycles); break;
            case 9: model_name = GetRandomModel(VehList.models_offroad); break;
            case 10: model_name = GetRandomModel(VehList.models_industrial); break;
            case 11: model_name = GetRandomModel(VehList.models_industrial); break;
            case 12: model_name = GetRandomModel(VehList.models_vans); break;
            case 13: model_name = GetRandomModel(VehList.models_cycles); break;
        }

        return model_name;
    }

    private string GetRandomModel(System.Collections.Generic.List<string> list)
    {
        if (list == null || list.Count == 0) return "vivanite2";
        return list[_rnd.Next(list.Count)];
    }

    private void SpawnGhostCar(Vector3 position, float heading, string modelName)
    {
        Model carModel = new Model(modelName);

        if (!carModel.IsValid || !carModel.IsInCdImage)
        {
            if (modelName != "vivanite2") SpawnGhostCar(position, heading, "vivanite2");
            return;
        }

        if (!carModel.IsLoaded) carModel.Request();
        if (!carModel.IsLoaded) return;

        Function.Call(Hash.CLEAR_AREA_OF_VEHICLES, position.X, position.Y, position.Z, 6.0f, false, false, false, false, false);

        _spawnedVehicle = World.CreateVehicle(carModel, position, heading);

        if (_spawnedVehicle != null)
        {
            if (_trafficBlipConfig == 1) CreateMarkerAboveCar(_spawnedVehicle);

            _spawnedVehicle.IsPersistent = true;
            _spawnedVehicle.IsEngineRunning = true;
            _spawnedVehicle.AreLightsOn = true;

            if (modelName.ToLower() == "vivanite2")
            {
                _spawnedVehicle.Mods.CustomPrimaryColor = Color.White;
                _spawnedVehicle.Mods.CustomSecondaryColor = Color.White;
                Function.Call(Hash.SET_VEHICLE_MOD_KIT, _spawnedVehicle, 0);
                Function.Call(Hash.SET_VEHICLE_MOD, _spawnedVehicle, 48, 0, false);
                _spawnedVehicle.LockStatus = VehicleLockStatus.CannotEnter;

                _driver = _spawnedVehicle.CreateRandomPedOnSeat(VehicleSeat.Driver);

                if (_driver != null)
                {
                    _driver.IsVisible = false;
                    _driver.CanBeTargetted = false;
                    _driver.BlockPermanentEvents = true;
                    _driver.IsInvincible = true;
                    _driver.CanRagdoll = false;
                    _driver.CanBeDraggedOutOfVehicle = false;
                }
            }
            else
            {
                _driver = _spawnedVehicle.CreateRandomPedOnSeat(VehicleSeat.Driver);
                if (_driver != null)
                {
                    _driver.IsVisible = true;
                    _driver.CanBeTargetted = true;
                    _driver.BlockPermanentEvents = false;
                }
            }

            if (_driver != null && _driver.Exists())
            {
                _driver.Task.CruiseWithVehicle(_spawnedVehicle, 10.0f, DrivingStyle.Normal);
            }

            AdvanceSequence();
        }

        carModel.MarkAsNoLongerNeeded();
    }

    private void CleanUp()
    {
        if (_driver != null && _driver.Exists()) _driver.Delete();
        if (_spawnedVehicle != null && _spawnedVehicle.Exists()) _spawnedVehicle.Delete();
        if (_trafficBlip != null && _trafficBlip.Exists()) _trafficBlip.Delete();

        _spawnedVehicle = null;
        _driver = null;
        _trafficBlip = null;
    }

    private void CreateMarkerAboveCar(Vehicle car)
    {
        if (_trafficBlip != null && _trafficBlip.Exists()) _trafficBlip.Delete();

        _trafficBlip = Function.Call<Blip>(Hash.ADD_BLIP_FOR_ENTITY, car);
        Function.Call(Hash.SET_BLIP_SPRITE, _trafficBlip, 1);
        Function.Call(Hash.SET_BLIP_COLOUR, _trafficBlip, _blipColor);
        Function.Call(Hash.FLASH_MINIMAP_DISPLAY);
        _trafficBlip.Name = "Unique vehicle";
    }

    private void OnAborted(object sender, EventArgs e)
    {
        CleanUp();
    }
}