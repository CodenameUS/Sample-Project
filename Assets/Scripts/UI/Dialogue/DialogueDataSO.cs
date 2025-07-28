using UnityEngine;

/*
                            << DialogueDataSO >>

        - 대화창에 표시된 대화 데이터 클래스(Scriptable Object)

        - 기호 "--" 를 기준으로 페이지 나누기
 */


[CreateAssetMenu(fileName = "NewDialogue", menuName = "Datas/DialogueData")]
public class DialogueDataSO : ScriptableObject
{
    public string npcName;              // NPC 이름
    [TextArea(2, 10)]
    public string[] sentences;          // 대화 내용
}
