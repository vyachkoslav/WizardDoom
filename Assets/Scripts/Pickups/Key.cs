using Player.UI;
using UnityEngine;

public class Key : Pickup
{
    [SerializeField] private Material _keyMaterial;

    private ItemsUI _itemsDisplay; 

    public Material KeyMaterial { get { return _keyMaterial; }   }


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
