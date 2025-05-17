using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Audio;

[System.Serializable]
public class BGMData
{
    public string id;
    public string clipName;
    public float volume;
    public bool loop;
}

[System.Serializable]
public class BGMList
{
    public List<BGMData> bgmList;
}

[System.Serializable]
public class SFXData
{
    public string id;
    public string clipName;
    public float volume;
    public bool loop;
}

[System.Serializable]
public class SFXList
{
    public List<SFXData> sfxList;
}


public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup bgmGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;

    #region ** BGM Settings **
    private AudioSource bgmSource;
    #endregion

    #region ** SFX Settings **
    private List<AudioSource> sfxSource;
    private int currentSFXIndex = 0;
    private int sfxPoolSize = 10;                                       // SFX Ǯ ������
    #endregion

    private Dictionary<string, BGMData> bgmDictionary;                  // BGM ĳ�� ������
    private Dictionary<string, SFXData> sfxDictionary;                  // SFX ĳ�� ������
    private Dictionary<string, List<AudioSource>> loopedSFXMap = new(); // �ݺ�����Ǵ� SFX�� �����ϱ����� ��ųʸ�

    private const string MASTER_VOL = "MasterVolume";
    private const string BGM_VOL = "BGMVolume";
    private const string SFX_VOL = "SFXVolume";

    protected override void Awake()
    {
        base.Awake();
        LoadBGMData();
        LoadSFXData();
        InitAudioSources();
    }

    // �ʱ�ȭ
    private void InitAudioSources()
    {
        // BGM 
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmSource = bgmObject.AddComponent<AudioSource>();
        bgmSource.outputAudioMixerGroup = bgmGroup;

        // SFX
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxSource = new List<AudioSource>();

        for (int i = 0; i < sfxPoolSize; i++)
        {
            AudioSource source = sfxObject.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = sfxGroup;
            sfxSource.Add(source);
        }

    }

    // BGM ������ �ҷ�����
    private void LoadBGMData()
    {
        string path = Path.Combine(Application.persistentDataPath, "bgmData.json");

        if(File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            BGMList bgmList = JsonUtility.FromJson<BGMList>(jsonData);

            bgmDictionary = new Dictionary<string, BGMData>();
            foreach(var bgm in bgmList.bgmList)
            {
                bgmDictionary[bgm.id] = bgm;
            }
        }
        else
        {
            Debug.LogWarning("bgmData.json ������ ����.");
        }
    }

    // BGM ������ �ҷ�����
    private void LoadSFXData()
    {
        string path = Path.Combine(Application.persistentDataPath, "sfxData.json");

        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            SFXList list = JsonUtility.FromJson<SFXList>(jsonData);

            sfxDictionary = new Dictionary<string, SFXData>();
            foreach (var sfx in list.sfxList)
            {
                sfxDictionary[sfx.id] = sfx;
            }
        }
        else
        {
            Debug.LogWarning("sfxData.json ������ ����.");
        }
    }

    
    // BGM ����(ID)
    public void PlayBGM(string bgmId)
    {
        // ���� BGM ����
        if(bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }

        // BGM ID�� BGM ����
        if(bgmDictionary.TryGetValue(bgmId, out BGMData bgmData))
        {
            ResourceManager.Instance.LoadSound(bgmId, bgm =>
            {
                // ����
                if (bgm != null)
                {
                    bgmSource.clip = bgm;
                    bgmSource.volume = bgmData.volume;
                    bgmSource.loop = bgmData.loop;
                    bgmSource.Play();
                }
                else
                {
                    Debug.Log($"����� Ŭ���� ã�� �� ���� : {bgmData.clipName}");
                }
            });
        }
        else
        {
            Debug.Log($"BGM ID�� ã�� �� ���� : {bgmId}");
        }
    }

    // SFX ����(ID)
    public void PlaySFX(string sfxId, float delay = 0f)
    {
        // BGM ID�� BGM ����
        if (sfxDictionary.TryGetValue(sfxId, out SFXData sfxData))
        {
            ResourceManager.Instance.LoadSound(sfxId, sfx =>
            {
                // ����
                if (sfx != null)
                {
                    for(int index = 0; index < sfxPoolSize;index++)
                    {
                        int loopIndex = (index + currentSFXIndex) % sfxPoolSize;

                        if (sfxSource[loopIndex].isPlaying)
                            continue;

                        currentSFXIndex = loopIndex;
                        sfxSource[currentSFXIndex].clip = sfx;
                        sfxSource[currentSFXIndex].volume = sfxData.volume;
                        sfxSource[currentSFXIndex].loop = sfxData.loop;
                        sfxSource[currentSFXIndex].PlayDelayed(delay);

                        // �ݺ�����Ǵ� SFX �� �������
                        if(sfxSource[currentSFXIndex].loop)
                        {
                            // ���� ���
                            if (!loopedSFXMap.ContainsKey(sfxId))
                                loopedSFXMap[sfxId] = new List<AudioSource>();

                            loopedSFXMap[sfxId].Add(sfxSource[currentSFXIndex]);
                        }
                        else
                        {
                            StartCoroutine(CleanupClipAfterPlay(sfxSource[currentSFXIndex]));
                        }

                        break;
                    }
                }
                else
                {
                    Debug.Log($"����� Ŭ���� ã�� �� ���� : {sfxData.clipName}");
                }
            });
        }
        else
        {
            Debug.Log($"BGM ID�� ã�� �� ���� : {sfxId}");
        }
    }

    // SFX ����(ID)
    public void StopSFX(string sfxId)
    {
        // ID�� ������ SFX Ž��
        if(loopedSFXMap.TryGetValue(sfxId, out var sources))
        {
            // ID Ű�� ������ ��� AudioSource ����
            foreach(var src in sources)
            {
                if (src.isPlaying)
                    src.Stop();
                src.clip = null;
            }
            
            // ���� ����
            loopedSFXMap.Remove(sfxId);
        }
    }

    // ������ ��������
    public void SetMasterVolume(float value)
    {
        // 0.0001~1 => -80db~0db
        float volume = Mathf.Clamp(value, 0.0001f, 1f); // �ּҰ� ����
        float dB = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat(MASTER_VOL, dB);
    }

    // BGM ��������
    public void SetBGMVolume(float value)
    {
        // 0.0001~1 => -80db~0db
        float volume = Mathf.Clamp(value, 0.0001f, 1f); // �ּҰ� ����
        float dB = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat(BGM_VOL, Mathf.Log10(value) * 20);
    }

    // SFX ��������
    public void SetSFXVolume(float value)
    {
        // 0.0001~1 => -80db~0db
        float volume = Mathf.Clamp(value, 0.0001f, 1f); // �ּҰ� ����
        float dB = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat(SFX_VOL, Mathf.Log10(value) * 20);
    }


    // ���� ��� �� AudioSource ����
    private IEnumerator CleanupClipAfterPlay(AudioSource source)
    {
        yield return new WaitWhile(() => source.isPlaying);

        if(!source.loop)
        {
            source.clip = null;
        }
    }
}
