using Player.UI;
using UnityEngine;

public class Key : Pickup
{
    private ItemsUI _itemsDisplay; 

    private void Start()
    {
        _itemsDisplay = FindAnyObjectByType<ItemsUI>();
    }

    protected override void PickupEffect()
    {
        DataManager.Instance.AddKeyToList(this);
        _itemsDisplay.UpdateItemDisplay(this);
    }
}
