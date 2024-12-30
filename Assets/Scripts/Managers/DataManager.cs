using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

/// <summary>
///                             DataManager 
///                     1. 각 데이터 로드 및 세이브
///                     2. 각 데이터에 대한 접근자 메서드 제공 
///                     3. 임시 데이터 생성
/// </summary>
public class DataManager : MonoBehaviour
{
    // 싱글톤
    private static DataManager instance;

    private string playerDataPath;          // 플레이어 데이터 저장경로
    private string weaponItemDataPath;      // 무기 데이터 저장경로
    private string portionItemDataPath;     // 포션 데이터 저장경로

    private Dictionary<int, WeaponItemData> weponDataDictionary;
    private Dictionary<int, PortionItemData> portionDataDictionary;

    private PlayerData playerData;

    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                // 인스턴스가 존재하는지 한번 더 체크
                instance = FindObjectOfType<DataManager>();

                // 없다면
                if (instance == null)
                {
                    GameObject obj = new GameObject("DataManager");
                    instance = obj.AddComponent<DataManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        playerDataPath = Path.Combine(Application.persistentDataPath, "Playerdata.json");
        weaponItemDataPath = Path.Combine(Application.persistentDataPath, "WeaponData.json");
        portionItemDataPath = Path.Combine(Application.persistentDataPath, "PortionData.json");

        // 임시(플레이어 데이터만)
        if (!File.Exists(playerDataPath))
        {
            InitData();
        }
        else
        {
            LoadCachedData();
        }

        weponDataDictionary = LoadWeaponData();
        portionDataDictionary = LoadPortionData();
    }

    private void InitData()
    {
        // 임의 플레이어 데이터생성
        playerData = new PlayerData(1000f, 1000f, 1000f, 1000f, 3f, 10f);
        SaveData(playerData, "PlayerData");
    }

    private void LoadCachedData()
    {
        playerData = LoadData<PlayerData>("PlayerData");
    }

    //  데이터 저장
    public void SaveData<T>(T data, string fileName = "")
    {
        string jsonData = JsonUtility.ToJson(data);
        string filePath = Path.Combine(Application.persistentDataPath, fileName + ".json");
        File.WriteAllText(filePath, jsonData);

        if(typeof(T) == typeof(PlayerData))
        {
            playerData = data as PlayerData;
        }
    }

    // 데이터 불러오기
    public T LoadData<T>(string fileName = "")
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName + ".json");

        // 저장된 파일이 있을 때
        if(File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            return JsonUtility.FromJson<T>(jsonData);
        }
        // 저장된 파일이 없을 때
        else
        {
            InitData();
            return default(T); 
        }
    }

    // 무기 데이터 불러오기
    private Dictionary<int, WeaponItemData> LoadWeaponData()
    {
        if(File.Exists(weaponItemDataPath))
        {
            string jsonData = File.ReadAllText(weaponItemDataPath);
            var weaponDict = JsonConvert.DeserializeObject<Dictionary<string, List<WeaponItemDTO>>>(jsonData);
            
            // Sword 항목 가져와서 저장
            if(weaponDict.TryGetValue("Sword", out List<WeaponItemDTO> weaponList))
            {
                Dictionary<int, WeaponItemData> dataDictionary = new Dictionary<int, WeaponItemData>();

                // DTO를 WeaponItemData로 변환하여 저장
                foreach(var weaponDTO in weaponList)
                {
                    WeaponItemData weaponData = new WeaponItemData(weaponDTO);
                    dataDictionary[weaponData.ID] = weaponData;
                }
                return dataDictionary;
            }
            else
            {
                Debug.LogWarning("WeaponData.json 파일에 'Sword'키가 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning("불러올 파일이 없습니다.");
        }
        return new Dictionary<int, WeaponItemData>();
    }

    // 포션 데이터 불러오기
    private Dictionary<int, PortionItemData> LoadPortionData()
    {
        if (File.Exists(portionItemDataPath))
        {
            string jsonData = File.ReadAllText(portionItemDataPath);
            var portionDict = JsonConvert.DeserializeObject<Dictionary<string, List<PortionItemDTO>>>(jsonData);

            // Portion 항목 가져와서 저장
            if (portionDict.TryGetValue("Portion", out List<PortionItemDTO> portionList))
            {
                Dictionary<int, PortionItemData> dataDictionary = new Dictionary<int, PortionItemData>();

                // DTO를 WeaponItemData로 변환하여 저장
                foreach (var portionDTO in portionList)
                {
                    PortionItemData portionData = new PortionItemData(portionDTO);
                    dataDictionary[portionData.ID] = portionData;
                }
                return dataDictionary;
            }
            else
            {
                Debug.LogWarning("PortionData.json 파일에 'Portion'키가 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning("불러올 파일이 없습니다.");
        }
        return new Dictionary<int, PortionItemData>();
    }

    // ID로 무기 데이터 가져오기
    public WeaponItemData GetDataById(int id)
    {
        if(weponDataDictionary != null && weponDataDictionary.TryGetValue(id, out var resultData))
        {
            return resultData;
        }
        else
        {
            Debug.LogWarning("ID에 해당하는 데이터가 없음.");
            return null;
        }
    }

    // ID로 포션 데이터 가져오기
    public PortionItemData GetPortionDataById(int id)
    {
        if (portionDataDictionary != null && portionDataDictionary.TryGetValue(id, out var resultData))
        {
            return resultData;
        }
        else
        {
            Debug.LogWarning("ID에 해당하는 데이터가 없음.");
            return null;
        }
    }

    // 플레이어 데이터에 대한 접근자 메서드
    public PlayerData GetPlayerData()
    {
        return playerData;
    }
}
