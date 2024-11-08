using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    // 데이터 경로
    private string dataPath;

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

        dataPath = Path.Combine(Application.persistentDataPath + "/Data", "/data.json");

        // 임시
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
        // 임의 플레이어 데이터생성
        playerData = new PlayerData(1000f, 1000f, 1000f, 1000f, 3f, 10f);
    }

    private void LoadCachedData()
    {
        playerData = LoadData<PlayerData>("PlayerData");
    }

    //  데이터 저장
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

    // 데이터 불러오기
    public T LoadData<T>(string fileName = "")
    {
        string filePath = string.IsNullOrEmpty(fileName) ? dataPath : Path.Combine(Application.persistentDataPath, fileName + ".json");
        // 저장된 파일이 있을 때
        if(File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            return JsonUtility.FromJson<T>(jsonData);
        }
        // 저장된 파일이 없을 때
        else
        {
            return default(T); 
        }
    }

    // 플레이어 데이터에 대한 접근자 메서드
    public PlayerData GetPlayerData()
    {
        return playerData;
    }

}
