using Player.UI;
using UnityEngine;

public class Key : Pickup
{
    private ItemsUI _itemsDisplay; 

    protected override void Start()
    {
        _itemsDisplay = FindAnyObjectByType<ItemsUI>();;
        if (DataManager.Instance.CheckKeyInList(this))
        {
            this.gameObject.SetActive(false);
        }
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
        return "Collected " + _id;
    }
}
