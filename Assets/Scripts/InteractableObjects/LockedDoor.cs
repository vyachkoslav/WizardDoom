using UnityEngine;

public class LockedDoor : Door
{
    [SerializeField] private Key _key;

    public override void Interact()
    {
        // Check if assigned key is already obtained
        if (!DataManager.Instance.IsFighting && DataManager.Instance.CheckKeyInList(_key))
        {
            StartCoroutine(OpenDoor());
        }
    }

    public override string DisplayText()
    {
        if (DataManager.Instance.IsFighting)
        {
            return "Defeat all enemies!";
        }
        else if (DataManager.Instance.CheckKeyInList(_key))
        {
            return base.DisplayText();
        }
        else { return "Find '" + _key.ToString() + "' to unlock door."; }
    }
}