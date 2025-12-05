using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

namespace Player.UI
{
    public class ItemsUI : MonoBehaviour
    {
        [SerializeField] private LayoutGroup _parent;
        [SerializeField] private Image _itemDisplay;

        public void UpdateItemDisplay(Key key)
        {
            if (DataManager.Instance.CheckKeyInList(key))
            {
                Instantiate(_itemDisplay, _parent.transform);
            }
        }
    }
}