using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager
{
    private List<string> _autoSaves = new();
    private int currIndex = 0; 

    [Serializable]
    private class TimeData
    {
        public bool _isPaused;
        public int _inGameHour;
        public int _inGameMinute;
        public int _inGameDay;
    }

    [Serializable]
    private class VirusData
    {
        public float _coughRate;
        public float _staminaDecayRate;
        public float _healthDecayRate;
        public float _mutationChance;
    }

    [Serializable]
    private class PosData
    {
        public float _posX;
        public float _posY;
        public float _posZ;
        public PosData(float x, float y, float z)
        {
            _posX = x;
            _posY = y;
            _posZ = z;
        }
    }

    [Serializable]
    private class NpcData
    {
        public string _name;
        public bool _isInfected;
        public float _health;
        public VirusData _virus;
        public float _stamina;
        public float _staminaDecayBase;
        public float _happiness;
        public float _happinessDecayRate;
        public float _happinessDecayBase;
        public bool _isHappinessDecayActive;
        public bool _isCommuting;
        public float _rotX;
        public float _rotY;
        public float _rotZ;
        public float _rotW;
        public PosData _position;
        public PosData _destination;
        public PosData _home;
    }

    [Serializable]
    private class GameData
    {
        public bool _godMode;
        public int _maxNPCs;
        public int _npcCount;
        public float _politicalPower;
        public float _politicalPowerMultiplier;
        public int _healthThreshold;
        public int _staminaThreshold;
        public TimeData _timeData;
        public List<NpcData> _npcDataList = new();
    }

    public void SaveGame(GameManager gm, string fileName)
    {
        BinaryFormatter formatter = new();
        if (!Directory.Exists(Application.persistentDataPath + "/saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/saves");
        }
        string filePath = Application.persistentDataPath + $"/saves/{fileName}.dat";
        FileStream stream = new(filePath, FileMode.Create);

        GameData data = new();
        #region GameManager
        data._godMode = gm.GodMode;
        data._maxNPCs = gm.MaxNPCs;
        data._politicalPower = gm.PoliticalPower;
        data._politicalPowerMultiplier = gm.PoliticalPowerMultiplier;
        data._healthThreshold = gm.HealthThreshold;
        data._staminaThreshold = gm.StaminaThreshold;
        #endregion GameManager

        #region TimeManager
        data._timeData = new();
        data._timeData._isPaused = gm.TimeManager.IsPaused;
        data._timeData._inGameHour = gm.TimeManager.InGameHour;
        data._timeData._inGameMinute = gm.TimeManager.InGameMinute;
        data._timeData._inGameDay = gm.TimeManager.InGameDay;
        #endregion TimeManager

        #region NpcRegion
        foreach (GameObject obj in gm.NPCs.ToList())
        {
            NPC npc = obj.GetComponent<NPC>();
            Navigation nav = obj.GetComponent<Navigation>();
            NpcData npcData = new()
            {
                _isInfected = npc.IsInfected,
                _health = npc.Health
            };
            if (npc.IsInfected && npc.Virus)
            {
                npcData._virus = new()
                {
                    _coughRate = npc.Virus.CoughRate,
                    _staminaDecayRate = npc.Virus.StaminaDecayRate,
                    _healthDecayRate = npc.Virus.HealthDecayRate,
                    _mutationChance = npc.Virus.MutationChance
                };
            }
            npcData._name = npc.Name;
            npcData._stamina = npc.Stamina;
            npcData._staminaDecayBase = npc.StaminaDecayBase;
            npcData._happiness = npc.Happiness;
            npcData._happinessDecayRate = npc.HappinessDecayRate;
            npcData._happinessDecayBase = npc.HappinessDecayBase;
            npcData._isHappinessDecayActive = npc.IsHappinessDecayActive;
            npcData._position = new(obj.transform.position.x, obj.transform.position.y, obj.transform.position.z);
            npcData._rotX = obj.transform.rotation.x;
            npcData._rotY = obj.transform.rotation.y;
            npcData._rotZ = obj.transform.rotation.z;
            npcData._rotW = obj.transform.rotation.w;
            npcData._isCommuting = nav.IsTravelling;
            npcData._home = new(nav.Home.transform.position.x, nav.Home.transform.position.y, nav.Home.transform.position.z);
            npcData._destination = new(nav.Destination.transform.position.x, nav.Destination.transform.position.y, nav.Destination.transform.position.z);
            data._npcDataList.Add(npcData);
        }
        #endregion NpcRegion
        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log("Saved game in " + Application.persistentDataPath);
    }

    public void LoadGame(GameManager gm, string fileName)
    {
        string filePath = Application.persistentDataPath + $"/saves/{fileName}";
        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new();
            FileStream stream = new(filePath, FileMode.Open);
            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();
            Debug.Log("Loaded game file");

            #region GameManager
            gm.GodMode = data._godMode;
            gm.MaxNPCs = data._maxNPCs;
            gm.PoliticalPower = data._politicalPower;
            gm.PoliticalPowerMultiplier = data._politicalPowerMultiplier;
            gm.HealthThreshold = data._healthThreshold;
            gm.StaminaThreshold = data._staminaThreshold;
            #endregion GameManager

            #region TimeManager
            gm.TimeManager.IsPaused = data._timeData._isPaused;
            gm.TimeManager.InGameMinute = data._timeData._inGameMinute;
            gm.TimeManager.InGameHour = data._timeData._inGameHour;
            gm.TimeManager.InGameDay = data._timeData._inGameDay;
            gm.TimeManager.SetTimeScale(0f);
            #endregion TimeManager

            #region NPCs
            if (gm.NPCs.Count != 0)
            {
                foreach (GameObject obj in gm.NPCs.ToList())
                {
                    gm.DestroyNPC(obj);
                }
                gm.NPCs.Clear();
            }
            foreach (NpcData npcData in data._npcDataList)
            {
                Vector3 position = new(npcData._position._posX, npcData._position._posY, npcData._position._posZ);
                Quaternion rotation = new(npcData._rotX, npcData._rotY, npcData._rotZ, npcData._rotW);
                GameObject obj = gm.SpawnNPC(position, rotation);
                NPC npc = obj.GetComponent<NPC>();
                Navigation nav = obj.GetComponent<Navigation>();
                npc.IsInfected = npcData._isInfected;
                npc.Health = npcData._health;
                if (npc.IsInfected)
                {
                    npc.Virus = ScriptableObject.CreateInstance<Virus>();
                    npc.Virus.CoughRate = npcData._virus._coughRate;
                    npc.Virus.StaminaDecayRate = npcData._virus._staminaDecayRate;
                    npc.Virus.HealthDecayRate = npcData._virus._healthDecayRate;
                    npc.Virus.MutationChance = npcData._virus._mutationChance;
                }
                npc.Name = npcData._name;
                npc.Stamina = npcData._stamina;
                npc.StaminaDecayBase = npcData._staminaDecayBase;
                npc.Happiness = npcData._happiness;
                npc.HappinessDecayRate = npcData._happinessDecayRate;
                npc.HappinessDecayBase = npcData._happinessDecayBase;
                npc.IsHappinessDecayActive = npcData._isHappinessDecayActive;
                nav.IsTravelling = npcData._isCommuting;
                nav.SetDestination(new Vector3(npcData._destination._posX, npcData._destination._posY, npcData._destination._posZ));
                nav.SetHome(new Vector3(npcData._home._posX, npcData._home._posY, npcData._home._posZ));
            }
            #endregion NPCs
        }
        else
        {
            Debug.LogError("Save file not found");
        }
    }

    public struct GameDisplayData
    {
        public float _politicalPower;
        public int _population;
        public int _totalInfected;
        public int _inGameMinute;
        public int _inGameHour;
        public int _inGameDay;
    }

    public GameDisplayData LoadDisplay(string fileName){
        BinaryFormatter formatter = new();
        string filePath = Application.persistentDataPath + "/saves/" + fileName;
        FileStream stream = new(filePath, FileMode.Open);
        GameData data = formatter.Deserialize(stream) as GameData;
        stream.Close();

        GameDisplayData displayData = new()
        {
            _politicalPower = data._politicalPower,
            _population = data._npcDataList.Count,
            _totalInfected = data._npcDataList.Count(npc => npc._isInfected),
            _inGameMinute = data._timeData._inGameMinute,
            _inGameHour = data._timeData._inGameHour,
            _inGameDay = data._timeData._inGameDay
        };
        return displayData;
    }

    public void RemoveSave(string savePath, string imagePath)
    {
        if (File.Exists(savePath) && File.Exists(imagePath))
        {
            Debug.Log("Save file deleted");
        }
        else
        {
            Debug.LogError("Save file not found");
        }
    }

    public void AddAutoSave(GameManager gm)
    {
        Debug.Log("currIndex: " + currIndex);
        Debug.Log("autosaves: " + _autoSaves.Count);
        if (_autoSaves.Count == 0)
        {
            FillList();
        }
        if(currIndex == 5){
            currIndex = 0;
        }
        if(_autoSaves.Count >= 5)
        {
            string rmImagePath = Application.persistentDataPath + "/images/Saves/" + _autoSaves[currIndex].Replace(".dat", ".png");
            string rmSavePath = Application.persistentDataPath + "/saves/" + _autoSaves[currIndex];
            RemoveSave(rmSavePath, rmImagePath);
            _autoSaves.RemoveAt(currIndex);
        }
        SaveGame(gm,$"autosave{currIndex}");
        _autoSaves.Add($"autosave{currIndex}.dat");
        string imageName = $"autosave{currIndex}.png";
        string imagePath = Application.persistentDataPath + "/images/Saves/" + imageName;
        ScreenCapture.CaptureScreenshot(imagePath);
        currIndex++;
        
    }

    public void loadRecentSave(GameManager gm)
    {
        string pattern = "*.dat";
        var dirInfo = new DirectoryInfo(Application.persistentDataPath + "/saves");
        var file = (from f in dirInfo.GetFiles(pattern) orderby f.LastWriteTime descending select f).First();
        string fileName = file.Name;
        LoadGame(gm, fileName);
    }

    void FillList()
    {
        _autoSaves = new List<string>();
        string[] files = Directory.GetFiles(Application.persistentDataPath + "/saves/");
        foreach (string file in files)
        {
            if (file.Contains("autosave") && file.Contains(".dat"))
            {
                string fileName = file.Replace(Application.persistentDataPath + "/saves/", "");
                _autoSaves.Add(fileName);
            }
        }
        currIndex = _autoSaves.Count;
    }
}
