using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

/// <summary>
///                             DataManager 
///                     1. �� ������ �ε� �� ���̺�
///                     2. �� �����Ϳ� ���� ������ �޼��� ���� 
///                     3. �ӽ� ������ ����
/// </summary>
public class DataManager : MonoBehaviour
{
    // �̱���
    private static DataManager instance;

    private string playerDataPath;          // �÷��̾� ������ ������
    private string weaponItemDataPath;      // ���� ������ ������
    private string portionItemDataPath;     // ���� ������ ������

    private Dictionary<int, WeaponItemData> weponDataDictionary;
    private Dictionary<int, PortionItemData> portionDataDictionary;

    private PlayerData playerData;

    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                // �ν��Ͻ��� �����ϴ��� �ѹ� �� üũ
                instance = FindObjectOfType<DataManager>();

                // ���ٸ�
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

        // �ӽ�(�÷��̾� �����͸�)
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
        // ���� �÷��̾� �����ͻ���
        playerData = new PlayerData(1000f, 1000f, 1000f, 1000f, 3f, 10f);
        SaveData(playerData, "PlayerData");
    }

    private void LoadCachedData()
    {
        playerData = LoadData<PlayerData>("PlayerData");
    }

    //  ������ ����
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

    // ������ �ҷ�����
    public T LoadData<T>(string fileName = "")
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName + ".json");

        // ����� ������ ���� ��
        if(File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            return JsonUtility.FromJson<T>(jsonData);
        }
        // ����� ������ ���� ��
        else
        {
            InitData();
            return default(T); 
        }
    }

    // ���� ������ �ҷ�����
    private Dictionary<int, WeaponItemData> LoadWeaponData()
    {
        if(File.Exists(weaponItemDataPath))
        {
            string jsonData = File.ReadAllText(weaponItemDataPath);
            var weaponDict = JsonConvert.DeserializeObject<Dictionary<string, List<WeaponItemDTO>>>(jsonData);
            
            // Sword �׸� �����ͼ� ����
            if(weaponDict.TryGetValue("Sword", out List<WeaponItemDTO> weaponList))
            {
                Dictionary<int, WeaponItemData> dataDictionary = new Dictionary<int, WeaponItemData>();

                // DTO�� WeaponItemData�� ��ȯ�Ͽ� ����
                foreach(var weaponDTO in weaponList)
                {
                    WeaponItemData weaponData = new WeaponItemData(weaponDTO);
                    dataDictionary[weaponData.ID] = weaponData;
                }
                return dataDictionary;
            }
            else
            {
                Debug.LogWarning("WeaponData.json ���Ͽ� 'Sword'Ű�� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogWarning("�ҷ��� ������ �����ϴ�.");
        }
        return new Dictionary<int, WeaponItemData>();
    }

    // ���� ������ �ҷ�����
    private Dictionary<int, PortionItemData> LoadPortionData()
    {
        if (File.Exists(portionItemDataPath))
        {
            string jsonData = File.ReadAllText(portionItemDataPath);
            var portionDict = JsonConvert.DeserializeObject<Dictionary<string, List<PortionItemDTO>>>(jsonData);

            // Portion �׸� �����ͼ� ����
            if (portionDict.TryGetValue("Portion", out List<PortionItemDTO> portionList))
            {
                Dictionary<int, PortionItemData> dataDictionary = new Dictionary<int, PortionItemData>();

                // DTO�� WeaponItemData�� ��ȯ�Ͽ� ����
                foreach (var portionDTO in portionList)
                {
                    PortionItemData portionData = new PortionItemData(portionDTO);
                    dataDictionary[portionData.ID] = portionData;
                }
                return dataDictionary;
            }
            else
            {
                Debug.LogWarning("PortionData.json ���Ͽ� 'Portion'Ű�� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogWarning("�ҷ��� ������ �����ϴ�.");
        }
        return new Dictionary<int, PortionItemData>();
    }

    // ID�� ���� ������ ��������
    public WeaponItemData GetDataById(int id)
    {
        if(weponDataDictionary != null && weponDataDictionary.TryGetValue(id, out var resultData))
        {
            return resultData;
        }
        else
        {
            Debug.LogWarning("ID�� �ش��ϴ� �����Ͱ� ����.");
            return null;
        }
    }

    // ID�� ���� ������ ��������
    public PortionItemData GetPortionDataById(int id)
    {
        if (portionDataDictionary != null && portionDataDictionary.TryGetValue(id, out var resultData))
        {
            return resultData;
        }
        else
        {
            Debug.LogWarning("ID�� �ش��ϴ� �����Ͱ� ����.");
            return null;
        }
    }

    // �÷��̾� �����Ϳ� ���� ������ �޼���
    public PlayerData GetPlayerData()
    {
        return playerData;
    }
}
