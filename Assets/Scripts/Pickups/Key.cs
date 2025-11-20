using UnityEngine;

public class Key : Pickup
{
    protected override void PickupEffect()
    {
        DataManager.Instance.AddKeyToList(this);
    }
}
