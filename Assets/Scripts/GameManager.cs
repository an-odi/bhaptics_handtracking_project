using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static HapticManager hapticManager;

    public bool isDebugMode = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        hapticManager = HapticManager.instance;
        if (hapticManager != null)
        {
            Debug.Log("HapticManager instance found.");
        }
        else
        {
            Debug.LogError("HapticManager instance not found!");
        }
    }
}
