using Photon.Pun;

/*
                            << SingletonPun >>

        - 멀티용 싱글톤 제네릭 클래스
 */

public class SingletonPun<T> : MonoBehaviourPun where T : MonoBehaviourPun
{
    private static T instance;
    public static T Instance => instance;

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this as T;
        DontDestroyOnLoad(gameObject);
    }
}
