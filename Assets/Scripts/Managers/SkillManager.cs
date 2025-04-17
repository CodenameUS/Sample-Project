using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

/// <summary>
///                             SkillManager 
///                     1. 스킬 데이터 로드
///                     2. 스킬 실행 
///                     
/// </summary>

public class SkillManager : Singleton<SkillManager>
{
    public List<Skill> playerSkills = new();

    private Dictionary<string, SkillData> skillDataDictionary = new();

    protected override void Awake()
    {
        base.Awake();
        LoadSkillData();
    }

    
    // 스킬 데이터 불러오기
    private void LoadSkillData()
    {
        string path = Path.Combine(Application.persistentDataPath, "SkillData.json");

        if(File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            var skillDict = JsonConvert.DeserializeObject<Dictionary<string, List<SkillDataDTO>>>(jsonData);

            if(skillDict != null)
            {
                foreach(var category in skillDict)
                {
                    foreach(var skillDTO in category.Value)
                    {
                        SkillData skillData = new SkillData(skillDTO);
                        skillDataDictionary[skillData.Id] = skillData;
                    }
                }
            }
            else
            {
                Debug.LogWarning("Json 데이터를 파싱할 수 없음.");
            }
        }
        else
        {
            Debug.LogWarning("SkillData.json 파일이 없음.");
        }
    }

    // 스킬 인스턴스 생성
    public Skill CreateSkillInstance(SkillData data)
    {
        switch (data.Id)
        {
            case "buff":
                return new Buff(data);
            case "iceshot":
                return new IceShot(data);
            case "slash":
                return new Slash(data);
            default:
                Debug.Log($"정의되지 않은 스킬 ID : {data.Id}");
                return null;
        }
    }

    // ID로 스킬 데이터 가져오기
    public SkillData GetSkillDataById(string id)
    {
        if(skillDataDictionary != null && skillDataDictionary.TryGetValue(id, out var resultData))
        {
            return resultData;
        }
        else
        {
            Debug.LogWarning("ID에 해당하는 스킬데이터가 없음.");
            return null;
        }
    }

    // 스킬 사용(Id 기반)
    public void ExecuteSkill(string skillId, GameObject user)
    {
        var skillData = GetSkillDataById(skillId);
        if (skillData == null) return;

        Skill skill = CreateSkillInstance(skillData);
        skill.Activate(user);
    }
}
