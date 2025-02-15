using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

/// <summary>
///                             DataManager 
///                     1. �� ������ �ε� �� ���̺�
///                     2. �� �����Ϳ� ���� ������ �޼��� ���� 
///                     
/// </summary>
public class DataManager : Singleton<DataManager>
{
    private string playerDataPath;          // �÷��̾� ������ ������
    private string weaponItemDataPath;      // ���� ������ ������
    private string portionItemDataPath;     // ���� ������ ������
    private string armorItemDataPath;       // �� ������ ������

    private Dictionary<int, WeaponItemData> weponDataDictionary;
    private Dictionary<int, PortionItemData> portionDataDictionary;
    private Dictionary<int, ArmorItemData> armorDataDictionary;

    private PlayerData playerData;

    protected override void Awake()
    {
        base.Awake();
        InitAndLoadData();
    }

    private void InitAndLoadData()
    {
        playerDataPath = Path.Combine(Application.persistentDataPath, "Playerdata.json");
        weaponItemDataPath = Path.Combine(Application.persistentDataPath, "WeaponData.json");
        portionItemDataPath = Path.Combine(Application.persistentDataPath, "PortionData.json");
        armorItemDataPath = Path.Combine(Application.persistentDataPath, "ArmorData.json");

        playerData = LoadPlayerData();
        weponDataDictionary = LoadWeaponData();
        portionDataDictionary = LoadPortionData();
        armorDataDictionary = LoadArmorData();
    }

    // �÷��̾� ������ �ҷ�����
    private PlayerData LoadPlayerData()
    {
        if(File.Exists(playerDataPath))
        {
            string jsonData = File.ReadAllText(playerDataPath);
            var playerDTO = JsonConvert.DeserializeObject<PlayerDataDTO>(jsonData);

            if(playerDTO != null && playerDTO.Status != null)
            {
                PlayerData playerData = new PlayerData(playerDTO.Status[0]);
                return playerData;
            }
            else
            {
                Debug.LogWarning("PlayerData.json ���Ͽ� Status �迭�� �������");
                return default(PlayerData);
            }
        }
        else
        {
            Debug.LogWarning("PlayerData.json ������ ����.");
            return default(PlayerData);
        }
    }

    // ���� ������ �ҷ�����
    private Dictionary<int, WeaponItemData> LoadWeaponData()
    {
        if(File.Exists(weaponItemDataPath))
        {
            string jsonData = File.ReadAllText(weaponItemDataPath);
            var weaponDict = JsonConvert.DeserializeObject<Dictionary<string, List<WeaponItemDTO>>>(jsonData);

            if(weaponDict != null)
            {
                foreach(var category in weaponDict)
                {
                    Dictionary<int, WeaponItemData> dataDictionary = new Dictionary<int, WeaponItemData>();
                    // DTO�� WeaponItemData�� ��ȯ�Ͽ� ����
                    foreach (var weaponDTO in category.Value)
                    {
                        WeaponItemData weaponData = new WeaponItemData(weaponDTO);
                        dataDictionary[weaponData.ID] = weaponData;
                    }
                    return dataDictionary;
                }
            }
            else
            {
                Debug.LogWarning("Json �����͸� �Ľ��� �� �����ϴ�.");
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

            if (portionDict !=  null)
            {
                foreach (var category in portionDict)
                {
                    Dictionary<int, PortionItemData> dataDictionary = new Dictionary<int, PortionItemData>();
                    // DTO�� WeaponItemData�� ��ȯ�Ͽ� ����
                    foreach (var portionDTO in category.Value)
                    {
                        PortionItemData portionData = new PortionItemData(portionDTO);
                        dataDictionary[portionData.ID] = portionData;
                    }
                    return dataDictionary;
                }
            }
            else
            {
                Debug.LogWarning("Json �����͸� �Ľ��� �� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogWarning("�ҷ��� ������ �����ϴ�.");
        }
        return new Dictionary<int, PortionItemData>();
    }

    // �� ������ �ҷ�����
    private Dictionary<int, ArmorItemData> LoadArmorData()
    {
        if(File.Exists(armorItemDataPath))
        {
            string jsonData = File.ReadAllText(armorItemDataPath);
            var armorDict = JsonConvert.DeserializeObject<Dictionary<string, List<ArmorItemDTO>>>(jsonData);

            if(armorDict != null)
            {
                foreach(var category in armorDict)  // "Top", "Shoes", "Gloves"
                {
                    Dictionary<int, ArmorItemData> dataDictionary = new Dictionary<int, ArmorItemData>();
                    foreach (var armorDTO in category.Value)
                    {
                        ArmorItemData armorData = new ArmorItemData(armorDTO);
                        dataDictionary[armorData.ID] = armorData;
                    }
                    return dataDictionary;
                }
            }
            else
            {
                Debug.LogWarning("Json �����͸� �Ľ��� �� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogWarning("�ҷ��� ������ �����ϴ�.");
        }
        return new Dictionary<int, ArmorItemData>();
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
            Debug.LogWarning("ID�� �ش��ϴ� ���ⵥ���Ͱ� ����.");
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
            Debug.LogWarning("ID�� �ش��ϴ� ���ǵ����Ͱ� ����.");
            return null;
        }
    }

    // ID�� �� ������ ��������
    public ArmorItemData GetArmorDataById(int id)
    {
        if(armorDataDictionary != null && armorDataDictionary.TryGetValue(id, out var resultData))
        {
            return resultData;
        }
        else
        {
            Debug.LogWarning("ID�� �ش��ϴ� �������Ͱ� ����.");
            return null;
        }
    }
    // �÷��̾� �����Ϳ� ���� ������ �޼���
    public PlayerData GetPlayerData()
    {
        return playerData;
    }

    //  ������ ����(���ʸ�)
    public void SaveData<T>(T data, string fileName = "")
    {
        string jsonData = JsonUtility.ToJson(data);
        string filePath = Path.Combine(Application.persistentDataPath, fileName + ".json");
        File.WriteAllText(filePath, jsonData);

        if (typeof(T) == typeof(PlayerData))
        {
            playerData = data as PlayerData;
        }
    }
}
