using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager _instance;
    public static DataManager Instance { get { return _instance; } }

    // Used when determining if LifeSteal effect should be applied
    private static bool isLifeStealActive = false;
    public bool IsLifeStealActive
    {
        get { return isLifeStealActive; }
        set { isLifeStealActive = value; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else { _instance = this; }
    }
}
