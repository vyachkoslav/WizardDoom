using Player.UI;
using UnityEngine;

public class Key : Pickup
{
    [SerializeField] private string _keyName;
    private ItemsUI _itemsDisplay; 

    private void Start()
    {
        _itemsDisplay = FindAnyObjectByType<ItemsUI>();
    }

    protected override void PickupEffect()
    {
        if (!DataManager.Instance.CheckKeyInList(this)) 
        {
            DataManager.Instance.AddKeyToList(this);
            _itemsDisplay.UpdateItemDisplay(this);
            SoundManager.Instance.PlaySound3D("Key", transform.position);
        }
    }

    public override string ToString()
    {
        return _keyName;
    }
}
