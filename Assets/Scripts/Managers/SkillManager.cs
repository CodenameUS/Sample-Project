using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

/// <summary>
///                             SkillManager 
///                     1. ��ų ������ �ε�
///                     2. ��ų ���� 
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

    
    // ��ų ������ �ҷ�����
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
                Debug.LogWarning("Json �����͸� �Ľ��� �� ����.");
            }
        }
        else
        {
            Debug.LogWarning("SkillData.json ������ ����.");
        }
    }

    // ��ų �ν��Ͻ� ����
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
                Debug.Log($"���ǵ��� ���� ��ų ID : {data.Id}");
                return null;
        }
    }

    // ID�� ��ų ������ ��������
    public SkillData GetSkillDataById(string id)
    {
        if(skillDataDictionary != null && skillDataDictionary.TryGetValue(id, out var resultData))
        {
            return resultData;
        }
        else
        {
            Debug.LogWarning("ID�� �ش��ϴ� ��ų�����Ͱ� ����.");
            return null;
        }
    }

    // ��ų ���(Id ���)
    public void ExecuteSkill(string skillId, GameObject user)
    {
        var skillData = GetSkillDataById(skillId);
        if (skillData == null) return;

        Skill skill = CreateSkillInstance(skillData);
        skill.Activate(user);
    }
}
