using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GTA;
using GTA.Native;
using GTA.Math;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

[Serializable]
public class VehicleData
{
    public Vector3 Position;
    public float Heading;
    public int Model;
    public int PrimaryColor;
    public int SecondaryColor;
    public Dictionary<VehicleModType, int> Modifications;
    public int OwnerHash;
    public bool IsCurrentVehicle;
}

public class SaveVehicleScript : Script
{
    private const string PersonalVehiclePath = "scripts\\AllMpVehiclesinSp\\PersonalVehicle.bin";
    Dictionary<int, VehicleData> personalVehicles = new Dictionary<int, VehicleData>();
    private HashSet<int> dlcVehicleHashes;
    private VehicleData personalVehicle;
    Blip PlayerVeh;

    private bool vehiclesRestored = false;
    private List<Vehicle> persistentVehicles = new List<Vehicle>();
    Dictionary<int, Vehicle> persistentVehiclesNew = new Dictionary<int, Vehicle>();
    Dictionary<int, Vector3> personalPositions = new Dictionary<int, Vector3>();
    Dictionary<int, float> personalHeadings = new Dictionary<int, float>();
    int last_player = 0;
    bool saveCurrentVeh = false;

    public SaveVehicleScript()
    {
        //Tick += OnTick;
        //Aborted += OnAbort;
        //LoadDlcVehicles();
    }

    private void LoadDlcVehicles()
    {
        dlcVehicleHashes = new HashSet<int>();
        int dlcVehicleCount = Function.Call<int>(Hash.GET_NUM_DLC_VEHICLES);
        for (int i = 0; i < dlcVehicleCount; i++)
        {
            int modelHash = Function.Call<int>(Hash.GET_DLC_VEHICLE_MODEL, i);
            dlcVehicleHashes.Add(modelHash);
        }
    }

    private Dictionary<int, VehicleData> LoadPersonalVehicles()
    {
        if (File.Exists(PersonalVehiclePath))
        {
            using (FileStream stream = new FileStream(PersonalVehiclePath, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<int, VehicleData>)formatter.Deserialize(stream);
            }
        }
        return new Dictionary<int, VehicleData>();
    }

    private bool IsPersonalVehicle(Vehicle vehicle, int playerHash)
    {
        if (persistentVehiclesNew.ContainsKey(playerHash))
        {
            if (vehicle != null && vehicle.Exists() && persistentVehiclesNew[playerHash] == vehicle)
            {
                return true;
            }
        }

        int blipHandle = Function.Call<int>(Hash.GET_BLIP_FROM_ENTITY, vehicle);
        if (blipHandle == 0) return false; 

        int blipSprite = Function.Call<int>(Hash.GET_BLIP_SPRITE, blipHandle);
        return blipSprite == (int)BlipSprite.PersonalVehicleCar;
    }

    private Blip SetBlipColor(PedHash player, Vehicle veh, int test)
    {
        Blip bp = veh.AddBlip();
        bp.Sprite = BlipSprite.PersonalVehicleCar;
        if ((PedHash)player == PedHash.Michael) bp.Color = BlipColor.Michael;
        else if ((PedHash)player == PedHash.Franklin) bp.Color = BlipColor.Franklin;
        else if ((PedHash)player == PedHash.Trevor) bp.Color = BlipColor.Trevor;

        //GTA.UI.Screen.ShowSubtitle("With func " + test);
        return bp;
    }

    private void RemovePlayerBlip(Blip PlayerVeh)
    {
        if (PlayerVeh != null && PlayerVeh.Exists())
        {
            PlayerVeh.Delete();
            PlayerVeh = null;
        }
    }

    private void ReplaceTargetVehicles()
    {
        Dictionary<int, VehicleData> personalVehicles = LoadPersonalVehicles();
        if (personalVehicles.Count == 0) return;

        Dictionary<int, int> playerVehicleReplacements = new Dictionary<int, int>
        {
            { Function.Call<int>(Hash.GET_HASH_KEY, "player_zero"), Function.Call<int>(Hash.GET_HASH_KEY, "TAILGATER") },
            { Function.Call<int>(Hash.GET_HASH_KEY, "player_one"), Function.Call<int>(Hash.GET_HASH_KEY, "BUFFALO2") },
            { Function.Call<int>(Hash.GET_HASH_KEY, "player_two"), Function.Call<int>(Hash.GET_HASH_KEY, "BODHI2") }
        };

        List<(Ped ped, VehicleSeat seat)> occupants = new List<(Ped, VehicleSeat)>();

        Ped player = Game.Player.Character;
        Vehicle playerVehicle = player.CurrentVehicle;
        int playerHash = player.Model.Hash;
        int targetVehicleHash = playerVehicleReplacements[player.Model.Hash];

        if (!personalVehicles.ContainsKey(playerHash)) return;
        VehicleData personalVehicle = personalVehicles[playerHash];

        if (!persistentVehiclesNew.ContainsKey(playerHash)) persistentVehiclesNew.Add(playerHash, null);

        if (personalVehicle.IsCurrentVehicle && player.CurrentVehicle != null)
        {
            Vehicle newVehicle = player.CurrentVehicle;
            persistentVehiclesNew[playerHash] = newVehicle;


            if (PlayerVeh != null && PlayerVeh.Exists())
            {
                PlayerVeh.Delete();
                PlayerVeh = null;
            }
            last_player = playerHash;
            SavePersonalVehicle(player.CurrentVehicle, false);
            personalVehicle = personalVehicles[playerHash];
            return;
        }

        Vehicle[] allVehicles = World.GetAllVehicles();
        foreach (Vehicle veh in allVehicles)
        {
            if (IsPersonalVehicle(veh, player.Model.Hash) && veh.Model.Hash == targetVehicleHash)
            {
                Vehicle newVehicle;
                Vector3 position;
                float heading;

                if (personalPositions.ContainsKey(playerHash))
                {
                    position = personalPositions[playerHash];
                    heading = personalHeadings[playerHash];
                }
                else
                {
                    position = veh.Position;
                    heading = veh.Heading;
                }

                int seatCount = Function.Call<int>(Hash.GET_VEHICLE_MODEL_NUMBER_OF_SEATS, veh.Model);

                // Запоминаем всех пассажиров и водителя с их мест
                for (int i = -1; i < seatCount - 1; i++) // -1 это водитель
                {
                    Ped ped = veh.GetPedOnSeat((VehicleSeat)i);
                    if (ped != null && ped.Exists())
                    {
                        occupants.Add((ped, (VehicleSeat)i));
                    }
                }

                veh.Delete();

                if (!persistentVehiclesNew.ContainsKey(playerHash)) persistentVehiclesNew.Add(playerHash, null);

                if (persistentVehiclesNew[playerHash] != null && persistentVehiclesNew[playerHash].Exists()) continue;

                if (player.CurrentVehicle != null && player.CurrentVehicle.Model.Hash == personalVehicle.Model)
                {
                    newVehicle = player.CurrentVehicle;
                    persistentVehiclesNew[playerHash] = newVehicle;
                    RemovePlayerBlip(PlayerVeh);
                    PlayerVeh = SetBlipColor((PedHash)Game.Player.Character.Model.Hash, newVehicle, 1);
                }
                else
                {
                    newVehicle = World.CreateVehicle(new Model(personalVehicle.Model), position, heading);
                    while (newVehicle == null) Script.Wait(0);

                    foreach (var (ped, seat) in occupants)
                    {
                        if (ped != null && ped.Exists())
                        {
                            ped.Task.WarpIntoVehicle(newVehicle, seat);
                        }
                    }

                    newVehicle.Mods.PrimaryColor = (VehicleColor)personalVehicle.PrimaryColor;
                    newVehicle.Mods.SecondaryColor = (VehicleColor)personalVehicle.SecondaryColor;
                    ApplyVehicleModifications(newVehicle, personalVehicle.Modifications);
                    newVehicle.IsPersistent = true;
                    persistentVehicles.Add(newVehicle);
                    persistentVehiclesNew[playerHash] = newVehicle;
                    RemovePlayerBlip(PlayerVeh);
                    PlayerVeh = SetBlipColor((PedHash)Game.Player.Character.Model.Hash, newVehicle, 1);
                }


            }
        }
    }

    private void ReplaceTargetVehiclesOld()
    {
        Dictionary<int, VehicleData> personalVehicles = LoadPersonalVehicles();
        if (personalVehicles.Count == 0) return;

        Dictionary<int, int> playerVehicleReplacements = new Dictionary<int, int>
    {
        { Function.Call<int>(Hash.GET_HASH_KEY, "player_zero"), Function.Call<int>(Hash.GET_HASH_KEY, "TAILGATER") },
        { Function.Call<int>(Hash.GET_HASH_KEY, "player_one"), Function.Call<int>(Hash.GET_HASH_KEY, "BUFFALO2") },
        { Function.Call<int>(Hash.GET_HASH_KEY, "player_two"), Function.Call<int>(Hash.GET_HASH_KEY, "BODHI2") }
    };

        Ped player = Game.Player.Character;
        Vehicle playerVehicle = player.CurrentVehicle;
        int playerHash = player.Model.Hash;
        int targetVehicleHash = playerVehicleReplacements[player.Model.Hash];
        bool IsPlayerInTargetVehicle = false;

        if (!personalVehicles.ContainsKey(playerHash)) return;

        VehicleData personalVehicle = personalVehicles[playerHash];

        if (!persistentVehiclesNew.ContainsKey(playerHash)) persistentVehiclesNew.Add(playerHash, null);

        if (personalVehicle.IsCurrentVehicle && player.CurrentVehicle != null)
        {
            Vehicle newVehicle = player.CurrentVehicle;
            persistentVehiclesNew[playerHash] = newVehicle;


            if (PlayerVeh != null && PlayerVeh.Exists())
            {
                PlayerVeh.Delete();
                PlayerVeh = null;
            }
            last_player = playerHash;
            SavePersonalVehicle(player.CurrentVehicle, false);
            personalVehicle = personalVehicles[playerHash];
            return;
        }

        Vehicle[] allVehicles = World.GetAllVehicles();

        foreach (Vehicle veh in allVehicles)
        {
            if (IsPersonalVehicle(veh, player.Model.Hash) && player.Model.Hash == playerHash && veh.Model.Hash == targetVehicleHash)
            {
                Vehicle newVehicle;
                Vector3 position;
                float heading;

                if (personalPositions.ContainsKey(playerHash))
                {
                    position = personalPositions[playerHash];
                    heading = personalHeadings[playerHash];
                }
                else
                {
                    position = veh.Position;
                    heading = veh.Heading;
                }
                //
                if (veh.Model.Hash == targetVehicleHash) veh.Delete();

                if (persistentVehiclesNew.ContainsKey(playerHash))
                {
                    if (persistentVehiclesNew[playerHash] != null && persistentVehiclesNew[playerHash].Exists()) continue;
                    else
                    {
                        if (player.CurrentVehicle != null && player.CurrentVehicle.Model.Hash == personalVehicle.Model)
                        {
                            newVehicle = player.CurrentVehicle;
                            persistentVehiclesNew[playerHash] = newVehicle;
                            GTA.UI.Screen.ShowSubtitle("Your personal vehicle has been restored. 0");
                            if (PlayerVeh != null && PlayerVeh.Exists())
                            {
                                PlayerVeh.Delete();
                                PlayerVeh = null;
                                last_player = playerHash;
                            }
                        }
                        else
                        {
                            newVehicle = World.CreateVehicle(new Model(personalVehicle.Model), position, heading);
                            while (newVehicle == null) Script.Wait(0);
                            newVehicle.Mods.PrimaryColor = (VehicleColor)personalVehicle.PrimaryColor;
                            newVehicle.Mods.SecondaryColor = (VehicleColor)personalVehicle.SecondaryColor;
                            ApplyVehicleModifications(newVehicle, personalVehicle.Modifications);
                            newVehicle.IsPersistent = true;
                            persistentVehicles.Add(newVehicle);
                            persistentVehiclesNew[playerHash] = newVehicle;
                            if (PlayerVeh != null && PlayerVeh.Exists())
                            {
                                PlayerVeh.Delete();
                                PlayerVeh = null;
                            }
                            PlayerVeh = SetBlipColor((PedHash)Game.Player.Character.Model.Hash, newVehicle, 1);
                            last_player = playerHash;
                            GTA.UI.Screen.ShowSubtitle("Your personal vehicle has been restored. 1");
                        }
                    }
                }
                else
                {
                    newVehicle = World.CreateVehicle(new Model(personalVehicle.Model), position, heading);
                    while (newVehicle == null) Script.Wait(0);
                    newVehicle.Mods.PrimaryColor = (VehicleColor)personalVehicle.PrimaryColor;
                    newVehicle.Mods.SecondaryColor = (VehicleColor)personalVehicle.SecondaryColor;
                    ApplyVehicleModifications(newVehicle, personalVehicle.Modifications);
                    newVehicle.IsPersistent = true;
                    persistentVehicles.Add(newVehicle);
                    persistentVehiclesNew[playerHash] = newVehicle;

                    if (PlayerVeh != null && PlayerVeh.Exists())
                    {
                        PlayerVeh.Delete();
                        PlayerVeh = null;
                    }
                    PlayerVeh = SetBlipColor((PedHash)Game.Player.Character.Model.Hash, newVehicle, 2);
                    last_player = playerHash;
                }
            }
        }
    }

    private Vehicle ReplaceVehicleAndReassignPassengers(Vehicle oldVehicle, VehicleHash newModel)
    {
        if (oldVehicle == null || !oldVehicle.Exists()) return null;

        Vector3 spawnPos = oldVehicle.Position;
        Vehicle newVehicle = null;
        List<(Ped ped, VehicleSeat seat)> occupants = new List<(Ped, VehicleSeat)>();

        int seatCount = Function.Call<int>(Hash.GET_VEHICLE_MODEL_NUMBER_OF_SEATS, oldVehicle.Model);

        // Запоминаем всех пассажиров и водителя с их мест
        for (int i = -1; i < seatCount - 1; i++) // -1 это водитель
        {
            Ped ped = oldVehicle.GetPedOnSeat((VehicleSeat)i);
            if (ped != null && ped.Exists())
            {
                occupants.Add((ped, (VehicleSeat)i));
            }
        }

        oldVehicle.Delete();
        newVehicle = World.CreateVehicle(newModel, spawnPos);

        // Рассаживаем всех обратно
        foreach (var (ped, seat) in occupants)
        {
            if (ped != null && ped.Exists())
            {
                ped.Task.WarpIntoVehicle(newVehicle, seat);
            }
        }

        return newVehicle;
    }

    private void SavePersonalVehicle(Vehicle vehicle, bool currentVeh)
    {
        Dictionary<int, VehicleData> personalVehicles = LoadPersonalVehicles();

        VehicleData data = new VehicleData
        {
            Position = vehicle.Position,
            Heading = vehicle.Heading,
            Model = vehicle.Model.Hash,
            PrimaryColor = (int)vehicle.Mods.PrimaryColor,
            SecondaryColor = (int)vehicle.Mods.SecondaryColor,
            Modifications = GetVehicleModifications(vehicle),
            OwnerHash = Game.Player.Character.Model.Hash,
            IsCurrentVehicle = currentVeh
        };

        personalVehicles[data.OwnerHash] = data;

        Directory.CreateDirectory(Path.GetDirectoryName(PersonalVehiclePath));
        using (FileStream stream = new FileStream(PersonalVehiclePath, FileMode.Create))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, personalVehicles);
        }
    }

    private void OnAbort(object sender, EventArgs e)
    {
        Ped player = Game.Player.Character;

        foreach (var entry in persistentVehiclesNew)
        {
            int playerHash = entry.Key;
            Vehicle veh = entry.Value;

            if (persistentVehiclesNew.ContainsKey(playerHash))
            {
                if (player.CurrentVehicle != null && player.CurrentVehicle == veh)
                {
                    SavePersonalVehicle(player.CurrentVehicle, true);
                }
                else
                {
                    //GTA.UI.Screen.ShowSubtitle(playerHash + " vs " + player.Model.Hash);
                    if (player.Model.Hash == playerHash)
                    {
                        Vector3 position = veh.Position;
                        float heading = veh.Heading;
                        veh.Delete();
                        Vehicle newVehicle = null;
                        Blip bp;

                        switch ((PedHash)player.Model.Hash)
                        {
                            case PedHash.Michael:
                                newVehicle = World.CreateVehicle(new Model("TAILGATER"), position, heading);
                                while (newVehicle == null) Script.Wait(0);
                                bp = newVehicle.AddBlip();
                                bp.Sprite = BlipSprite.PersonalVehicleCar;
                                break;

                            case PedHash.Franklin:
                                newVehicle = World.CreateVehicle(new Model("BUFFALO2"), position, heading);
                                while (newVehicle == null) Script.Wait(0);
                                bp = newVehicle.AddBlip();
                                bp.Sprite = BlipSprite.PersonalVehicleCar;
                                break;
                            case PedHash.Trevor:
                                newVehicle = World.CreateVehicle(new Model("BODHI2"), position, heading);
                                while (newVehicle == null) Script.Wait(0);
                                bp = newVehicle.AddBlip();
                                bp.Sprite = BlipSprite.PersonalVehicleCar;
                                break;
                        }
                    }
                }
            }
        }
    }

    private void OnTick(object sender, EventArgs e)
    {
        ReplaceTargetVehicles();
        CheckPersonalVehiclesBlips();

        if (Game.Player.Character.IsInVehicle())
        {
            Vehicle playerVehicle = Game.Player.Character.CurrentVehicle;

            if (playerVehicle != null)
            {
                if (!persistentVehiclesNew.ContainsKey(Game.Player.Character.Model.Hash)) persistentVehiclesNew.Add(Game.Player.Character.Model.Hash, null);

                if (playerVehicle != persistentVehiclesNew[Game.Player.Character.Model.Hash])
                {
                    if (Function.Call<bool>(Hash.IS_VEHICLE_IN_GARAGE_AREA, "Michael - Beverly Hills", playerVehicle) ||
                        Function.Call<bool>(Hash.IS_VEHICLE_IN_GARAGE_AREA, "Trevor - Countryside", playerVehicle) ||
                        Function.Call<bool>(Hash.IS_VEHICLE_IN_GARAGE_AREA, "Trevor - City", playerVehicle) ||
                        Function.Call<bool>(Hash.IS_VEHICLE_IN_GARAGE_AREA, "Trevor - Stripclub", playerVehicle) ||
                        Function.Call<bool>(Hash.IS_VEHICLE_IN_GARAGE_AREA, "Franklin - Aunt", playerVehicle) ||
                        Function.Call<bool>(Hash.IS_VEHICLE_IN_GARAGE_AREA, "Franklin - Hills", playerVehicle))
                    {
                        GTA.UI.Screen.ShowHelpTextThisFrame("Press ~INPUT_CONTEXT~ to make the vehicle personal.");

                        if (Game.IsControlPressed(GTA.Control.Context))
                        {
                            
                            SavePersonalVehicle(playerVehicle, true);
                            if (persistentVehiclesNew[Game.Player.Character.Model.Hash] != null && persistentVehiclesNew[Game.Player.Character.Model.Hash].Exists()) persistentVehiclesNew[Game.Player.Character.Model.Hash].Delete();
                            persistentVehiclesNew[Game.Player.Character.Model.Hash] = playerVehicle;
                            RemovePlayerBlip(PlayerVeh);
                            PlayerVeh = SetBlipColor((PedHash)Game.Player.Character.Model.Hash, playerVehicle, 1);
                            GTA.UI.Screen.ShowHelpText("This vehicle has become personal. It's shown on the Radar with ~BLIP_GANG_VEHICLE~.");
                        }
                    }
                }
            }
        }   
    }

    private void CheckPersonalVehiclesBlips()
    {
        foreach (var entry in persistentVehiclesNew)
        {
            int playerHash = entry.Key;
            Vehicle veh = entry.Value;

            if (veh != null && veh.Exists())
            {
                if (veh.Driver != null && veh.Driver == Game.Player.Character)
                {
                    Blip temp = Function.Call<Blip>(Hash.GET_BLIP_FROM_ENTITY, veh);
                    if (temp != null && temp.Exists())
                    {
                        temp.Delete();
                        PlayerVeh = null;
                        saveCurrentVeh = false;
                    }
                }
                else
                {
                    if (veh.Driver == null && PlayerVeh == null && Game.Player.Character.CurrentVehicle == null)
                    {
                        PlayerVeh = SetBlipColor((PedHash)Game.Player.Character.Model.Hash, veh, 3);
                        
                        if (!saveCurrentVeh)
                        {
                            SavePersonalVehicle(veh, false);
                            saveCurrentVeh = true;
                        }

                        if (personalPositions.ContainsKey(playerHash))
                        {
                            personalPositions[playerHash] = veh.Position;
                            personalHeadings[playerHash] = veh.Heading;
                        }
                        else
                        {
                            personalPositions.Add(playerHash, veh.Position);
                            personalHeadings.Add(playerHash, veh.Heading);
                        }
                    }
                }
            }
        }
    }

    private Dictionary<VehicleModType, int> GetVehicleModifications(Vehicle vehicle)
    {
        Dictionary<VehicleModType, int> mods = new Dictionary<VehicleModType, int>();
        foreach (VehicleModType modType in Enum.GetValues(typeof(VehicleModType)))
        {
            int modValue = vehicle.Mods[modType].Index;
            if (modValue != -1)
            {
                mods[modType] = modValue;
            }
        }
        return mods;
    }

    private void ApplyVehicleModifications(Vehicle vehicle, Dictionary<VehicleModType, int> modifications)
    {
        Function.Call(Hash.SET_VEHICLE_MOD_KIT, vehicle, 0);

        foreach (var mod in modifications)
        {
            vehicle.Mods[mod.Key].Index = mod.Value;
        }
    }
}