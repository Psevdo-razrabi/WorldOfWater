using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if( _instance == null )
            {
                _instance = FindAnyObjectByType<T>();

                if( _instance == null )
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    _instance = singletonObject.GetComponent<T>();
                }
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if( _instance == null )
        {
            _instance = this as T;
            DontDestroyOnLoad(Instance);
        }
        else if( _instance != this )
        {
            Destroy(gameObject);
        }
    }
}
