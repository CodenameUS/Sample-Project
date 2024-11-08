using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    // ������ ���
    private string dataPath;

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

        dataPath = Path.Combine(Application.persistentDataPath + "/Data", "/data.json");

        // �ӽ�
        if(!File.Exists(dataPath))
        {
            InitData();
        }
        else
        {
            LoadCachedData();
        }
    }

    private void InitData()
    {
        // ���� �÷��̾� �����ͻ���
        playerData = new PlayerData(1000f, 1000f, 1000f, 1000f, 3f, 10f);
    }

    private void LoadCachedData()
    {
        playerData = LoadData<PlayerData>("PlayerData");
    }

    //  ������ ����
    public void SaveData<T>(T data, string fileName = "")
    {
        string jsonData = JsonUtility.ToJson(data);
        string filePath = string.IsNullOrEmpty(fileName) ? dataPath : Path.Combine(Application.persistentDataPath, fileName + ".json");
        File.WriteAllText(filePath, jsonData);

        if(typeof(T) == typeof(PlayerData))
        {
            playerData = data as PlayerData;
        }
    }

    // ������ �ҷ�����
    public T LoadData<T>(string fileName = "")
    {
        string filePath = string.IsNullOrEmpty(fileName) ? dataPath : Path.Combine(Application.persistentDataPath, fileName + ".json");
        // ����� ������ ���� ��
        if(File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            return JsonUtility.FromJson<T>(jsonData);
        }
        // ����� ������ ���� ��
        else
        {
            return default(T); 
        }
    }

    // �÷��̾� �����Ϳ� ���� ������ �޼���
    public PlayerData GetPlayerData()
    {
        return playerData;
    }

}
