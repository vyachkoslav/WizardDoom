using System.Collections.Generic;
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

    // Player's key collection
    private static List<Key> keyList = new List<Key>();
    public List<Key> KeyList { get {return keyList; } }

    public void AddKeyToList(Key key)
    {
        if (!keyList.Contains(key))
        {
            keyList.Add(key);
        }
        foreach (Key k in keyList){Debug.Log(k);}
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
