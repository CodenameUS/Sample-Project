using UnityEngine;

/*
                            << DontDestroy >>

        - 씬 변경간에 파괴되지 않을 오브젝트에 부착
 */

public class DontDestroy : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);    
    }
}
